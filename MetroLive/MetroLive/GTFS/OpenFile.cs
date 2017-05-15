using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.GTFS
{
    public class OpenFile
    {
        public IFile fileInfo { get; set; }
        public Stream fileStream { get; set; }

        //constructor
        public OpenFile(IFile mFileInfo, Stream mFileStream)
        {
            this.fileInfo = mFileInfo;
            this.fileStream = mFileStream;
        }
    }
}
