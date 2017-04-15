using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.GTFS
{
    public class TableWithHeader
    {
        public List<string> Header { get; set; }
        public List<List<string>> InterTable { get; set; }

        //constructor
        public TableWithHeader(List<string> mHeader, List<List<string>> mInterTable)
        {
            this.Header = mHeader;
            this.InterTable = mInterTable;
        }

        public StructureWithHeader GetFirstInstance(string columnName, string fieldValue)
        {
            int columnNum = Header.IndexOf(columnName);
            foreach(List<string> row in InterTable)
            {
                if(row[columnNum] == fieldValue)
                {
                    return new StructureWithHeader(Header, row);
                }
            }
            //failed to find the column
            throw new Exception("Failed to find field value = " + fieldValue + " in table.");
        }

        public TableWithHeader Filter(string columnName, string fieldValue)
        {
            List<List<string>> returnRows = new List<List<string>>();
            int columnNum = Header.IndexOf(columnName);
            foreach (List<string> row in InterTable)
            {
                if (row[columnNum] == fieldValue)
                {
                    returnRows.Add(row);
                }
            }
            return new TableWithHeader(Header, returnRows);
        }

        public void SortByColumn(string columnName)
        {
            int columnNum = Header.IndexOf(columnName);
            InterTable = InterTable.OrderBy(i => i[columnNum]).ToList();
        }
    }
}
