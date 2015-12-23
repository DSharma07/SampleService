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
            String applicationName = "cmd.exe";

            
            ApplicationLoader.PROCESS_INFORMATION procInfo;
            ApplicationLoader.StartProcess(applicationName, out procInfo);
        }

        protected override void OnStop()
        {
        }
    }
}
