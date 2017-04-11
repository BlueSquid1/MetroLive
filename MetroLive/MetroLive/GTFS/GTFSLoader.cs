using MetroLive.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.GTFS
{
    public class GTFSLoader
    {
        protected string GTFSBaseUrl;
        protected FileManager fileMgr;

        //constructor
        public GTFSLoader(FileManager mFileMgr, string baseUrl)
        {
            this.fileMgr = mFileMgr;
            this.GTFSBaseUrl = baseUrl;
        }

        public async Task DownloadTimeTable()
        {
            //download zip file
            HttpClient httpClient = new HttpClient();
            Stream GTFSCompressed = await httpClient.GetStreamAsync(GTFSBaseUrl);
            await fileMgr.WriteStreamToArchive("GTFSLoader.zip", GTFSCompressed);
        }

        /*
        public virtual async Task<bool> TimeTableUptoDate()
        {
            ZipArchiveEntry feedInfo = await fileMgr.GetArchiveEntryAsync("feed_info.txt");
            if(feedInfo == null)
            {
                return false;
            }

            Stream infoStream = feedInfo.Open();
            BinaryReader binReader = new BinaryReader(infoStream);
            string version = binReader.ReadString();
            return true;
        }
        */
    }
}
