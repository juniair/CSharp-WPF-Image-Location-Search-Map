using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using TermProject.Infra;

namespace TermProject.MapScreen.Views
{
    /// <summary>
    /// Interaction logic for MapScreenView
    /// </summary>
    public partial class MapScreenView : UserControl
    {
        public IEventAggregator EA { get; set; }
        public MapScreenView(IEventAggregator ea)
        {
            EA = ea;
            InitializeComponent();
            EA.GetEvent<CreateMapEvent>().Publish(this.FindName("MainMap"));
            EA.GetEvent<CreateUserControlEvent>().Publish(this);
        }

    }
}
