﻿using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.GTFS;
using System.Threading;

namespace MetroLive.Common
{
    public abstract class FileManager
    {
        //default file path
        protected string filePathRoot;

        protected FileSystemFolder fileSystem;
        protected List<OpenFile> CachedFiles;

        //constructor
        public FileManager(string mFilePath)
        {
            CachedFiles = new List<OpenFile>();
            this.fileSystem = new FileSystemFolder("./");
            this.filePathRoot = mFilePath;
        }

        //deconstructor
        ~FileManager()
        {
            foreach(OpenFile file in CachedFiles)
            {
                file.fileStream.Flush();
                file.fileStream.Dispose();
            }
        }

        public async Task<bool> DoesFolderExist(string path)
        {
            IFolder gtfsFolder = await this.fileSystem.GetFolderAsync(path, new CancellationToken());
            return gtfsFolder != null;
        }

        //will overwrite the existing file if it exists
        public async Task WriteStringToFile(string targetFile, string text)
        {
            //attempt to retrieve file
            OpenFile activeFile = await GetFileAsync(targetFile);

            IFile updateFile = null;
            //check if file already exists
            if (activeFile != null)
            {
                //clear from cache
                CachedFiles.Remove(activeFile);
                updateFile = activeFile.fileInfo;
                await activeFile.fileStream.FlushAsync();
                activeFile.fileStream.Dispose();
            }
            else
            {
                //create the file
                string completeFilePath = filePathRoot + targetFile;
                IFolder root = await fileSystem.GetFolderAsync("./", new CancellationToken());
                updateFile = await root.CreateFileAsync(completeFilePath, CreationCollisionOption.FailIfExists);
            }

            //write content 
            await updateFile.WriteAllTextAsync(text);
        }

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