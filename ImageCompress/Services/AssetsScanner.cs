using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageCompress.Extensions;

namespace ImageCompress.Services
{
    public class AssetsScanner
    {
        private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };
        private static readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();

        public static void ScanFolder(string rootPath)
        {
            Console.WriteLine("AssetsScanner: Start Scanning");
            Queue<string> bfsQueue = new Queue<string>();
            bfsQueue.Enqueue(rootPath);
            while (bfsQueue.Count > 0)
            {
                var folderPath = bfsQueue.Dequeue();
                var directory = new DirectoryInfo(folderPath);
                FileInfo[] files = directory.GetFiles();
                AddToProcessQueue(files);
                var subFolders = Directory.GetDirectories(folderPath);
                foreach (var child in subFolders)
                {
                    bfsQueue.Enqueue(child);
                }
            }
        }

        public static void AddToProcessQueue(FileInfo[] files)
        {
            if (!AssetsDistributor.Ready()) return;
            foreach (var file in files)
            {
                if (AllowedExtensions.Contains(file.Extension))
                {
                    AssetsDistributor.Enqueue(file);
                }
            }
        }
    }
}
