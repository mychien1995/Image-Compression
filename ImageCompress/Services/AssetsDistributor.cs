using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageCompress.Extensions;

namespace ImageCompress.Services
{
    public class AssetsDistributor
    {
        private const int MaxThread = 10;
        private static BlockingCollection<FileInfo> ScannedImages;
        private static readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();

        public static bool Ready()
        {
            using (_readerWriterLock.Read())
            {
                return ScannedImages != null;
            }
        }

        public static void Init()
        {
            if (ScannedImages == null) ScannedImages = new BlockingCollection<FileInfo>();
        }

        public static void Enqueue(FileInfo imagePath)
        {
            Console.WriteLine($"AssetsDistributor: Add {imagePath.Name} to queue");
            ScannedImages.Add(imagePath);
        }

        public static FileInfo Dequeue()
        {
            if (ScannedImages.Count == 0) return null;
            FileInfo file = ScannedImages.Take();
            if (file != null)
                Console.WriteLine($"AssetsDistributor: Remove {file.Name} from queue");
            return file;
        }

    }

    public class ProcessRunner
    {
        private readonly Thread thread;
        private readonly string _oldFolderPath;
        private readonly string _newFolderPath;
        private readonly string _compareFolder = "E:\\Test\\CMS_3\\";
        private bool IsStop;

        public ProcessRunner(string oldFolderPath, string newFolderPath)
        {
            thread = new Thread(new ThreadStart(Run));
            _oldFolderPath = oldFolderPath;
            _newFolderPath = newFolderPath;
            IsStop = false;
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            IsStop = true;
        }

        private void Run()
        {
            while (!IsStop)
            {
                var image = AssetsDistributor.Dequeue();
                if (image == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (image.Extension == ".jpg" || image.Extension == ".jpeg")
                {
                    Task.Run(() =>
                    {
                        System.IO.File.Copy(image.FullName, _compareFolder + Guid.NewGuid() + image.Extension);
                        new JpegCompressService().Compress(image, _oldFolderPath, _newFolderPath);
                    });
                }
                else if (image.Extension == ".png")
                {
                    Task.Run(() =>
                    {
                        System.IO.File.Copy(image.FullName, _compareFolder + Guid.NewGuid() + image.Extension);
                        new PngCompressService().Compress(image, _oldFolderPath, _newFolderPath);
                    });
                }
            }
        }
    }
}
