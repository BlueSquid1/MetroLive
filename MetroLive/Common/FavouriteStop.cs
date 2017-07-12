using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MetroLive.Common
{
    public class FavouriteStop
    {
        //stop reference number
        public string StopId { get; set; }

        //custom name set by the user
        public string CustomName { get; set; }

        public Color CustColour { get; set; }

        public FavouriteStop(string mStopId, string mCustomName, Color mCustColour)
        {
            this.StopId = mStopId;
            this.CustomName = mCustomName;
            this.CustColour = mCustColour;
        }
    }
}
