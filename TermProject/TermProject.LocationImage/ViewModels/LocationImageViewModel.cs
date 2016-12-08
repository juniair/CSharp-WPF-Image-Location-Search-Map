using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TermProject.Infra;

namespace TermProject.LocationImage.ViewModels
{
    public class LocationImageViewModel : BindableBase
    {

        private string applicationName = "TermProject";

        private string[] Scopes { get; set; }

        private UserCredential Credential { get; set; }

        private DriveService Service { get; set; }

        private FilesResource.ListRequest ListRequest { get; set; }

        private Image imageFile;
        public Image ImageFile
        {
            get { return imageFile; }
            set { SetProperty(ref imageFile, value); }
        }

        

        public LocationImageViewModel()
        {
            ImageFile = null;
            //init();            
        }

        private void init()
        {
            Scopes = new string[] { DriveService.Scope.Drive };
            Credential = GetUserCredential();
            Service = GetDriveService();
            ListRequest = GoogleDriveApiSingleTon.Instance.GetListRequset();
            ListRequest.Fields = "nextPageToken, files(imageMediaMetadata, id, name, thumbnailLink)";
            IList<Google.Apis.Drive.v3.Data.File> files = ListRequest.Execute().Files;
            bool isGet = false;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.ImageMediaMetadata != null && file.ImageMediaMetadata.Location != null)
                    {
                        if(!isGet)
                        {
                            ImageFile = GetImageFile(file);
                            isGet = true;
                        }
                        
                    }
                }
            }
        }

        private Image GetImageFile(Google.Apis.Drive.v3.Data.File file)
        {
            Image img = new Image();
            img.Width = Double.NaN;
            img.Height = Double.NaN;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(file.ThumbnailLink, UriKind.Relative);
            bi.EndInit();


            img.Source = bi;

            return img;

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
                    "juniair.kim",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
        }

        private DriveService GetDriveService()
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = applicationName,
            });
        }
    }
}
