using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.BusStop;

namespace MetroLive.Common
{
    //main interface for all logic operations
    public abstract class MetroLiveCore
    {
        protected virtual string GTFSBaseUrl { get; set; }
        protected virtual string SIRIBaseUrl { get; set; }

        public MetroLiveSettings Settings { get; set; }

        //protected 

        //constructor
        public MetroLiveCore()
        {

        }

        public abstract BusStopDetails GetBusStopDetails(string busId);
    }
}
