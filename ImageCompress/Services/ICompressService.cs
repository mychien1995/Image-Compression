using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompress.Services
{
    public abstract class BaseCompressService
    {
        public abstract void Compress(FileInfo oldImage, string oldBaseFolder, string newBaseFolder);

        protected string GetNewPath(string oldImage, string oldPath, string newPath)
        {
            var fullPath = oldImage;
            return fullPath.Replace(oldPath, newPath);
        }
    }
}
