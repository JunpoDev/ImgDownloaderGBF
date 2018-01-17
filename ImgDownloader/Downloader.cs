using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImgDownloader
{
    class Downloader
    {
        public static string rootF = @"D:\Image\";
        static int newCnt = 0;

        static void Main(string[] args)
        {
            DL_Weapons.DL();
            DL_Characters.DL();

            Console.WriteLine("Finished : Saved " + newCnt + " image(s).");
            Console.ReadLine();

        }
        public static bool DownloadImg(string imgUri, string outputPath)
        {
            // check exist file
            if (File.Exists(outputPath)) return true;

            var client = new HttpClient();

            var cnt = 0;
            while (cnt++ < 10)
            {
                try
                {
                    HttpResponseMessage res = client.GetAsync(imgUri, HttpCompletionOption.ResponseContentRead).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        using (var fileStream = File.Create(outputPath))
                        using (var httpStream = res.Content.ReadAsStreamAsync().Result)
                            httpStream.CopyTo(fileStream);
                        Console.WriteLine("Saved at : " + outputPath);
                        newCnt++;
                        return true;
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
            return false;
        }
        public static bool CheckURL(int i, string imgUri)
        {
            var client = new HttpClient();
            var cnt = 0;
            while (cnt++ < 3)
            {
                try
                {
                    HttpResponseMessage res = client.GetAsync(imgUri, HttpCompletionOption.ResponseContentRead).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        // Console.WriteLine("Add List : " + i);
                        return true;
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
            return false;
        }
    }
}
