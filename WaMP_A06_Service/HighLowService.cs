using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WaMP_A06_Service;
using System.Threading;


namespace WaMP_A06_Service
{
    public partial class HighLowService : ServiceBase
    {
        private Thread t = null;
        Listener listener = null;
        
        public HighLowService()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            
            listener = new Listener();
            t = new Thread(new ThreadStart(listener.Listen));
            t.Start();
            Logger.Log("Service Started");
        }

        protected override void OnStop()
        {
            listener.Run = false;
            t.Join();
            t = null;
            Logger.Log("HighLow Service Stopped");
        }
    }
}
