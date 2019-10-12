using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using LogFileMonitor.Interfaces;
using Models;
using System.Linq;

namespace Monitor
{
    public class MonitorThreadController
    {
        //public static event EventHandler<FileChanged> fileChanged;

        public MonitorThreadController()
        {
            
            notify = new NotifySurface();
            
        }
        private static NotifySurface notify;
        
        private Repo repo;
        
        public static bool RetainThreading { get; set; }
        public void InitializeMonitors(List<LogFileInfo> list)
        {
            RetainThreading = true;
            int i = 0;
            
            foreach (var l in list)
            {
                Console.WriteLine("starting monitor thread");
                Thread newThread = new Thread(StartMonitor);
                newThread.Start(i);
                i++;
            }
        }

        public static void StartMonitor(object state)
        {
            // object array = state as object;
            int fileId = Convert.ToInt32(state);
            var lfi = Repo.lfiList.First(a => a.index == fileId);
            string logFileName = lfi.fullName;
            long fileLength = lfi.length;

            FileStream fs;
            var st = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"StartMonitor {st}");
            int cnt = 0;

            Console.WriteLine("Monitor started:");
            
            
            fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            while (true)
            {
                    try
                    {
                        if(fs == null)
                        {
                            fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        }
                        fs.Seek(fileLength, 0);
                    }
                    catch
                    {
                        fs = null;
                        continue;
                    }
                        
                    
                    if (fileLength != fs.Length)
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            bool hasData = true;
                            var lines = new List<string>();
                            while (hasData)
                            {
                                var line = sr.ReadLine();
                                if (line == null)
                                {
                                    hasData = false;
                                    break;
                                }
                                else
                                {
                                    lines.Add(line);
                                }
                            }
                            cnt++;
                            Console.WriteLine($"notifying change: {cnt}");
                            notify.Notify(fileId, lines.ToArray());
                            fileLength = fs.Length;
                        }
                    }

                Thread.Sleep(200);
            }
            
        }
    }
}


