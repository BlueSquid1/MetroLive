using MetroLive.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MetroLive.MetroData;
using ServiceStack.Text;
using MetroLive.Common.EventArgs;

namespace MetroLive.GTFS
{
    public class GTFSLoader
    {
        public event EventHandler<DownloadProgEventArgs> downloadProg;


        private string GTFSBaseUrl;
        private FileManager fileMgr;
        private string archiveFilePath;

        //constructor
        public GTFSLoader(FileManager mFileMgr, string baseUrl)
        {
            this.fileMgr = mFileMgr;
            this.GTFSBaseUrl = baseUrl;
            archiveFilePath = "GTFSLoader.zip";
        }

        public async Task<bool> DownloadTimeTable()
        {
            //download zip file
            HttpClient httpClient = null;
            Stream GTFSCompressed = null;
            try
            {
                httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);
                GTFSCompressed = await httpClient.GetStreamAsync(GTFSBaseUrl);
                bool writeSuccess = await fileMgr.WriteStreamToArchive(archiveFilePath, GTFSCompressed);
                return writeSuccess;
            }
            catch
            {
                httpClient?.CancelPendingRequests();
                GTFSCompressed?.Dispose();
                httpClient?.Dispose();
                return false;
            }
        }

        //TODO
        public async Task<BusStopDetails> GetBusStopData(string busStopId, DateTimeOffset timeInterval)
        {
            //get general info about stop
            ZipArchive zipArch = await fileMgr.GetZipFile(archiveFilePath);
            Stream busStopStream = zipArch.GetEntry("stops.txt").Open();
            //Stream to string
            StreamReader streamReader = new StreamReader(busStopStream);
            string busStopCSVString = await streamReader.ReadToEndAsync();
            List<string> busStopList = CsvReader.ParseLines(busStopCSVString);
            return new BusStopDetails();
        }

        
        public async Task<bool> IsTimeTableUptoDate()
        {
            ZipArchive archive = await fileMgr.GetZipFile(archiveFilePath);

            if(archive == null)
            {
                return false;
            }

            /*
            Stream feedInfoStream = archive.GetEntry("feed_info.txt").Open();
            StreamReader streamReader = new StreamReader(feedInfoStream);
            string feedInfoCSVString = await streamReader.ReadToEndAsync();
            List<string> feedInfoList = CsvReader.ParseLines(feedInfoCSVString);
            */
            return true;
        }

    }
}
