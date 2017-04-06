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

namespace MetroLive.Pages.StopDetails
{
    public partial class StopDetailPage : ContentPage
    {
        private StopDetailsModel stopDetailsModel;
        private MetroLiveCore metroLive;
        private BusStopMgr stopMgr;

        //constructors
        public StopDetailPage(MetroLiveCore mMetroLive, string busReference)
        {
            InitializeComponent();

            this.stopDetailsModel = new StopDetailsModel();

            this.Appearing += StopDetailsView_Appearing;

            this.metroLive = mMetroLive;

            this.BindingContext = stopDetailsModel;

            stopMgr = mMetroLive.GetBusStopDetails(busReference);

            this.Title = "Stop ID: " + busReference;
        }

        //triggered when page is about to be displayed
        private async void StopDetailsView_Appearing(object sender, EventArgs e)
        {
            //get realtime info
            BusStopDetails stopDetails = await stopMgr.GetRealTimeDataAsync(new DateTimeOffset(DateTime.Now.Ticks, TimeSpan.FromMinutes(60)));
            UpdateDisplay(stopDetails);
        }

        private void Fav_Clicked(object sender, EventArgs e)
        {
            //toggle adding bus to favourites
            metroLive.AddBusToFavourites(stopMgr.BusStopId, "");
        }

        
        private void UpdateDisplay(BusStopDetails stopDetails)
        {
            List<BusViewModel> busCollection = new List<BusViewModel>();

            //populate the collection
            foreach (VehicleJourney vehicle in stopDetails.IncomingVehicles)
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
            stopDetailsModel.BusCollection = busCollection;
            //listView.ItemsSource = busCollection;

            /*
            if (busCollection.Count > 0)
            {
                listView.ScrollTo(busCollection.LastOrDefault(), ScrollToPosition.Start, false);
            }
            */
        }

        public async void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            BusViewModel busViewModel = (BusViewModel)e.SelectedItem;

            VehicleJourney vehicle = FindJourneyByBusRef(busViewModel.BusId);

            VehicleDetailPage stopDetails = new VehicleDetailPage(metroLive, vehicle);
            await this.Navigation.PushAsync(stopDetails);

        }

        private VehicleJourney FindJourneyByBusRef(string busRef)
        {
            //for bus ref number get the correct VehicleJourney
            foreach (VehicleJourney vehicle in stopMgr.BusStopData.IncomingVehicles)
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
