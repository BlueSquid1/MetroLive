using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.GTFS
{
    public class StructureWithHeader
    {
        public List<string> Header { get; set; }
        public List<string> InternalStruct { get; set; }

        public StructureWithHeader(List<string> mHeader, List<string> mInternalStruct)
        {
            this.Header = mHeader;
            this.InternalStruct = mInternalStruct;
        }

        public string GetField(string mColumnName)
        {
            return InternalStruct[Header.IndexOf(mColumnName)];
        }
    }
}
