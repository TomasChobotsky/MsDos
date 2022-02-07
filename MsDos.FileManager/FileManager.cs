using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MsDos
{
    public class FileManager
    {
        public List<DirFile> Directories { get; set; } = new List<DirFile>();
        public void ReadDirectories(string path)
        {
            var directoryInfoList = Directory.GetDirectories(path).Select(x => new DirectoryInfo(x));
            var fileList = Directory.GetFiles(path).Select(x => new FileInfo(x));
            Directories = new List<DirFile>();
            foreach (var directoryInfo in directoryInfoList)
            {
                Directories.Add(new DirFile(directoryInfo.Name, 
                    directoryInfo.LastWriteTime.ToString(CultureInfo.CreateSpecificCulture("de-DE")), "<SUB-DIR>", false));
            }

            foreach (var fileInfo in fileList)
            {
                Directories.Add(new DirFile(fileInfo.Name, 
                    fileInfo.LastWriteTime.ToString(CultureInfo.CreateSpecificCulture("de-DE")), (int)Math.Round((double)fileInfo.Length / 1024, 0) + "kB", true));
            }
        }
    }
}