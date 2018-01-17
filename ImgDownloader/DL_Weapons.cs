using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImgDownloader
{
    public class DL_Weapons
    {
        // file path to save
        static string folder = Downloader.rootF + @"weapons\";
        // N:1010 R:1020, SR:1030, SSR:1040
        static string[] rares = { "1010", "1020", "1030", "1040" };

        static List<int> successL;

        static string[] size = { "m", "ls", "b" };

        public static void DL()
        {
            Console.WriteLine("--- Weapons ---");
            foreach (var rare in rares)
            {
                successL = new List<int>();
                // Check file exist
                Console.WriteLine("Check urls");
                Parallel.For(0, 10000, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
                {
                    var uri = "http://game-a1.granbluefantasy.jp/assets/img/sp/assets/weapon/b/" + rare + String.Format("{0:D4}", i) + "00.png";
                    if (Downloader.CheckURL(i, uri)) successL.Add(i);
                });
                Console.WriteLine("Check result : " + successL.Count);
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
                            var result = false;
                            while (true)
                            {
                                var filename = rare + String.Format("{0:D4}", j) + "00";
                                filename += ".png";
                                var uri = "http://game-a1.granbluefantasy.jp/assets/img/sp/assets/weapon/" + size[i] + "/" + filename;
                                result = Downloader.DownloadImg(uri, path + filename);
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
        }
    }
}
