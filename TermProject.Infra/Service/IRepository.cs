using System.Collections.ObjectModel;
using Google.Apis.Drive.v3.Data;

namespace TermProject.Infra.Service
{
    public interface IRepository
    {
        int FileCount { get; }

        ObservableCollection<File> Files { get; }

        File GetFile(int index);
    }
}