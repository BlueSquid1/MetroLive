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
        //for identification purposes only
        public string BusId { get; set; }

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
        private BusStopDetails stopDetails;

        //constructors
        public StopDetailPage(MetroLiveCore mMetroLive, string busReference)
        {
            InitializeComponent();
            this.metroLive = mMetroLive;
            this.Appearing += StopDetailsView_Appearing;

            stopMgr = mMetroLive.GetBusStopDetails(busReference);

            this.Title = "Stop ID: " + busReference;
        }

        //triggered when page is about to be displayed
        private async void StopDetailsView_Appearing(object sender, EventArgs e)
        {
            //get realtime info
            this.stopDetails = await stopMgr.GetRealTimeDataAsync(new DateTimeOffset(DateTime.Now.Ticks, TimeSpan.FromMinutes(60)));
            UpdateDisplay(this.stopDetails);
        }

        private void Fav_Clicked(object sender, EventArgs e)
        {
            
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
                string timeTillArrivalStr = ((int)timeTillArrival.Value.TotalMinutes).ToString();

                busCollection.Add(new BusViewModel
                {
                    BusId = vehicle?.VehicleRef,
                    ExpectedUncertainty = uncertainty,
                    TimeDiff = timeDiff,
                    LineRef = lineRef,
                    ExpectArrival = estimateAvgString,
                    TimeTillArrival = timeTillArrivalStr
                });
            }
            listView.ItemsSource = busCollection;
        }

        public async void OnSelectedItem(object sender, SelectedItemChangedEventArgs  e)
        {
            BusViewModel busViewModel = (BusViewModel)e.SelectedItem;

            VehicleJourney vehicle = FindJourneyByBusRef(busViewModel.BusId);

            VehicleDetailPage stopDetails = new VehicleDetailPage(metroLive, vehicle);
            await this.Navigation.PushAsync(stopDetails);
                        
        }

        public VehicleJourney FindJourneyByBusRef(string busRef)
        {
            //for bus ref number get the correct VehicleJourney
            foreach (VehicleJourney vehicle in stopDetails.IncomingVehicles)
            {
                if (vehicle.VehicleRef == busRef)
                {
                    return vehicle;
                }
            }
            return null;

        }
    }
}
