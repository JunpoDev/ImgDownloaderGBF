using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImgDownloader
{
    public class DL_Characters
    {

        // file path to save
        static string folder = Downloader.rootF + @"character\";
        // R:3020, SR:3030, SSR:3040
        static string[] rares = { "3020", "3030", "3040" };

        static List<int> successL;

        static string[] size = { "s", "m", "f", "detail", "zoom" };

        public static void DL()
        {
            Console.WriteLine("--- Characters ---");
            foreach (var rare in rares)
            {
                successL = new List<int>();
                // Check file exist
                Console.WriteLine("Check urls");
                Parallel.For(0, 1000, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
                {
                    var uri = "http://game-a1.granbluefantasy.jp/assets/img/sp/assets/npc/s/" + rare + String.Format("{0:D3}", i) + "000_01.jpg";
                    if(Downloader.CheckURL(i, uri)) successL.Add(i);
                });
                Console.WriteLine("Check result : "+successL.Count );
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
                                result = Downloader.DownloadImg(uri, path + filename);
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
        }
    }
}
