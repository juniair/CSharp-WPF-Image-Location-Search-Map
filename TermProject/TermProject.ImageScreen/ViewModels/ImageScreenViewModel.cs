using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TermProject.ImageScreen.ViewModels
{
    public class ImageScreenViewModel : BindableBase
    {
        private string applicationName = "TermProject";

        private string[] Scopes;

        private UserCredential Credential;

        private DriveService Service;

        private FilesResource.ListRequest ListRequest;

        private string imageUri;
        public string ImageUri
        {
            get { return imageUri; }
            set { SetProperty(ref imageUri, value); }
        }



        public ImageScreenViewModel()
        {
            ImageUri = null;
            
            init();            
        }

        private void init()
        {
            Scopes = new string[] { DriveService.Scope.Drive };
            Credential = GetUserCredential();
            Service = GetDriveService();
            ListRequest = Service.Files.List();
            ListRequest.Fields = "nextPageToken, files(imageMediaMetadata, id, name, thumbnailLink)";
            IList<Google.Apis.Drive.v3.Data.File> files = ListRequest.Execute().Files;
            bool isGet = false;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.ImageMediaMetadata != null && file.ImageMediaMetadata.Location != null)
                    {
                        if (!isGet)
                        {
                            ImageUri = file.ThumbnailLink;
                            isGet = true;
                        }

                    }
                }
            }
        }

        private ImageSource GetImageFile(Google.Apis.Drive.v3.Data.File file)
        {


            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(file.ThumbnailLink, UriKind.Relative);
            bi.EndInit();


            return bi;

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
