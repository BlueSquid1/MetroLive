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
                var y = new StreamReader(GTFSCompressed);
                string x = y.ReadToEnd();
                await fileMgr.WriteStringToFile(archiveFilePath, x);
                return true;
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
            //first load tables into RAM

            //stops.csv contains general info about the stop
            TableWithHeader stopTable = await FileContentToTable("stops.csv");
            //stop_times.csv contains the bus trips and their respected times
            TableWithHeader stopTimesTable = await FileContentToTable("stop_times.csv");

            //write down general info about the stop
            StructureWithHeader stopRow = stopTable.GetFirstInstance("stop_code", busStopId);
            string stopId = stopRow.GetField("stop_id");
            stopDetails.StopRef = stopRow.GetField("stop_code");
            stopDetails.StopPointName = stopRow.GetField("stop_name");
            stopDetails.busStopX = float.Parse(stopRow.GetField("stop_lat"));
            stopDetails.busStopY = float.Parse(stopRow.GetField("stop_lon"));
            //stopDetails.RspTimestamp
            //stopDetails.Version

            //get bus time info
            TableWithHeader stopTimesFilter = stopTimesTable.Filter("stop_id", stopId);

            //move results to return object
            for (int i = 1; i < stopTimesFilter.InterTable.Count; i++)
            {
                foreach (string field in stopTimesFilter.InterTable[i])
                {

                }
            }
            /*
            foreach( List<string> row in stopTimesFilter.InterTable)
            {

            }
            */

            //stopDetails.IncomingVehicles.Add()

            return stopDetails;
        }

        private async Task<TableWithHeader> FileContentToTable(string archiveName)
        {
            //get general info about stop
            ZipArchive zipArch = await this.GetZipFile(archiveFilePath);
            ZipArchiveEntry zipArchiveEntry = zipArch.GetEntry(archiveName);
            if(zipArchiveEntry == null)
            {
                throw new Exception("failed to find " + archiveName + " in the archive.");
            }
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
            ZipArchive archive = await this.GetZipFile(archiveFilePath);

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

        public async Task<ZipArchive> GetZipFile(string zipFileName)
        {
            OpenFile zipFile = await fileMgr.GetFileAsync(zipFileName);

            Stream zipStream = zipFile.fileStream;
            ZipArchive catchedArchive = null;
            try
            {
                catchedArchive = new ZipArchive(zipStream);
            }
            catch (Exception e)
            {
                //failed to load zip file
                throw new Exception("Failed to load zip file.");
            }

            return catchedArchive;
        }

    }
}
