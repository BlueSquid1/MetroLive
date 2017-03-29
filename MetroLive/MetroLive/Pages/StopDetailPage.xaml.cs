using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using System.IO;
using MetroLive.Common;
using MetroLive.MetroData;
using System.Collections.ObjectModel;

namespace MetroLive.Pages
{
    public class BusViewModel
    {
        public string BusName { get; set; }
        public string ExpectArrival { get; set; }
        public string EarlyDiff { get; set; }
    }

    public partial class StopDetailPage : ContentPage
    {
        private MetroLiveCore metroLive;
        private BusStopMgr stopMgr;

        //constructors
        public StopDetailPage(MetroLiveCore mMetroLive, string busReference)
        {
            InitializeComponent();
            this.metroLive = mMetroLive;

            this.Appearing += StopDetailsView_Appearing;

            stopMgr = mMetroLive.GetBusStopDetails(busReference);
        }

        //triggered when page is about to be displayed
        private async void StopDetailsView_Appearing(object sender, EventArgs e)
        {
            /*
            await stopMgr.FetchscheduledDataAsync(new DateTimeOffset(DateTime.Now, TimeSpan.FromMinutes(120)));
            UpdateDisplay();
            await stopMgr.FetchLRealTimeDataAsync(new DateTimeOffset(DateTime.Now + metroLive.Settings.SIRIStart, metroLive.Settings.SIRIPreviewInterval));
            UpdateDisplay();
            */

            //get realtime info
            BusStopDetails stopDetails = await stopMgr.GetRealTimeDataAsync(new DateTimeOffset(DateTime.Now.Ticks, TimeSpan.FromMinutes(60)));
            UpdateDisplay(stopDetails);
        }

        private void UpdateDisplay(BusStopDetails stopDetails)
        {
            ObservableCollection<BusViewModel> busCollection = new ObservableCollection<BusViewModel>();

            //populate the collection
            foreach ( VehicleJourney vehicle in stopDetails.IncomingVehicles )
            {
                TimeSpan? earilyDiff = vehicle.AimedArrival - vehicle.EarliestEstimatedArrival;
                busCollection.Add(new BusViewModel { BusName = vehicle.LineRef, ExpectArrival = vehicle.EarliestEstimatedArrival.ToString(), EarlyDiff = earilyDiff.ToString() });
            }
            listView.ItemsSource = busCollection;
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /*
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            DisplayAlert("Tapped", e.SelectedItem + " row was tapped", "OK");
            ((ListView)sender).SelectedItem = null; // de-select the row
            */
        }
    }
}
