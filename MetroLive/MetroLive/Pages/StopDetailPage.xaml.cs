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
        public string LineRef { get; set; }
        public string ExpectArrival { get; set; }
        public string ExpectedUncertainty { get; set; }
        public string TimeDiff { get; set; }
        public string TimeTillArrival { get; set; }
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
                string lineRef = vehicle?.LineRef;
                TimeSpan? realTimeUncertainty = vehicle?.LatestEstimatedArrival - vehicle?.EarliestEstimatedArrival;
                string uncertainty = "+-" + Math.Abs(realTimeUncertainty.Value.Minutes).ToString();
                TimeSpan? realTimeDiff = vehicle.AimedArrival - vehicle.EarliestEstimatedArrival;
                string timeDiff = realTimeDiff.Value.Minutes.ToString() + " mins";
                DateTime? estimateAvg = vehicle?.EarliestEstimatedArrival;
                estimateAvg.Value.AddMinutes(realTimeUncertainty.Value.Minutes / 2);
                string estimateAvgString = estimateAvg.Value.TimeOfDay.ToString();
                TimeSpan? timeTillArrival = estimateAvg - DateTime.Now;
                string timeTillArrivalStr = timeTillArrival.Value.TotalMinutes.ToString();

                busCollection.Add(new BusViewModel
                {
                    ExpectedUncertainty = uncertainty,
                    TimeDiff = timeDiff,
                    LineRef = lineRef,
                    ExpectArrival = estimateAvgString,
                    TimeTillArrival = timeTillArrivalStr
                });
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
