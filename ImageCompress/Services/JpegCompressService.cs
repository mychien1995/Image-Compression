using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

namespace ImageCompress.Services
{
    public class JpegCompressService : BaseCompressService
    {
        public override void Compress(FileInfo oldImage, string oldBaseFolder, string newBaseFolder)
        {
            try
            {
                var imageFullPath = oldImage.FullName;
                var newQuality = 85;
                using (MagickImage image = new MagickImage(imageFullPath))
                {
                    if (image.Quality <= 60) return;
                    newQuality = image.Quality - 10;
                }
                using (Bitmap bmp1 = new Bitmap(imageFullPath))
                {

                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder =
                        System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, newQuality);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    var newPath = GetNewPath(oldImage.FullName, oldBaseFolder, newBaseFolder);
                    var newFolderPath = GetNewPath(oldImage.DirectoryName, oldBaseFolder, newBaseFolder);
                    if (!Directory.Exists(newFolderPath))
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }

                    bmp1.Save(newPath, jpgEncoder, myEncoderParameters);
                    Console.WriteLine($"JpegCompressService: Compress Done {oldImage.Name}");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
