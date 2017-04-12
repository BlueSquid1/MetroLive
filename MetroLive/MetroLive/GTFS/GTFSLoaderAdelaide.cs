using MetroLive.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.GTFS
{
    public class GTFSLoaderAdelaide : GTFSLoader
    {
        //constructor
        public GTFSLoaderAdelaide(FileManager mFileMgr) : base(mFileMgr, "http://adelaidemetro.com.au/GTFS/google_transit.zip")
        {

        }

        /*
        public async Task LoadSchedule()
        {
            HttpClient httpClient = new HttpClient();
            Stream GTFSCompressed = await httpClient.GetStreamAsync(GTFSBaseUrl);

            //System.IO.Compression.GZipStream gZip = new System.IO.Compression.GZipStream(GTFSCompressed, System.IO.Compression.CompressionLevel.Fastest);
            //gZip.
            //x.
            //System.IO.Compression.ZipFile x;
        }
        */
    }
}
