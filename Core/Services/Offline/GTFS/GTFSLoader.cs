using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using System.Diagnostics;
using MetroLive.Models;

namespace MetroLive.Services.Offline.GTFS
{
	public class GTFSLoader //: IOffline
	{
		public event EventHandler<DownloadProgEventArgs> DownloadProg;


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

        //assume file length is less than 4Gb
        async private Task<byte[]> DownloadGTFS()
        {
            byte[] downloadFile;

            DownloadProg?.Invoke(this, new DownloadProgEventArgs(0, 0));

			using (var httpClient = new HttpClient())
			using (var dataStream = await httpClient.GetStreamAsync(GTFSBaseUrl))
			{
                int fileLen = (int)dataStream.Length;
				int receivedBytes = 0;
				byte[] buffer = new byte[fileLen];

                downloadFile = new byte[fileLen];

				while (receivedBytes < fileLen)
				{
					int bytesRead = await dataStream.ReadAsync(buffer, 0, buffer.Length);
					Array.Copy(buffer, 0, downloadFile, receivedBytes, bytesRead);
					receivedBytes += bytesRead;
                    DownloadProg?.Invoke(this, new DownloadProgEventArgs(receivedBytes,fileLen));
				}
			}
            return downloadFile;
        }

		public async Task<bool> UpdateAsync()
		{
			try
			{
                Console.WriteLine("downloading GTFS");
                byte[] gtfsFile = await this.DownloadGTFS();
				fileMgr.WriteBytesToFile(archiveFilePath, gtfsFile);
				Console.WriteLine("done writing file");
				return true;
			}
			catch
			{
                Console.WriteLine("Failed to update GTFS.");
				return false;
			}
		}

		private byte[] ReadBytesAsync(Stream dataStream, int bytesToRead)
		{
			Byte[] steamContent = new Byte[bytesToRead];
			int numBytesProcessed = 0;
			while (numBytesProcessed < bytesToRead)
			{
				//ReadAsync() can return less then intSize therefore keep on looping until intSize is reached
				byte[] dataBuffer = new Byte[bytesToRead - numBytesProcessed];
				int bytesRead = dataStream.Read(dataBuffer, 0, bytesToRead - numBytesProcessed);
				Array.Copy(dataBuffer, 0, steamContent, numBytesProcessed, bytesRead);
				numBytesProcessed += bytesRead;
			}
			return steamContent;
		}

		public Task<BusStopDetails> GetStopDataAsync(string StopRef, DateTime timeStart, DateTime timeEnd, bool forceRefresh = false)
		{
			throw new NotImplementedException();
		}

		/*
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
                    Debug.WriteLine(field);
                }
            }

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
            if (this.useUncompressedGTFS == false)
            {
                ZipArchive archive = await this.GetZipFile(archiveFilePath);

                if (archive == null)
                {
                    return false;
                }
            }
            else
            {
                return await fileMgr.DoesFolderExist("./GTFSLoader");
            }

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
        */
	}
}
