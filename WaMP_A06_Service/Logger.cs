using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace WaMP_A06_Service
{
    public static class Logger
    {
        public static void Log(string message)
        {
            EventLog serviceEventLog = new EventLog();
            if (!EventLog.SourceExists("MyEventSource"))
            {
                EventLog.CreateEventSource("MyEventSource", "MyEventLog");
            }
            serviceEventLog.Source = "MyEventSource";
            serviceEventLog.Log = "MyEventLog";
            serviceEventLog.WriteEntry(message);

            
            string pathname = ConfigurationManager.AppSettings.Get("filePath");
            string logmsg = DateTime.Now.ToString() + ":" + message;
            FileStream file;
            StreamWriter sw;
            file = File.Open(pathname, FileMode.Append);
            sw = new StreamWriter(file);
            sw.WriteLine(logmsg);
            sw.Close();
            file.Close();
        }
    }
}
