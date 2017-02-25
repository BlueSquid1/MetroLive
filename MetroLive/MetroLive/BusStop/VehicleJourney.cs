using System;

namespace MetroLive.BusStop
{
    public class VehicleJourney
    {
        //time this message was created
        public DateTime? RecordedAt { get; set; }
        //time message is valid till
        public DateTime? ValidUntil { get; set; }
        //confidence level in data (1 = high, 2 = medium, 3 = not real time I think)
        public int ConfidenceLevel { get; set; }
        public string FinalDestinationName { get; set; }
        public char Dirrection { get; set; }
        public string DriverName { get; set; }
        public string DriverRef { get; set; }
        public string LineRef { get; set; }
        public string VehicleRef { get; set; }

        //the aimed time of arrival
        public DateTime AimedArrival { get; set; }

        //the estimated time of arrival I think
        public DateTime EstimatedArrival { get; set; }


    }
}