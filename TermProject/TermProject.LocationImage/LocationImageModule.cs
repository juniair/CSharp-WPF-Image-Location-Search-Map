using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermProject.LocationImage.Views;

namespace TermProject.LocationImage
{
    
    public class LocationImageModule : IModule
    {
        IRegionManager _regionManager;

        public LocationImageModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            this._regionManager.RegisterViewWithRegion("LocationImage", typeof(LocationImageView));
        }
    }
}
