﻿using System;
using System.Collections.Generic;
using System.IO;
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

        //Checks if the timetable is avaliable locally
        public virtual async Task<bool> TimeTableAvaliableOffline()
        {
            return false;
            /*
            HttpClient httpClient = new HttpClient();
            Stream GTFSCompressed =await httpClient.GetStreamAsync(GTFSBaseUrl);

            //System.IO.Compression.GZipStream gZip = new System.IO.Compression.GZipStream(GTFSCompressed, System.IO.Compression.CompressionLevel.Fastest);
            //gZip.
            //x.
            //System.IO.Compression.ZipFile x;
            */
        }
    }
}
