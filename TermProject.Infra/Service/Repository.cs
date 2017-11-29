using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TermProject.Infra.Service
{
    public class GoogleDriveRepository : IRepository
    {

        private readonly string _applicationName = "TermProject";
        private string ApplicationName { get { return _applicationName; } }

        private readonly string _apikey = @"pack://application:,,,/TermProject.Infra;component/API/client_secret.json";
        private string ApiKey { get { return _apikey; } }

        private readonly string[] _scopes = { DriveService.Scope.Drive };
        private string[] Scopes { get { return _scopes; } }

        private DriveService GoogleDriveService { get; set; }


        public ObservableCollection<Google.Apis.Drive.v3.Data.File> Files { get; private set; }
        public int FileCount { get { return Files.Count; } }

        public GoogleDriveRepository()
        {
            InitializeAsync();
        }

        public Google.Apis.Drive.v3.Data.File GetFile(int index)
        {
            return Files[index];
        }


        private async Task<IEnumerable<Google.Apis.Drive.v3.Data.File>> GetFilesAsync()
        {
            var request = GoogleDriveService.Files.List();
            request.Fields = "nextPageToken, files(imageMediaMetadata, thumbnailLink, id, name)";
            var data = await request.ExecuteAsync();

            var files = from file in data.Files
                         where file.ImageMediaMetadata != null
                         where file.ImageMediaMetadata.Location != null
                         select file;
            return files;

        }

        private async void InitializeAsync()
        {
            var credential = await GetUserCredentialAsync();
            GoogleDriveService = GetDriverService(credential, ApplicationName);

            var files = await GetFilesAsync();

            Files = new ObservableCollection<Google.Apis.Drive.v3.Data.File>(files);
        }

        private async Task<UserCredential> GetUserCredentialAsync()
        {

            ClientSecrets secrets = null;
            FileDataStore store = null;
            using (Stream stream = new FileStream(ApiKey, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/termproject.json");

                secrets = GoogleClientSecrets.Load(stream).Secrets;
            }

            UserCredential userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, Scopes, "user", CancellationToken.None, store);

            return userCredential;
        }

        private DriveService GetDriverService(UserCredential credential, string applicationName)
        {
            var initalizer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            };
            return new DriveService(initalizer);
        }
    }
}
