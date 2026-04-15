using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitoringService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                // Run as a console application
                MyFileMonitoring service = new MyFileMonitoring();
                service.StartInConsole();

            }
            else
            {
                // Run as a Windows Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MyFileMonitoring()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
