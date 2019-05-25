using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageCompress.Services;

namespace ImageCompress
{
    class Program
    {
        private const string OldImageFolder = "E:\\Test\\CMS";
        private const string NewImageFolder = "E:\\Test\\CMS_2";
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            AssetsDistributor.Init();
            Console.WriteLine("Assets Distributor Init Done");
            var processRunner = new ProcessRunner(OldImageFolder, NewImageFolder);
            AssetsScanner.ScanFolder(OldImageFolder);
            processRunner.Start();
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
