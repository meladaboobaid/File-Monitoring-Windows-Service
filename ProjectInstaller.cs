using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FileMonitoringService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            serviceInstaller = new ServiceInstaller
            {
                ServiceName = "MyFileMonitoring",
                DisplayName = "My File Monitoring Service",
                StartType = ServiceStartMode.Automatic,
                Description = "This is my file monitoring service.",
                ServicesDependedOn = new string[] { "RpcSs", "EventLog", "LanmanWorkstation" } // Dependencies
            };

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
