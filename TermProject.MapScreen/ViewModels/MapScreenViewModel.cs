using GMap.NET;
using GMap.NET.WindowsPresentation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TermProject.Infra;
using TermProject.MapScreen.Views;
using Google.Apis.Drive.v3.Data;
using TermProject.MapScreen.CustomMarkers;
using GMap.NET.MapProviders;

namespace TermProject.MapScreen.ViewModels
{
    /// <summary>
    /// MapScreenView의 ViewModel이다.
    /// 해당 ViewModel은 연결되어 있는 View에 출력을 해야 되지만
    /// GMap UI에 자식 Element로 UI를 추가하기 위해 어쩔수 없이 MVVM 모델을 위배시켜서 구현하였다.
    /// </summary>
    public class MapScreenViewModel : BindableBase
    {

        private GMap.NET.WindowsPresentation.GMapControl MainMap;
        private MapScreenView MapScreenView;
        private GMapMarker it;

        public IEventAggregator EA { get; set; }

        /// <summary>
        /// ImageScreen 모듈 혹은 View에서 표시 되는 UI를 ViewModel에서 사용하기 위해 ea라는 인자를 받아 상속 한다.
        /// 해당 생성자는 Prism 라이브러이에 있는 EventAggregator Container를 통해 Constructor 스타일의 Dependency Injection을 한다.
        /// Property인 EA는 해당 클래스 이벤트에서 Publish한 인자 값들을 Subscribe를 통해 다음 method에서 처리한다.
        /// </summary>
        /// <param name="ea">EventAggregator와 의존 관계인 객체</param>
        public MapScreenViewModel(IEventAggregator ea)
        {
            EA = ea;
            EA.GetEvent<CreateMapEvent>().Subscribe(initMap);
            EA.GetEvent<CreateUserControlEvent>().Subscribe(initUserControl);

            EA.GetEvent<CreateMakerEvent>().Subscribe(SearchMap);
        }


        /// <summary>
        /// ImageScreen 모듈에서 출력되는 사진이 어디서 촬영 된는지 확인해준다.
        /// 해당 사진에 GPS정보를 GMapMarker에 저장하여 해당 좌표에 표시 해준다.
        /// </summary>
        /// <param name="file">현재 화면에 출력된 이미지 파일</param>
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

    }
}
