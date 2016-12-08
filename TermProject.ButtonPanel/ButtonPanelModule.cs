using Prism.Modularity;
using Prism.Regions;
using System;

namespace TermProject.ButtonPanel
{
    public class ButtonPanelModule : IModule
    {
        IRegionManager _regionManager;

        public ButtonPanelModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}