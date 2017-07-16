using System;

namespace MetroLive.Models
{
    public class VehicleJourney
    {
        public int TripId { get; set; }

        //metadata
        public DateTime? RecordedAt { get; set; } //time this message was created
        public DateTime? ValidUntil { get; set; } //time message is valid till

        //journey details
        public int? ConfidenceLevel { get; set; } //confidence level in data (1 = high, 2 = medium, 3 = not real time I think)
        public string FinalDestinationName { get; set; }
        public bool? DirrectionAway { get; set; } //dirrection is going away from the city
        public string LineRef { get; set; }
        public DateTime? AimedArrival { get; set; } //does not take into account real time data
        public DateTime? EarliestEstimatedArrival { get; set; } //takes into account real time data
        public DateTime? LatestEstimatedArrival { get; set; } //takes into account real time data


        //vehicle details
        public string DriverName { get; set; }
        public string DriverRef { get; set; }
        public string VehicleRef { get; set; }

    }
}