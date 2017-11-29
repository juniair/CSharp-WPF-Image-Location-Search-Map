using Microsoft.Practices.Unity;
using Prism.Unity;
using TermProject.Views;
using System.Windows;
using Prism.Modularity;
using TermProject.Infra.Service;

namespace TermProject
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.RegisterTypeIfMissing(typeof(IRepository), typeof(GoogleDriveRepository), true);
        }
    }
}
