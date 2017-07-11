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
	public class OpenFile
	{
		public FileInfo fileInfo { get; set; }
		public FileStream fileStream { get; set; }

		//constructor
		public OpenFile(FileInfo mFileInfo, FileStream mFileStream)
		{
			this.fileInfo = mFileInfo;
			this.fileStream = mFileStream;
		}
	}

    public class FileManager
    {
        //default file path
        protected string filePathRoot;

        private List<OpenFile> bufferedFiles;

        //constructor
        public FileManager(string mFilePath)
        {
            this.filePathRoot = mFilePath;
            this.bufferedFiles = new List<OpenFile>();
        }

        //deconstructor
        ~FileManager()
        {
            foreach (OpenFile openFile in bufferedFiles)
            {
                openFile.fileStream.Dispose();
            }
        }

        //will overwrite the existing file if it exists
        public void WriteBytesToFile(string targetFile, byte[] fileData)
        {
            
            //check if there is an entry in the buffer
            OpenFile openFile = GetFileFromBuffer(targetFile);
            FileStream fileStream = null;
            if (openFile != null)
            {
                //close the file pointer
                openFile.fileStream.Dispose();
            }

            //wipe anything currently stored in that file
            fileStream = File.Open(targetFile, FileMode.Create, FileAccess.ReadWrite);

            fileStream.Write(fileData, 0, fileData.Length);
            fileStream.Flush();
            fileStream.Dispose();

			if (openFile == null)
			{
				//add to buffer
				//bufferedFiles.Add(new OpenFile(fileInfo, fileStream));
			}
            else
            {
				//update metadata
				//openFile.fileInfo = fileInfo;
                //openFile.fileStream = fileStream;
            }
        }

        public OpenFile GetFileFromBuffer(string filePath)
        {
            string completePath = this.filePathRoot + filePath;
            foreach(OpenFile openFile in bufferedFiles)
            {
                if (openFile.fileInfo.FullName == completePath)
                {
                    return openFile;
                }
            }
            //file not in buffer
            return null;
        }

        /*
        public async Task<OpenFile> GetFileAsync(string targetFile, bool writePermission = false)
        {
            string completeFilePath = filePathRoot + targetFile;

            //first check if file in in cache
            foreach (OpenFile file in CachedFiles)
            {
                if( file.fileInfo.Path == completeFilePath)
                {
                    //read file from start
                    file.fileStream.Seek(0, SeekOrigin.Begin);
                    //found file in cache
                    return file;
                }
            }

            //check if file already exists
            PCLStorage.FileAccess fileAccess = PCLStorage.FileAccess.Read;
            if(writePermission == true)
            {
                fileAccess = PCLStorage.FileAccess.ReadAndWrite;
            }
            IFile activeFile = await fileSystem.GetFileAsync(completeFilePath, new CancellationToken());
            if(activeFile != null)
            {
                Stream fileStream = await activeFile.OpenAsync(fileAccess);
                fileStream.Seek(0, SeekOrigin.Begin);
                OpenFile openFile = new OpenFile(activeFile, fileStream);
                CachedFiles.Add(openFile);
                return openFile;
            }

            //failed to find the file
            return null;
        }
        */


        /*
        public async Task<string> ReadStringFromFile(string targetFile)
        {
            string completeFilePath = filePathRoot + targetFile;

            IFile file = await fileSystem.GetFileAsync(completeFilePath, new CancellationToken());

            if(file == null)
            {
                return null;
            }

            return await file.ReadAllTextAsync();
        }
        */
        

        /*
        public async Task<ZipArchive> GetZipFile(string archiveName)
        {
            string completeFilePath = filePathRoot + archiveName;

            IFile file = await fileSystem.GetFileFromPathAsync(completeFilePath);

            if ( file == null )
            {
                return null;
            }

            Stream zipStream = await file.OpenAsync(FileAccess.Read);
            ZipArchive catchedArchive = null;
            try
            {
                catchedArchive = new ZipArchive(zipStream);
            }
            catch(Exception e)
            {
                //failed to load zip file
                throw new Exception("Failed to load zip file.");
            }

            //ZipArchiveEntry compressedFile = catchedArchive.GetEntry(fileName);
            return catchedArchive;

        }
        */
        

        /*
        public async Task<ZipArchiveEntry> GetArchiveEntryAsync(string fileName)
        {
            //check if zip archive is already in RAM
            if(CatchedArchive != null)
            {
                ZipArchiveEntry cachedEntry = CatchedArchive.GetEntry(fileName);
                return cachedEntry;
            }

            //not in RAM check if its on Disk
            bool archiveExist = await DoesArchiveExist();

            if(!archiveExist)
            {
                //no archive to explore
                return null;
            }

            string timetablePath = FilePathRoot + gtfsFileName;

            IFile file = await fileSystem.GetFileFromPathAsync(timetablePath);
            
            Stream timeTableStream = await file.OpenAsync(FileAccess.Read);

            this.CatchedArchive = new ZipArchive(timeTableStream);

            ZipArchiveEntry compressedFile = CatchedArchive.GetEntry(fileName);
            return compressedFile;
        }
        */
    }
}
