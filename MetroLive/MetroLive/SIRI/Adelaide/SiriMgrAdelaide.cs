using MetroLive.MetroData;
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
        public override async Task<BusStopDetails> GetStopDataAsync(string StopRef, DateTimeOffset timeRange)
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
                return null;
            }

            replyMsg = "{\"StopMonitoringDelivery\":[{\"ResponseTimestamp\":\"\\/Date(1489843600000+1030)\\/\",\"Status\":true,\"ValidUntil\":\"\\/Date(0)\\/\",\"MonitoredStopVisit\":[{\"RecordedAtTime\":\"\\/Date(1489843589000+1030)\\/\",\"ItemIdentifier\":\"8399a2cd-d257-482e-a30d-d550c4982abf\",\"MonitoredVehicleJourney\":{\"DestinationAimedArrivalTime\":\"\\/Date(0)\\/\",\"DirectionRef\":{\"Value\":\"O\"},\"DriverRef\":\"74544\",\"LineRef\":{\"Value\":\"C2\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1489843801000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1489843836000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoredSpecified\":true,\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1489745129000+1030)\\/\",\"ItemIdentifier\":\"b17ba6de-a6b0-47a0-9f69-b64c0c1b4d3f\",\"MonitoredVehicleJourney\":{\"DestinationAimedArrivalTime\":\"\\/Date(0)\\/\",\"DirectionRef\":{\"Value\":\"O\"},\"LineRef\":{\"Value\":\"N178\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1489844700000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1489844700000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoredSpecified\":true,\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1489843537000+1030)\\/\",\"ItemIdentifier\":\"7ddb9fdf-1b35-4ec5-a3ce-0e6c2238937f\",\"MonitoredVehicleJourney\":{\"DestinationAimedArrivalTime\":\"\\/Date(0)\\/\",\"DirectionRef\":{\"Value\":\"O\"},\"DriverRef\":\"74350\",\"LineRef\":{\"Value\":\"M44\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1489844700000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1489844745000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoredSpecified\":true,\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1489745114000+1030)\\/\",\"ItemIdentifier\":\"809824fb-47ad-4c9e-959d-6f04eaab5ff2\",\"MonitoredVehicleJourney\":{\"DestinationAimedArrivalTime\":\"\\/Date(0)\\/\",\"DirectionRef\":{\"Value\":\"O\"},\"LineRef\":{\"Value\":\"N1\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1489846260000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1489846260000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoredSpecified\":true,\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"ValidUntilTime\":\"\\/Date(0)\\/\"}],\"MonitoringRef\":[{\"Value\":\"13277\"}],\"version\":\"2.0\"}]}";
            SIRIObjAdelaide siriAdel = JsonConvert.DeserializeObject<SIRIObjAdelaide>(replyMsg);


            BusStopDetails busStop = new BusStopDetails();
            //convert to started format
            List<StopVisit> BussesTracked = siriAdel.StopMonitoringDelivery[0].MonitoredStopVisit;

            foreach( StopVisit bus in BussesTracked)
            {
                MetroData.VehicleJourney incomingVehicle = new MetroData.VehicleJourney();
                incomingVehicle.LineRef = bus.MonitoredVehicleJourney.LineRef.Value;
                busStop.IncomingVehicles.Add(incomingVehicle);
            }

            return busStop;
        }
    }
}
