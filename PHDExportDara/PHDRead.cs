using System;
using System.IO;
using System.Threading;
using Uniformance.PHD;

namespace PHDExportData
{
    public class PHDRead
    {
        public PHDRead()
        {

        }

        public static void Read (string tag, Tag MyTag, PHDHistorian phd, Double[] timestamps, Double[] values, short[] confidences, string pathOutput)
        {
            Console.WriteLine($"{DateTime.Now} - Start PHD query, tag: {tag}");
            phd.FetchData(MyTag, ref timestamps, ref values, ref confidences);
            var pathOutput1 = pathOutput + tag + ".txt";
            var writer = File.CreateText(pathOutput1);
            Console.WriteLine($"{DateTime.Now} - End PHD query, tag: {tag}");
            if (timestamps != null)
            {
                int count = timestamps.GetUpperBound(0);
                Console.WriteLine($"{DateTime.Now} - Start writing to file");
                Console.WriteLine($"{DateTime.Now} - Retrieved {count} data points.");

                using (writer)
                {
                    writer.WriteLine($"No; timestamp; value; confidence");
                    for (int i = 0; i < count; ++i)
                    {
                        writer.WriteLine($"{i + 1}) {DateTime.FromOADate(timestamps[i])}, {values[i]}, {confidences[i]}");
                    }
                }
                Console.WriteLine($"{DateTime.Now} - End writing to file");
                Console.WriteLine();
                if (count>2000000)
                {
                    Console.WriteLine("Wait 1min due to big data package");
                    Thread.Sleep(15000);
                    Console.WriteLine();
                }
            }
            
        }

    }
}
