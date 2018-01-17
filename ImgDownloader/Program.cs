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
    class Program
    {
        // file path to save
        static string folder = @"D:\img\";
        // R:3020, SR:3030, SSR:3040
        static string[] rares = { "3020", "3030", "3040" };
        static int newCnt = 0;

        static List<int> successL;

        static string[] size = { "s", "m", "f", "detail", "zoom" };


        static void Main(string[] args)
        {
            foreach (var rare in rares)
            {
                successL = new List<int>();
                // Check file exist
                Parallel.For(0, 1000, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
                {
                    var uri = "http://game-a1.granbluefantasy.jp/assets/img/sp/assets/npc/s/" + rare + String.Format("{0:D3}", i) + "000_01.jpg";
                    CheckURL(i, uri);
                });
                Console.WriteLine("Checked");
                Thread.Sleep(1000);

                // Download
                for (int i = 0; i < size.Count(); i++)
                {
                    var path = folder + rare + @"\" + size[i] + @"\";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    try
                    {
                        Parallel.ForEach(successL, new ParallelOptions { MaxDegreeOfParallelism = 10 }, j =>
                        {
                            var k = 1;
                            var result = false;
                            while (true)
                            {
                                var filename = rare + String.Format("{0:D3}", j) + "000_" + String.Format("{0:D2}", k);
                                if (i > 2) filename += ".png";
                                else filename += ".jpg";
                                var uri = "http://game-a1.granbluefantasy.jp/assets/img/sp/assets/npc/" + size[i] + "/" + filename;
                                result = DownloadImg(uri, path + filename);
                                k++;
                                if (!result) break;
                            }
                        });
                    }
                    catch
                    {
                        Console.WriteLine("error");
                    }
                }
            }


            Console.WriteLine("Finished : Saved " + newCnt + " image(s).");
            Console.ReadLine();

        }
        static bool DownloadImg(string imgUri, string outputPath)
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
        static void CheckURL(int i, string imgUri)
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
                        successL.Add(i);
                        // Console.WriteLine("Add List : " + i);
                        return;
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
        }
    }
}
