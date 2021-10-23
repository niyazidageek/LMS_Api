using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace LMS_Api.Utils
{
    public static class FileHelper
    {
        static FileHelper()
        {
            uniqueId = Guid.NewGuid().ToString();
        }

        public static string uniqueId;

        public static string path = "/Users/niyazibabayev/Desktop/LMS_FrontEnd/lms-app/src/assets/materials/";

        public static string AddFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                string _fileName = uniqueId + file.FileName;
                using (FileStream fileStream = System.IO.File.Create(path + _fileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }

                return _fileName;
            }
            else
            {
                return null;
            }
        }
    }
}
