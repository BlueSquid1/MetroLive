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

        //TODO
        public async Task<bool> UpdateTimeTable()
        {
            HttpClient httpClient = new HttpClient();
            Stream GTFSCompressed = await httpClient.GetStreamAsync(GTFSBaseUrl);
            await fileMgr.WriteToFileAsync(GTFSCompressed);
            
            return true;
        }

        //Checks if the timetable is avaliable locally
        public virtual async Task<bool> TimeTableAvaliableOffline()
        {
            //TODO
            return false;
        }
    }
}
