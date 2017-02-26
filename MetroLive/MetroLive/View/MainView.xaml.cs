using MetroLive.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MetroLive.View
{
    public partial class MainView : ContentPage
    {
        private MetroLiveCore metroLive;

        //constructor
        public MainView()
        {
            InitializeComponent();
            metroLive = new MetroLiveCoreSA();
        }

        private async void OnTripDetail(object sender, EventArgs e)
        {
            StopDetailsView stopDetails = new StopDetailsView(metroLive, txtBusID.Text);
            await this.Navigation.PushAsync(stopDetails);
        }
    }
}
