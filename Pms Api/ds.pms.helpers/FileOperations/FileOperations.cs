using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ds.pms.helpers.FileOperations
{
    public static class FileOperations
    {
        public static byte[] ReadFileStream(IFormFile formFile )
        {
            byte[] fileData = null;
            using (Stream fs = formFile.OpenReadStream())
            {
                var binaryReader = new BinaryReader(fs);
                fileData = binaryReader.ReadBytes((int)fs.Length);

            }
            return fileData;
        }
    }
}
