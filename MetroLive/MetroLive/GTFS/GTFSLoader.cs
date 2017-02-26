using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive
{
    public class GTFSLoader
    {
        private string GTFSBaseUrl;
        //constructor
        public GTFSLoader(string GTFSBase)
        {
            this.GTFSBaseUrl = GTFSBase;
        }

        public async Task LoadSchedule()
        {
            HttpClient httpClient = new HttpClient();
            Stream GTFSCompressed =await httpClient.GetStreamAsync(GTFSBaseUrl);

            //System.IO.Compression.GZipStream gZip = new System.IO.Compression.GZipStream(GTFSCompressed, System.IO.Compression.CompressionLevel.Fastest);
            //gZip.
            //x.
            //System.IO.Compression.ZipFile x;
        }
    }
}
