using System;

namespace MetroLive.MetroData
{
    public class VehicleJourney
    {
        //metadata
        public DateTime? RecordedAt { get; set; } //time this message was created
        public DateTime? ValidUntil { get; set; } //time message is valid till

        //journey details
        public int ConfidenceLevel { get; set; } //confidence level in data (1 = high, 2 = medium, 3 = not real time I think)
        public string FinalDestinationName { get; set; }
        public char Dirrection { get; set; }
        public string LineRef { get; set; }
        public DateTime AimedArrival { get; set; } //the aimed time of arrival
        public DateTime EstimatedArrival { get; set; } //the estimated time of arrival I think


        //vehicle details
        public string DriverName { get; set; }
        public string DriverRef { get; set; }
        public string VehicleRef { get; set; }

    }
}