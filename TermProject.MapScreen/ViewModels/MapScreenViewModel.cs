using GMap.NET;
using GMap.NET.WindowsPresentation;
using Prism.Events;
using Prism.Mvvm;
using TermProject.Infra;
using TermProject.MapScreen.Views;
using Google.Apis.Drive.v3.Data;
using TermProject.MapScreen.CustomMarkers;
using GMap.NET.MapProviders;
using TermProject.Infra.Service;

namespace TermProject.MapScreen.ViewModels
{

    public class MapScreenViewModel : BindableBase
    {

        private IRepository Repository { get; set; }

        private GMap.NET.WindowsPresentation.GMapControl MainMap;
        private MapScreenView MapScreenView;
        private GMapMarker it;

        public IEventAggregator EA { get; set; }

        public MapScreenViewModel(IEventAggregator ea, IRepository repository)
        {
            Repository = repository;
            EA = ea;
            EA.GetEvent<CreateMapEvent>().Subscribe(initMap);
            EA.GetEvent<CreateUserControlEvent>().Subscribe(initUserControl);

            EA.GetEvent<CreateMakerEvent>().Subscribe(SearchMap);
        }

        private void initUserControl(object view)
        {
            MapScreenView = view as MapScreenView;
        }

        private void initMap(object map)
        {
            MainMap = map as GMap.NET.WindowsPresentation.GMapControl;
            MainMap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            MainMap.Zoom = 15;
        }

        private void SearchMap(File file)
        {
            GeoCoderStatusCode status;
            PointLatLng city = new PointLatLng(file.ImageMediaMetadata.Location.Latitude.Value, file.ImageMediaMetadata.Location.Longitude.Value);

            Placemark place = (Placemark) (GMapProviders.GoogleMap.GetPlacemark(city, out status));
            if (it != null)
            {
                MainMap.Markers.Remove(it);
            }

            if(status == GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                it = new GMapMarker(city);
                {
                    it.ZIndex = 55;
                    it.Shape = new CustomMarkerRed(MapScreenView, it, place.Address);
                }
            }

            MainMap.Markers.Add(it);
            MainMap.Position = city;
        }



    }
}
