using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.Models;

namespace MetroLive.Services.Realtime
{
    public interface IRealtime
    {
        Task<BusStopDetails> GetStopDataAsync(string StopRef, TimeSpan timeLength, bool forceRefresh = false);
    }
}
