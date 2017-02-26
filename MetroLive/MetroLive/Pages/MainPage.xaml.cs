using MetroLive.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MetroLive.Pages
{
    public partial class MainPage : ContentPage
    {
        private MetroLiveCore metroLive;

        //constructor
        public MainPage()
        {
            InitializeComponent();
            this.Appearing += MainView_Appearing;
            metroLive = new MetroLiveCore();
        }

        private async void MainView_Appearing(object sender, EventArgs e)
        {
            await metroLive.LoadScheduleAsync();
        }

        private async void OnTripDetail(object sender, EventArgs e)
        {
            StopDetailPage stopDetails = new StopDetailPage(metroLive, txtBusID.Text);
            await this.Navigation.PushAsync(stopDetails);
        }

    }
}
