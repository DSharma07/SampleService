using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace SampleWindowService
{
    public partial class Service1 : ServiceBase
    {       

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            StartProcessing();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        public void StartProcessing()
        {
            ThreadStart starter = new ThreadStart(bw_DoWork);
            Thread t = new Thread(starter);
            t.Start();
        }

        private void bw_DoWork()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo();
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                processStartInfo.WorkingDirectory = @"C:\Users\" + userName.Split('\\')[1];
                processStartInfo.FileName = "cmd.exe";
                var proc = Process.Start(processStartInfo);                
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
            }

        }
    }
}
