using Prism.Mvvm;

namespace TermProject.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "다찾아";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
