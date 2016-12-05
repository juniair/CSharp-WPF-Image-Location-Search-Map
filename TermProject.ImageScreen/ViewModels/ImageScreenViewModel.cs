using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TermProject.Infra;

namespace TermProject.ImageScreen.ViewModels
{
    public class ImageScreenViewModel : BindableBase
    {

        public IEventAggregator EA { get; set; }

        private string ApplicationName;


        private List<Google.Apis.Drive.v3.Data.File> Files;
        private UserCredential Credential;
        private DriveService Service;
        private FilesResource.ListRequest ListRequest;
        private int Count = 0;

        
        private string[] Scopes = { DriveService.Scope.Drive };
        

        #region Property
        private string imageFile;
        public string ImageFile
        {
            get { return imageFile; }
            set { SetProperty(ref imageFile, value); }
        }
        #endregion

        #region Command
        public ICommand NextImageCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        #endregion

        public ImageScreenViewModel(IEventAggregator ea)
        {
            EA = ea;
            this.ApplicationName = "TermProject";
            init();
            NextImageCommand = new DelegateCommand<object>(NextEvnet);
            PreviousImageCommand = new DelegateCommand<object>(PrevEvent);
            RefreshCommand = new DelegateCommand<object>(RefreshEvent);
            ImageFile = this.Files.ElementAt(0).ThumbnailLink;
            EA.GetEvent<CreateMakerEvent>().Publish(this.Files.ElementAt(0));
        }


        #region Command Method
        private void RefreshEvent(Object obj)
        {
            init();
        }

        private void PrevEvent(Object obj)
        {
            this.Count = (this.Count > 0) ? (this.Count - 1) : Files.Count-1;

            ImageFile = this.Files.ElementAt(Count).ThumbnailLink;
            EA.GetEvent<CreateMakerEvent>().Publish(Files.ElementAt(Count));
        }

        private void NextEvnet(Object obj)
        {

            this.Count = (this.Count < Files.Count-1) ? (this.Count + 1) : 0;
            ImageFile = this.Files.ElementAt(Count).ThumbnailLink;
            EA.GetEvent<CreateMakerEvent>().Publish(Files.ElementAt(Count));
        }
        #endregion


        private void init()
        {
            

            Credential = GetUserCredential();
            Service = GetDriverService();
            ListRequest = Service.Files.List();
            ListRequest.Fields = "nextPageToken, files(imageMediaMetadata, thumbnailLink, id, name)";
            IList<Google.Apis.Drive.v3.Data.File> Files = ListRequest.Execute().Files;

            if (this.Files == null)
            {
                this.Files = new List<Google.Apis.Drive.v3.Data.File>();
            }

            if (Files != null && Files.Count > 0)
            {
                foreach(var file in Files)
                {
                    if(file.ImageMediaMetadata != null && file.ImageMediaMetadata.Location != null)
                    {
                        if(!this.Files.Contains(file))
                            this.Files.Add(file);
                    }
                }
                
            }
        }

        private UserCredential GetUserCredential()
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/termproject.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
        }

        private DriveService GetDriverService()
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = this.Credential,
                ApplicationName = this.ApplicationName,
            });
        }
    }
}
