using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.BusStop;

namespace MetroLive.Common
{
    class MetroLiveCoreSA : MetroLiveCore
    {

        public MetroLiveCoreSA()
        {
            base.GTFSBaseUrl = "http://adelaidemetro.com.au/GTFS/google_transit.zip";
            base.SIRIBaseUrl = "http://realtime.adelaidemetro.com.au/SiriWebServiceSAVM/SiriStopMonitoring.svc/json/SM";
        }

        public override async Task<BusStopDetails> GetBusStopDetailsAsync(string busId)
        {
            BusStopDetailsSA busStop = new BusStopDetailsSA();
            await busStop.FetchDataAsync();
            return busStop;
        }
    }
}
