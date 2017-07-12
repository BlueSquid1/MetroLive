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
        //http://adelaidemetro.com.au/GTFS/google_transit.zip
        //https://drive.google.com/open?id=0B05c7VIZLKVQTDlwRmYzaDlQY0E
        //http://spiderpig1.duckdns.org/public%2Fgoogle_transit.zip
        public GTFSLoaderAdelaide(FileManager mFileMgr, bool mUseUncompressedGTFS = false) : base(mFileMgr, "http://spiderpig1.duckdns.org/public%2Fgoogle_transit.zip", mUseUncompressedGTFS)
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
