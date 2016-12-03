using Prism.Modularity;
using Prism.Regions;
using System;
using TermProject.MapScreen.Views;

namespace TermProject.MapScreen
{
    public class MapScreenModule : IModule
    {
        IRegionManager _regionManager;

        public MapScreenModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            this._regionManager.RegisterViewWithRegion("MapScreen", typeof(MapScreenView));
        }
    }
}