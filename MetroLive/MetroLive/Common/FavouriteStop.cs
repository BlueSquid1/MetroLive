using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Common
{
    public class FavouriteStop
    {
        //stop reference number
        public string stopId { get; set; }

        //custom name set by the user
        public string customName { get; set; }

        public FavouriteStop(string mStopId, string mCustomName = null)
        {
            this.stopId = mStopId;
            this.customName = mCustomName;
        }
    }
}
