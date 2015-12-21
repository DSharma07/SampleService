using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SampleWindowService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        
        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceInstaller1.ServiceName, Environment.MachineName))
                {
                    if (sc.Status != ServiceControllerStatus.Running)
                    {
                        sc.Start();
                    }
                }
            }
            catch (Exception ee)
            {
                EventLog.WriteEntry("Application", ee.ToString(), EventLogEntryType.Error);
            }
        }

        private void serviceInstaller1_Committed(object sender, InstallEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceInstaller1.ServiceName))
                {                  

                    var coOptions = new ConnectionOptions();
                    coOptions.Impersonation = ImpersonationLevel.Impersonate;
                    var mgmtScope = new ManagementScope(@"root\CIMV2", coOptions);
                    mgmtScope.Connect();
                    ManagementObject wmiService;
                    wmiService = new ManagementObject("Win32_Service.Name='" + serviceInstaller1.ServiceName + "'");
                    ManagementBaseObject InParam = wmiService.GetMethodParameters("Change");
                    InParam["DesktopInteract"] = true;
                    ManagementBaseObject OutParam = wmiService.InvokeMethod("Change", InParam, null);

                    SetRecoveryOptions(sc.ServiceName);
                }
            }
            catch (Exception e1)
            {
                EventLog.WriteEntry("Application", e1.ToString(), EventLogEntryType.Error);
                return;
            }
        }
        

        static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                startInfo.WorkingDirectory = @"C:\Users\" + userName.Split('\\')[1];
                startInfo.FileName = "cmd.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Normal;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/60000", serviceName);

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
                throw new InvalidOperationException();
        }
    }
}
