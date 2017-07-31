using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MetroLive.Models;
using MetroLive.Util;
using ServiceStack.Text;

namespace MetroLive.Services.Realtime.SIRI
{
    public class SiriMgrAdelaide : IRealtime
    {
        public async Task<BusStopDetails> GetStopDataAsync(string StopRef, TimeSpan timeLength, bool forceRefresh = false)
        {
            //get the response from the server
            string url = "http://realtime.adelaidemetro.com.au/SiriWebServiceSAVM/SiriStopMonitoring.svc/json/SM?MonitoringRef=" + StopRef.ToString() + "&PreviewInterval=" + timeLength.TotalMinutes.ToString() + "&StopMonitoringDetailLevel=normal&MaximumStopVisits=100&Item=1";
            string replyMsg = null;
            HttpClient httpClient = new HttpClient();
            replyMsg = await httpClient.GetStringAsync(new Uri(url));

            //for debugging purposes
            //replyMsg = "{\"StopMonitoringDelivery\":[{\"ResponseTimestamp\":\"\\/Date(1490271805000+1030)\\/\",\"Status\":true,\"ValidUntil\":\"\\/Date(0)\\/\",\"MonitoredStopVisit\":[{\"RecordedAtTime\":\"\\/Date(1490271700000+1030)\\/\",\"ItemIdentifier\":\"28c102b4-f252-4ef2-84f6-add8ecf568b5\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"5212\"},\"ConfidenceLevel\":1,\"DestinationAimedArrivalTime\":\"\\/Date(1490274660000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Greenwith\"}],\"DirectionRef\":{\"Value\":\"O\"},\"DriverName\":\"Confidential\",\"DriverRef\":\"74440\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"220330\"},\"JourneyPatternRef\":{\"Value\":\"14\"},\"LineRef\":{\"Value\":\"C2\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490272200000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Greenwith\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490272131000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"7 - Adelaide Metro (Light City Buses)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleRef\":{\"Value\":\"1110\"}},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1490271629000+1030)\\/\",\"ItemIdentifier\":\"dbae076a-5c98-428d-a75d-c7df8fc6fd5d\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"338\"},\"ConfidenceLevel\":1,\"DestinationAimedArrivalTime\":\"\\/Date(1490272860000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Marden\"}],\"DirectionRef\":{\"Value\":\"I\"},\"DriverName\":\"Confidential\",\"DriverRef\":\"52454\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"231586\"},\"JourneyPatternRef\":{\"Value\":\"4 \"},\"LineRef\":{\"Value\":\"W90M\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490272200000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Marden\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490272198000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"5 - Adelaide Metro (Torrens Transit)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleRef\":{\"Value\":\"1935\"}},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1490271765000+1030)\\/\",\"ItemIdentifier\":\"e029a004-633a-499a-bec8-6af516e465dd\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"3191\"},\"ConfidenceLevel\":1,\"DestinationAimedArrivalTime\":\"\\/Date(1490273400000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Paradise\"}],\"DirectionRef\":{\"Value\":\"O\"},\"DriverName\":\"Confidential\",\"DriverRef\":\"52298\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"300811\"},\"JourneyPatternRef\":{\"Value\":\"25\"},\"LineRef\":{\"Value\":\"174\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490272140000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Paradise\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490272219000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"5 - Adelaide Metro (Torrens Transit)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleRef\":{\"Value\":\"1923\"}},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1490271755000+1030)\\/\",\"ItemIdentifier\":\"47e8abb2-f5cb-411d-bb18-9e55cebbd359\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"5267\"},\"ConfidenceLevel\":2,\"DestinationAimedArrivalTime\":\"\\/Date(1490275020000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Golden Grove\"}],\"DirectionRef\":{\"Value\":\"O\"},\"DriverName\":\"Confidential\",\"DriverRef\":\"73125\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"296707\"},\"JourneyPatternRef\":{\"Value\":\"77\"},\"LineRef\":{\"Value\":\"M44\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490273100000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Golden Grove\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490272889000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"7 - Adelaide Metro (Light City Buses)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleRef\":{\"Value\":\"1138\"}},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1490177622000+1030)\\/\",\"ItemIdentifier\":\"329ad14b-a74f-4ef6-a7cc-8620562ed211\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"3200\"},\"ConfidenceLevel\":4,\"DestinationAimedArrivalTime\":\"\\/Date(1490276400000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Paradise\"}],\"DirectionRef\":{\"Value\":\"O\"},\"DriverName\":\"Confidential\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"300928\"},\"JourneyPatternRef\":{\"Value\":\"38\"},\"LineRef\":{\"Value\":\"178\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490273940000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Paradise\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490273940000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"5 - Adelaide Metro (Torrens Transit)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1490177705000+1030)\\/\",\"ItemIdentifier\":\"621218eb-7f76-453f-a9b6-fdde3d1e3a02\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"325\"},\"ConfidenceLevel\":4,\"DestinationAimedArrivalTime\":\"\\/Date(1490275200000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Paradise\"}],\"DirectionRef\":{\"Value\":\"O\"},\"DriverName\":\"Confidential\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"300817\"},\"JourneyPatternRef\":{\"Value\":\"24\"},\"LineRef\":{\"Value\":\"174\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490273940000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Paradise\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490273940000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"5 - Adelaide Metro (Torrens Transit)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1490177676000+1030)\\/\",\"ItemIdentifier\":\"ab4a2ec3-cc2a-4fe5-81d5-bcae9bbb756c\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"526\"},\"ConfidenceLevel\":4,\"DestinationAimedArrivalTime\":\"\\/Date(1490275620000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Golden Grove Village\"}],\"DirectionRef\":{\"Value\":\"O\"},\"DriverName\":\"Confidential\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"220264\"},\"JourneyPatternRef\":{\"Value\":\"2 \"},\"LineRef\":{\"Value\":\"C1G\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490274000000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Golden Grove Village\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490274000000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"7 - Adelaide Metro (Light City Buses)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"},{\"RecordedAtTime\":\"\\/Date(1490177674000+1030)\\/\",\"ItemIdentifier\":\"eff4c54f-1daa-40e9-9acf-122c6bcce501\",\"MonitoredVehicleJourney\":{\"BlockRef\":{\"Value\":\"5249\"},\"ConfidenceLevel\":4,\"DestinationAimedArrivalTime\":\"\\/Date(1490275860000+1030)\\/\",\"DestinationAimedArrivalTimeSpecified\":true,\"DestinationName\":[{\"Value\":\"Tea Tree Plaza\"}],\"DirectionRef\":{\"Value\":\"O\"},\"DriverName\":\"Confidential\",\"FramedVehicleJourneyRef\":{\"DataFrameRef\":{\"Value\":\"2017-03-23\"},\"DatedVehicleJourneyRef\":\"220277\"},\"JourneyPatternRef\":{\"Value\":\"1 \"},\"LineRef\":{\"Value\":\"C1T\"},\"Monitored\":true,\"MonitoredCall\":{\"StopPointName\":[{\"Value\":\"I2 North Tce\"}],\"StopPointRef\":{\"Value\":\"13277\"},\"AimedArrivalTime\":\"\\/Date(1490274900000+1030)\\/\",\"AimedArrivalTimeSpecified\":true,\"AimedDepartureTime\":\"\\/Date(0)\\/\",\"AimedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"DestinationDisplay\":{\"Value\":\"Tea Tree Plaza\"},\"EarliestExpectedDepartureTime\":\"\\/Date(0)\\/\",\"ExpectedLatestPassengerAccessTime\":\"\\/Date(0)\\/\",\"Item\":\"\\/Date(0)\\/\",\"Item1\":\"\\/Date(0)\\/\",\"LatestExpectedArrivalTime\":\"\\/Date(1490274900000+1030)\\/\",\"LatestExpectedArrivalTimeSpecified\":true,\"ProvisionalExpectedDepartureTime\":\"\\/Date(0)\\/\",\"VehicleLocationAtStop\":{\"Items\":[\"-34.92091044\",\"138.60981017\"]}},\"MonitoredSpecified\":true,\"OperatorRef\":{\"Value\":\"7 - Adelaide Metro (Light City Buses)\"},\"OriginAimedDepartureTime\":\"\\/Date(0)\\/\"},\"MonitoringRef\":{\"Value\":\"13277\"},\"StopVisitNote\":[{\"Value\":\"StopType=BS\"}],\"ValidUntilTime\":\"\\/Date(0)\\/\"}],\"MonitoringRef\":[{\"Value\":\"13277\"}],\"version\":\"2.0\"}]}";
            JsonObject siriAdel = JsonObject.Parse(replyMsg);
            BusStopDetails stopDetails = ConvertJsonToBusStop(siriAdel);
            return stopDetails;
		}

