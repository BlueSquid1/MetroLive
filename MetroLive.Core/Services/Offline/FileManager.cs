using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MetroLive.Services.Offline
{
    public class FileManager
    {
        //default file path
        public string FilePathRoot { get; set; }

        //constructor
        public FileManager(string mFilePath)
        {
            this.FilePathRoot = mFilePath;
        }

        //will overwrite the existing file if it exists
        public async Task WriteBytesToFileAsync(string targetFile, byte[] fileData)
        {
            string completePath = this.FilePathRoot + targetFile;

            //overwrite anything currently stored in that file
            using( FileStream fileStreamW = File.Open(completePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                await fileStreamW.WriteAsync(fileData, 0, fileData.Length);
                await fileStreamW.FlushAsync();
            }
        }


        public FileInfo GetFile(string targetFile)
        {
            string completeFilePath = FilePathRoot + targetFile;

            //need to load file from disk
            FileInfo fileInfo = new FileInfo(completeFilePath);
            if( fileInfo.Exists == false)
            {
                throw new FileNotFoundException("Unable to get file.", completeFilePath);
            }

            return fileInfo;
        }

        public ZipArchive GetZipFile(string targetFile)
        {
            FileInfo fileInfo = this.GetFile(targetFile);
            FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read);
			ZipArchive zipArchive = new ZipArchive(fileStream);
			return zipArchive;
        }
    }
}
