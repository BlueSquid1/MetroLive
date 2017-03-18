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

namespace MetroLive.Pages
{
    public partial class StopDetailPage : ContentPage
    {
        private MetroLiveCore metroLive;
        private BusStopMgr stopMgr;

        //constructors
        public StopDetailPage(MetroLiveCore mMetroLive, string busReference)
        {
            InitializeComponent();
            this.metroLive = mMetroLive;

            stopMgr = metroLive.GetBusStopDetails(busReference);
            this.Appearing += StopDetailsView_Appearing;
        }

        private void BusStop_NewInfo(object sender, EventArgs e)
        {
            UpdateDisplay();
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
        }

        private void UpdateDisplay()
        {

        }
    }
}
