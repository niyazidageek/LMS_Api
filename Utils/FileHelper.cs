using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace LMS_Api.Utils
{
    public static class FileHelper
    {

        public static string path = WebEnv().WebRootPath;

        public static IHostingEnvironment WebEnv()
        {
            var _accessor = new HttpContextAccessor();
            return _accessor.HttpContext.RequestServices.GetRequiredService<IHostingEnvironment>();
        }

        public async static Task<string> AddJsonFile(string json)
        {
            if (json.Length > 0)
            {
                string uniqueId = Guid.NewGuid().ToString();

                string _fileName = uniqueId + ".txt";

                using (FileStream fileStream = File.Create(Path.Combine(path, "images", _fileName)))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(json);

                    await fileStream.WriteAsync(info, 0, info.Length);
                }

                return _fileName;
            }
            else
            {
                return null;
            }
        }

        public async static Task<string> AddFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                string uniqueId = Guid.NewGuid().ToString();

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
