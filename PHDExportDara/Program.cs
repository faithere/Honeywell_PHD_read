using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uniformance.PHD;

namespace PHDExportData
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                PHDHistorian phd = new PHDHistorian();
                phd.DefaultServer = new PHDServer("162.20.0.26", SERVERVERSION.RAPI200);
                phd.Sampletype = SAMPLETYPE.Raw;
                phd.ReductionType = REDUCTIONTYPE.None;

                Double[] timestamps = null;
                Double[] values = null;
                short[] confidences = null;
                Console.WriteLine("Path of tag list:");
                var path = Console.ReadLine();
                Console.WriteLine("Path of output:");
                var pathOutput = Console.ReadLine();

                var tagList = new List<string>();

                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        var lines = reader.ReadLine();
                        tagList.Add(lines);
                    }
                }

                Console.WriteLine("Start time (ralative time):");
                phd.StartTime = Console.ReadLine();
                Console.WriteLine("End time (ralative time):");
                phd.EndTime = Console.ReadLine();
                Console.WriteLine("Enter log file name:");
                var logWrite = File.CreateText(Console.ReadLine());
                int milliseconds = 60000;
                using (logWrite)
                {
                    foreach (string tag in tagList)
                    {
                    Tag MyTag = new Tag(tag);

                        try
                        {
                           PHDRead.Read(tag, MyTag, phd, timestamps, values, confidences, pathOutput);
                           phd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                            logWrite.WriteLine($"{DateTime.Now} - {tag} - {ex.Message}");

                            if (!ex.Message.ToString().Contains("10012") && !ex.Message.ToString().Contains("10006") && !ex.Message.ToString().Contains("10068") && !ex.Message.ToString().Contains("10060"))
                            {
                                while (true)
                                {
                                    phd.Dispose();
                                    Thread.Sleep(milliseconds);
                                    try
                                    {
                                        PHDRead.Read(tag, MyTag, phd, timestamps, values, confidences, pathOutput);
                                        break;
                                    }
                                    catch (Exception ex1)
                                    {
                                        Console.WriteLine($"{DateTime.Now}: {ex1.Message}");
                                        logWrite.WriteLine($"{DateTime.Now} - {tag} - {ex1.Message}");
                                    }
                                }
                            }
                        }
                        



                    }
                }
            }
        }
    }
}






       