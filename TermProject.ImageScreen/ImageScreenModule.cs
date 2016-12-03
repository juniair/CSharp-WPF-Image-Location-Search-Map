using Prism.Modularity;
using Prism.Regions;
using System;
using TermProject.ImageScreen.Views;

namespace TermProject.ImageScreen
{
    public class ImageScreenModule : IModule
    {
        IRegionManager _regionManager;

        public ImageScreenModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            this._regionManager.RegisterViewWithRegion("ImageScreen", typeof(ImageScreenView));
        }
    }
}