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
            
            
            this.Unloaded += new RoutedEventHandler(CustomMarkerRed_Unloaded);
            this.Loaded += new RoutedEventHandler(CustomMarkerRed_Loaded);
            this.MouseEnter += new MouseEventHandler(CustomMarkerRed_MouseEnter);
            this.MouseLeave += new MouseEventHandler(CustomMarkerRed_MouseLeave);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerRed_SizeChanged);
            

            Popup.Placement = PlacementMode.Mouse;
            {
                Label.Background = Brushes.Black;
                Label.Foreground = Brushes.White;
                Label.FontSize = 24;
                Label.Content = title;
            }
            Popup.Child = Label;
        }

        void CustomMarkerRed_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void CustomMarkerRed_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= new RoutedEventHandler(CustomMarkerRed_Unloaded);
            this.Loaded -= new RoutedEventHandler(CustomMarkerRed_Loaded);
            this.SizeChanged -= new SizeChangedEventHandler(CustomMarkerRed_SizeChanged);

            Marker.Shape = null;
            icon.Source = null;
            icon = null;
            Popup = null;
            Label = null;
        }

        void CustomMarkerRed_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void CustomMarkerRed_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 10000;
            Popup.IsOpen = true;
        }

        void CustomMarkerRed_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 10000;
            Popup.IsOpen = false;
        }

    }
}
