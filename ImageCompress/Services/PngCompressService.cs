using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nQuant;

namespace ImageCompress.Services
{
    public class PngCompressService : BaseCompressService
    {
        public override void Compress(FileInfo oldImage, string oldBaseFolder, string newBaseFolder)
        {
            var quantizer = new WuQuantizer();
            var imageFullPath = oldImage.FullName;
            using (Bitmap bmp1 = new Bitmap(imageFullPath))
            {
                var bitMap = bmp1;
                if (bmp1.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                {
                    ConvertTo32bppAndDisposeOriginal(ref bitMap);
                }
                using (var quantized = quantizer.QuantizeImage(bitMap))
                {
                    var newPath = GetNewPath(oldImage.FullName, oldBaseFolder, newBaseFolder);
                    var newFolderPath = GetNewPath(oldImage.DirectoryName, oldBaseFolder, newBaseFolder);
                    if (!Directory.Exists(newFolderPath))
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }
                    quantized.Save(newPath, ImageFormat.Png);
                }
            }

            Console.WriteLine($"PngCompressService: Compress Done {oldImage.Name}");
        }

        private void ConvertTo32bppAndDisposeOriginal(ref Bitmap img)
        {
            var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            img.Dispose();
            img = bmp;
        }
    }
}
