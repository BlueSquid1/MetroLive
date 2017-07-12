using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.SIRI.Adelaide
{
    public class StopVisit
    {
        public string RecordedAtTime;
        public string ItemIdentifier;
        public VehicleJourney MonitoredVehicleJourney;
        public ValueObj MonitoringRef;
        public List<ValueObj> StopVisitNote;
        public string ValidUntilTime;
    }

    public class VehicleJourneyRef
    {
        public ValueObj DataFrameRef;
        public string DatedVehicleJourneyRef;
    }

    public class VehicleJourney
    {
        public ValueObj BlockRef;
        public int ConfidenceLevel;
        public string DestinationAimedArrivalTime;
        public bool DestinationAimedArrivalTimeSpecified;
        public List<ValueObj> DestinationName;
        public ValueObj DirectionRef;
        public string DriverName;
        public string DriverRef;
        public VehicleJourneyRef FramedVehicleJourneyRef;
        public ValueObj JourneyPatternRef;
        public ValueObj LineRef;
        public bool Monitored;
        public MonitoredBus MonitoredCall;
        public bool MonitoredSpecified;
        public ValueObj OperatorRef;
        public string OriginAimedDepartureTime;
        public ValueObj VehicleRef;

    }

    public class MonitoredBus
    {
        public List<ValueObj> StopPointName;
        public ValueObj StopPointRef;
        public string AimedArrivalTime;
        public bool AimedArrivalTimeSpecified;
        public string AimedDepartureTime;
        public string AimedLatestPassengerAccessTime;
        public ValueObj DestinationDisplay;
        public string EarliestExpectedDepartureTime;
        public string ExpectedLatestPassengerAccessTime;
        public string Item;
        public string Item1;
        public string LatestExpectedArrivalTime;
        public bool LatestExpectedArrivalTimeSpecified;
        public string ProvisionalExpectedDepartureTime;
        public GPSLoc VehicleLocationAtStop;
    }

    public class GPSLoc
    {
        //[x, y]
        public List<string> Items;
    }

    public class ValueObj
    {
        public string Value;
    }

    public class StopMonitor
    {
        public string ResponseTimestamp;
        public bool Status;
        public string ValidUntil;
        public List<StopVisit> MonitoredStopVisit;
        public List<ValueObj> MonitoringRef;
        public string version;
    }

    public class SIRIObjAdelaide
    {
        public List<StopMonitor> StopMonitoringDelivery;
    }
}
