using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.SIRI.Adelaide
{
    public class SiriMgrAdelaide : SiriManager
    {
        public override async Task GetStopDataAsync(DateTimeOffset timeRange, string StopRef)
        {
            //preview interval = 60 (how far into the future to read)
            //
            string url = "http://realtime.adelaidemetro.com.au/SiriWebServiceSAVM/SiriStopMonitoring.svc/json/SM?MonitoringRef=" + StopRef.ToString() + "&PreviewInterval=60&StopMonitoringDetailLevel=minimum&MaximumStopVisits=100&Item=1";

            string replyMsg = null;

            HttpClient httpClient = new HttpClient();
            try
            {
                replyMsg = await httpClient.GetStringAsync(new Uri(url));
            }
            catch
            {
                //failed
                return;
            }
            SIRIObjAdelaide x = JsonConvert.DeserializeObject<SIRIObjAdelaide>(replyMsg);

        }
    }
}
