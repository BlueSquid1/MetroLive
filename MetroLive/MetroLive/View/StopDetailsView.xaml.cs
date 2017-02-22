using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using System.IO;

namespace MetroLive.View
{
    public partial class StopDetailsView : ContentPage
    {
        //public static IList<string> PhoneNumbers { get; set; }

        //constructors
        public StopDetailsView(int busReference)
        {
            InitializeComponent();
            SetStopDetails(busReference);
        }

        private async void SetStopDetails(int busId)
        {
            this.txtBusID.Text = busId.ToString();

            string url = "http://realtime.adelaidemetro.com.au/SiriWebServiceSAVM/SiriStopMonitoring.svc/json/SM?MonitoringRef=" + busId.ToString() + "&PreviewInterval=60&StopMonitoringDetailLevel=normal&MaximumStopVisits=100&Item=1";

            HttpClient httpClient = new HttpClient();
            txtBusDetails.Text = await httpClient.GetStringAsync(new Uri(url));
        }
    }
}
