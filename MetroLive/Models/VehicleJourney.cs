using System;

namespace MetroLive.Models
{
    public class VehicleJourney : IComparable<VehicleJourney>
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

        private DateTime? estimatedArrival = null;
        public DateTime? EstimatedArrival //takes into account real time data
		{ 
            get
            {
                if(estimatedArrival == null)
                {
                    return AimedArrival;
                }
                return estimatedArrival;
            }

            set
            {
                estimatedArrival = value;
            }
        }


        //vehicle details
        public string DriverName { get; set; }
        public string DriverRef { get; set; }
        public string VehicleRef { get; set; }

        public int CompareTo(VehicleJourney other)
        {
			// If other is not a valid object reference, this instance is greater.
			if (other == null)
			{
				return 1;
			}

			if (this.EstimatedArrival > other.EstimatedArrival)
			{
				return 1;
			}
			else if (this.EstimatedArrival < other.EstimatedArrival)
			{
				return -1;
			}
			else
			{
				return 0;
			}
        }
    }
}