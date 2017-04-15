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
using System.Diagnostics;

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

        public async Task<BusStopDetails> GetBusStopData(string busStopId, DateTime startTime, TimeSpan timeLength)
        {
            BusStopDetails stopDetails = new BusStopDetails();

            //get general info about stop
            TableWithHeader stopTable = await FileContentToTable("stops.txt");
            StructureWithHeader stopRow = stopTable.GetFirstInstance("stop_code", busStopId);

            string stopId = stopRow.GetField("stop_id");

            stopDetails.StopRef = stopRow.GetField("stop_code");
            stopDetails.StopPointName = stopRow.GetField("stop_name");
            stopDetails.busStopX = float.Parse(stopRow.GetField("stop_lat"));
            stopDetails.busStopY = float.Parse(stopRow.GetField("stop_lon"));
            //stopDetails.RspTimestamp
            //stopDetails.Version

            //get route details
            TableWithHeader stopTimesTable = await FileContentToTable("stop_times.txt"); //4.3 seconds
            stopTimesTable.SortByColumn("arrival_time"); //5.3 seconds
            TableWithHeader stopTimesFilter = stopTimesTable.Filter("stop_id", stopId);

            //filter out the entries not in the timespan



            /*
            foreach( List<string> row in stopTimesFilter.InterTable)
            {
                Debug.WriteLine("trip id = " + row[0] + " arrival time = "  + row[1]);
            }
            */

            //Debug.WriteLine(stopTimeRow.GetField("trip_id"));

                return stopDetails;
        }

        private async Task<TableWithHeader> FileContentToTable(string archiveName)
        {
            //get general info about stop
            ZipArchive zipArch = await fileMgr.GetZipFile(archiveFilePath);
            Stream busStopStream = zipArch.GetEntry(archiveName).Open();
            //Stream to string
            StreamReader streamReader = new StreamReader(busStopStream);
            string busStopCSVString = await streamReader.ReadToEndAsync();
            List<List<string>> busStopTable = new List<List<string>>();
            List<string> busStopLines = CsvReader.ParseLines(busStopCSVString);

            List<string> StopHeader = CsvReader.ParseFields(busStopLines[0]);
            for (int i = 1; i < busStopLines.Count; i++)
            {
                busStopTable.Add(CsvReader.ParseFields(busStopLines[i]));
            }
            return new TableWithHeader(StopHeader, busStopTable);
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