        private BusStopDetails ConvertJsonToBusStop(JsonObject jsonObj)
        {
            BusStopDetails busStop = new BusStopDetails();
            JsonObject stopMon = jsonObj.GetArray<JsonObject>("StopMonitoringDelivery")?.FirstOrDefault();


            //get bus stop details
			busStop.StopRef = stopMon.GetArray<JsonObject>("MonitoringRef")?.FirstOrDefault()?.Get("Value");
			busStop.RspTimestamp = stopMon.Get<DateTime>("ResponseTimestamp");
			
			//other bus stop details are stored in the vehicle journey
			if (stopMon.Get("MonitoredStopVisit") == null)
			{
				return busStop;
			}
            JsonObject monitoredCall = stopMon.GetByBPath<JsonObject>("MonitoredStopVisit[0]/MonitoredVehicleJourney/MonitoredCall");
            busStop.StopPointName = monitoredCall.GetByBPath<string>("StopPointName[0]/Value");


            string xCordStr = monitoredCall?.GetByBPath<string>("VehicleLocationAtStop/Items[0]");
			string yCordStr = monitoredCall?.GetByBPath<string>("VehicleLocationAtStop/Items[1]");

			if (xCordStr != null && yCordStr != null)
			{
				busStop.busStopX = float.Parse(xCordStr);
				busStop.busStopY = float.Parse(yCordStr);
			}
            busStop.Version = stopMon.Get("version");

			//get busses that are comming
            JsonObject[] BussesTracked = stopMon.GetArray<JsonObject>("MonitoredStopVisit");
			foreach (JsonObject bus in BussesTracked)
			{
                //fill in a incomingVehicle object
				VehicleJourney incomingVehicle = new VehicleJourney();

                //make variables for frequently used objects
                JsonObject stopVisit = bus.Get<JsonObject>("MonitoredVehicleJourney");
                JsonObject busMonitoredCall = stopVisit.Get<JsonObject>("MonitoredCall");

                incomingVehicle.RecordedAt = GetLocalDateTime(bus.Get("RecordedAtTime"));
                incomingVehicle.ValidUntil = GetLocalDateTime(bus.Get("ValidUntilTime"));
                incomingVehicle.ConfidenceLevel = stopVisit.Get<int>("ConfidenceLevel");
				
                incomingVehicle.FinalDestinationName = busMonitoredCall.GetByBPath<string>("StopPointName[0]/Value");
                incomingVehicle.DirrectionAway = stopVisit.GetByBPath<string>("DirectionRef/Value") == "O" ? true : false;
                incomingVehicle.LineRef = stopVisit.GetByBPath<string>("LineRef/Value");
                incomingVehicle.AimedArrival = GetLocalDateTime(busMonitoredCall.Get("AimedArrivalTime"));
                incomingVehicle.EstimatedArrival = GetLocalDateTime(busMonitoredCall.Get("LatestExpectedArrivalTime"));
                incomingVehicle.DriverName = stopVisit.Get("DriverName");
                incomingVehicle.DriverRef = stopVisit.Get("DriverRef");
                incomingVehicle.VehicleRef = stopVisit?.GetByBPath<string>("VehicleRef/Value");
			    busStop.IncomingVehicles.Add(incomingVehicle);
			}

			return busStop;
		}

        private DateTime? GetLocalDateTime(string rawDateTime)
        {
            if (rawDateTime == null)
            {
                return null;
            }
            //remove the stupid Date label
            //     "/Date(1490274235000+1030)/"
            Match timeMatch = Regex.Match(rawDateTime, "\\b[0-9]+\\+[0-9]+");

            if(timeMatch.Success == false)
            {
                return null;
            }

            string unixTimeStampString = timeMatch.ToString();
            string[] unixTimeExtracts = unixTimeStampString.Split('+');

            long linuxTicks = long.Parse(unixTimeExtracts[0]);

            if (linuxTicks == 0)
            {
                return null;
            }

            //convert from linux time to C# datetime
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcTime = dtDateTime.AddMilliseconds(linuxTicks);

            //add the UTC offset
            double hours = double.Parse(unixTimeExtracts[1].Substring(0,2));
            double minutes = double.Parse(unixTimeExtracts[1].Substring(2, 2));
            DateTime returnDate = utcTime.AddHours(hours).AddMinutes(minutes);
            return returnDate;
        }
    }
}