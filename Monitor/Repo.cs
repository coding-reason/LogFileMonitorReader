using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;



namespace Monitor
{
    public class Repo
    {
        public Repo()
        {
            lfiList = new List<LogFileInfo>();
        

        }

        public void start()
        {
            PopulateLogFiles();
            Console.WriteLine($"Repo Started");
            var m = new MonitorThreadController();
            m.InitializeMonitors(lfiList);
            //threadController = new MonitorThreadController(this);
            //threadController.InitializeMonitors(lfiList);

        }
        MonitorThreadController threadController;
        public static List<LogFileInfo> lfiList { get; set; }

        public static void PopulateLogFiles()
        {
            var st = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Repo ThreadID {st}");


                StreamReader sr = new StreamReader(@"config\LogFileNames.txt");
                while (true)
                {
                    var read = sr.ReadLine();
                    if (read == null)
                        break;
                    

                    var lfi = new LogFileInfo { fullName = read };
                    lfiList.Add(lfi);

                }
                sr.Dispose();

        }
    }
}
