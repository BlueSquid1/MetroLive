using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Services.Offline.GTFS
{
    public class GTFSLoaderAdelaide : GTFSLoader
    {
		//constructor
		//http://adelaidemetro.com.au/GTFS/google_transit.zip
		//https://drive.google.com/open?id=0B05c7VIZLKVQTDlwRmYzaDlQY0E
		//http://spiderpig1.duckdns.org/public%2Fgoogle_transit.zip
		//http://192.168.1.201/%2Fpublic%2FGTFS%2FAdelaide%2Ffeed_info.csv
		public GTFSLoaderAdelaide(FileManager mFileMgr) : base(mFileMgr, "http://192.168.1.201/%2Fpublic%2FGTFS%2FAdelaide%2Fgoogle_transit.zip", "http://192.168.1.201/%2Fpublic%2FGTFS%2FAdelaide%2Ffeed_info.csv")
        {

		}
    }
}
