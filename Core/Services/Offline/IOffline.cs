using System;
using System.Threading.Tasks;
using MetroLive.Models;

namespace MetroLive.Services.Offline
{
	public class DownloadProgEventArgs : EventArgs
	{
        public int bytesProccessed;
        public int fileSize;
		public float Percentage 
        { 
            get
            {
                return ((float)(this.bytesProccessed)) / fileSize;
            }
        }

		//constructor
		public DownloadProgEventArgs(int mBytesProccessed, int mFileSize)
		{
            this.bytesProccessed = mBytesProccessed;
            this.fileSize = mFileSize;
		}
	}

    interface IOffline
    {
        //keeping track of update status
        event EventHandler<DownloadProgEventArgs> DownloadProg;
        Task<bool> UpdateAsync();

        Task<BusStopDetails> GetStopDataAsync(string StopRef, DateTime timeStart, DateTime timeEnd, bool forceRefresh = false);
    }
}
