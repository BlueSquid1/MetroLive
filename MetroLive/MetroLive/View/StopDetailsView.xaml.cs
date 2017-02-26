using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using System.IO;
using MetroLive.Common;
using MetroLive.BusStop;

namespace MetroLive.View
{
    public partial class StopDetailsView : ContentPage
    {
        private MetroLiveCore metroLive;
        private BusStopDetails busStop;

        //constructors
        public StopDetailsView(MetroLiveCore mMetroLive, string busReference)
        {
            InitializeComponent();
            this.metroLive = mMetroLive;

            busStop = metroLive.GetBusStopDetails(busReference);
            busStop.NewInfo += BusStop_NewInfo;
            this.Appearing += StopDetailsView_Appearing;
        }

        private void BusStop_NewInfo(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        //triggered when page is about to be displayed
        private async void StopDetailsView_Appearing(object sender, EventArgs e)
        {
            await busStop.StartListeningAsyc();
            await busStop.FetchscheduledDataAsync( new DateTimeOffset( DateTime.Now, TimeSpan.FromMinutes(120)));
            UpdateDisplay();
            await busStop.FetchLiveDataAsync(new DateTimeOffset(DateTime.Now + metroLive.Settings.SIRIStart, metroLive.Settings.SIRIPreviewInterval));
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {

        }

        /*
        private async void SetStopDetails(int busId)
        {
            this.txtBusID.Text = busId.ToString();

            //preview interval = 60 (how far into the future to read)
            //
            string url = "http://realtime.adelaidemetro.com.au/SiriWebServiceSAVM/SiriStopMonitoring.svc/json/SM?MonitoringRef=" + busId.ToString() + "&PreviewInterval=60&StopMonitoringDetailLevel=minimum&MaximumStopVisits=100&Item=1";

            HttpClient httpClient = new HttpClient();
            try
            {
                txtBusDetails.Text = await httpClient.GetStringAsync(new Uri(url));
            }
            catch
            {
                //WriteLine("");
            }
        }
        */
    }
}
