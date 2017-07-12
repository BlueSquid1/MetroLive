using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Common.EventArgs
{
    public class DownloadProgEventArgs : System.EventArgs
    {
        public float Percentage { get; set; }

        //constructor
        public DownloadProgEventArgs(int mPercentage)
        {
            this.Percentage = mPercentage;
        }
    }
}
