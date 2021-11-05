﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LMS_Api.Utils
{
    public static class FileHelper
    {
        static FileHelper()
        {
            uniqueId = Guid.NewGuid().ToString();
        }

        public static string uniqueId;

        public static string path = WebEnv().WebRootPath;

        public static IHostingEnvironment WebEnv()
        {
            var _accessor = new HttpContextAccessor();
            return _accessor.HttpContext.RequestServices.GetRequiredService<IHostingEnvironment>();
        }

        public async static Task<string> AddFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                string _fileName = uniqueId + file.FileName;
                using (FileStream fileStream = File.Create(Path.Combine(path,"images", _fileName)))
                {
                    await file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }

                return _fileName;
            }
            else
            {
                return null;
            }
        }

        public static void DeleteFile(string fileName)
        {
            string _deletePath = Path.Combine(path, "images", fileName);
            File.Delete(_deletePath);
        }
    }
}
