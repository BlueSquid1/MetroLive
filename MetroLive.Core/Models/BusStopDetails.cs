using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms.Internals;

namespace MetroLive.Models
{
    public class BusStopDetails
    {
        public int StopId { get; set; }

        //bus stop reference number
        public string StopRef { get; set; }

        //response timestamp
        public DateTime? RspTimestamp { get; set; }

        //valid until timestamp
        //public DateTime? ValidUntil { get; set; }

        //e.g. 38 Lower Athelstone Road
        public string StopPointName { get; set; }


        public List<VehicleJourney> IncomingVehicles { get; set; }


        //bus stop coordinates
        public float busStopX { get; set; }
        public float busStopY { get; set; }


        public string Version { get; set; }

        //constructor
        public BusStopDetails()
        {
            IncomingVehicles = new List<VehicleJourney>();
        }


        public void MergeStopDetails( BusStopDetails otherStop)
        {
            //make sure it for the same bus stop
            if (otherStop.StopRef != this.StopRef )
            {
                throw new ArgumentException("current bus stop ref:" + this.StopId + " does not match the other stop ref: " + otherStop);
            }

            bool defaultToThisValues = true;
            if (otherStop.RspTimestamp != null && this.RspTimestamp != null)
            {
                if (otherStop.RspTimestamp < this.RspTimestamp)
                {
                    defaultToThisValues = false;
                }
            }

            //only interested in the new fields that can be added from otherStop
            IEnumerable<PropertyInfo> BusStopProps = otherStop.GetType().GetProperties();
            foreach( PropertyInfo prop in BusStopProps )
            {
                object curProp = prop.GetValue(this);
                object otherProp = prop.GetValue(otherStop);


                if (ShouldUpdate(curProp, otherProp, defaultToThisValues))
                {
                    Console.WriteLine("got here");
                    prop.SetValue(this, otherProp);
                }
            }


            //this.RspTimestamp = SelectLatestValue(latestStop.RspTimestamp, oldestStop.RspTimestamp);


            if(this.RspTimestamp != otherStop.RspTimestamp)
            {
                this.RspTimestamp = otherStop.RspTimestamp;
            }


            /*
			public int StopId { get; set; }

		//bus stop reference number
		public string StopRef { get; set; }

		//response timestamp
		public DateTime? RspTimestamp { get; set; }

		//valid until timestamp
		//public DateTime? ValidUntil { get; set; }

		//e.g. 38 Lower Athelstone Road
		public string StopPointName { get; set; }


		public List<VehicleJourney> IncomingVehicles { get; set; }


		//bus stop coordinates
		public float busStopX { get; set; }
		public float busStopY { get; set; }


		public string Version { get; set; }
		*/
        }

        //defaultFirst will not update the current value by default
        private bool ShouldUpdate(object curValue, object newValue, bool defaultFirst)
        {
            if(newValue == null)
            {
                return false;
            }
            else if( curValue == null )
            {
                return true;
            }

            if(curValue.Equals(newValue))
            {
                return false;
            }

            //both have a different value return the default
            return !defaultFirst;
        }
    }
}
