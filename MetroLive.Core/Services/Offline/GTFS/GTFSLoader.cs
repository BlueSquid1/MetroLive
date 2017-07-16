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
    public class OpenArchive
    {
		public string completePath;
		public ZipArchive zip;

        //constructor
        public OpenArchive( string mCompletePath, ZipArchive mZip)
        {
            this.completePath = mCompletePath;
            this.zip = mZip;
        }

    }
	public class GTFSLoader //: IOffline
	{
		public event EventHandler<DownloadProgEventArgs> DownloadProg;

		private string GTFSBaseUrl;
		private FileManager fileMgr;
        private string archiveFileName;
        private List<OpenArchive> bufferedArchives;

		//constructor
		public GTFSLoader(FileManager mFileMgr, string baseUrl)
		{
            this.bufferedArchives = new List<OpenArchive>();
			this.fileMgr = mFileMgr;
			this.GTFSBaseUrl = baseUrl;
			this.archiveFileName = "GTFSLoader.zip";
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
				await fileMgr.WriteBytesToFileAsync(archiveFileName, gtfsFile);
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


		public async Task<BusStopDetails> GetStopDataAsync(string StopRef, DateTime timeStart, DateTime timeEnd, bool forceRefresh = false)
		{
            //check if its in the buffer
            OpenArchive gtfsArchive = GetArchiveFromBuffer(this.archiveFileName);
            if (gtfsArchive == null)
            {
                string completePath = this.fileMgr.FilePathRoot + this.archiveFileName;
                ZipArchive zipFile = this.fileMgr.GetZipFile(this.archiveFileName);
                gtfsArchive = new OpenArchive(completePath, zipFile);
                this.bufferedArchives.Add(gtfsArchive);
            }

            using(Stream stopsStream = gtfsArchive.zip.GetEntry("stops.csv").Open())
            {
                
                //ServiceStack.Text.CsvSerializer.DeserializeFromStream(stopsStream);
            }

            BusStopDetails stopDetails = new BusStopDetails();

            return stopDetails;
		}

        private OpenArchive GetArchiveFromBuffer(string fileArchive)
        {
            string completePath = this.fileMgr.FilePathRoot + fileArchive;
			//check if archive file is in buffer
			foreach (OpenArchive zipArchive in this.bufferedArchives)
			{
				if (zipArchive.completePath == completePath)
				{
					return zipArchive;
				}
			}
            //failed to find archive in buffer
            return null;
        }

		/*
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
        */
	}
}
