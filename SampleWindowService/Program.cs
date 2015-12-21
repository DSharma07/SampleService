using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace SampleWindowService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Set args.length to 0 to debug the service.
            if (args.Length > 0)
            {
                var service = new Service1();
                service.StartProcessing();
                Console.ReadLine();

            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Service1(), };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
