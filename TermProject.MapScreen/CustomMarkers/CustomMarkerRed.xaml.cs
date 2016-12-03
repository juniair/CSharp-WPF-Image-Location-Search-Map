using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using TermProject.MapScreen.Views;

namespace TermProject.MapScreen.CustomMarkers
{
    /// <summary>
    /// CustomMarkerRed.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomMarkerRed
    {
        Popup Popup;
        Label Label;
        GMapMarker Marker;
        MapScreenView MapScreenView;

        public CustomMarkerRed(MapScreenView view, GMapMarker marker, string title)
        {
            this.InitializeComponent();

            this.MapScreenView = view;
            this.Marker = marker;

            Popup = new Popup();
            Label = new Label();

            this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            

            Popup.Placement = PlacementMode.Mouse;
            {
                Label.Background = Brushes.Blue;
                Label.Foreground = Brushes.White;
                Label.BorderBrush = Brushes.WhiteSmoke;
                Label.BorderThickness = new Thickness(2);
                Label.Padding = new Thickness(5);
                Label.FontSize = 22;
                Label.Content = title;
            }
            Popup.Child = Label;
        }

        void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void CustomMarkerDemo_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            this.Loaded -= new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged -= new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);

            Marker.Shape = null;
            icon.Source = null;
            icon = null;
            Popup = null;
            Label = null;
        }

        void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

    }
}
