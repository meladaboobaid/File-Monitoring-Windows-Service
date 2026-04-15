using System;
using System.IO;
using System.ServiceProcess;
using System.Configuration;

namespace FileMonitoringService
{
    public partial class MyFileMonitoring : ServiceBase
    {

        private FileSystemWatcher fileWatcher;
        private string sourceFolder;
        private string destinationFolder;
        private string logFolder;
       
        public MyFileMonitoring()
        {
            InitializeComponent();

            
            sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
            logFolder = ConfigurationManager.AppSettings["LogFolder"];

            if (string.IsNullOrWhiteSpace(sourceFolder))
            {
                sourceFolder = @"C:\FileMonitoring\Source"; // Default source folder
                Log("SourceFolder is missing in App.config. Using default: " + sourceFolder);
            }

            if (string.IsNullOrWhiteSpace(destinationFolder))
            {
                destinationFolder = @"C:\FileMonitoring\Destination"; // Default destination folder
                Log ("DestinationFolder is missing in App.config. Using default: " + destinationFolder);
            }

            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = @"C:\FileMonitoring\Logs"; // Default log folder
                Log("LogFolder is missing in App.config. Using default: " + logFolder);
            }

            // Ensure directories exist
            Directory.CreateDirectory(sourceFolder);
            Directory.CreateDirectory(destinationFolder);
            Directory.CreateDirectory(logFolder);
        }

 
        protected override void OnStart(string[] args)
        {
            Log("Service Started...");

            fileWatcher = new FileSystemWatcher
            {
                Path = sourceFolder,
                Filter = "*.*",
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };

            fileWatcher.Created += OnFileCreated;

            Log("File monitoring started on folder: " + sourceFolder);
        }

        protected override void OnStop()
        {
            fileWatcher.EnableRaisingEvents = false;
            fileWatcher.Dispose();
            Log("Service Stopped...");

        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                Log($"File detected: {e.FullPath}");

                string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(e.Name)}";
                string destinationFile = Path.Combine(destinationFolder, newFileName);

                File.Move(e.FullPath, destinationFile);

                Log($"File moved: {e.FullPath} -> {destinationFile}");
            }
            catch (Exception ex)
            {
                Log($"Error processing file: {e.FullPath}. Exception: {ex.Message}");
            }
        }

        private void Log(string message)
        {
            string logFilePath = Path.Combine(logFolder, "ServiceLog.txt");
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n";

            File.AppendAllText(logFilePath, logMessage);

            // Enable console mode for debugging when running interactively
            if (Environment.UserInteractive)
            {
                Console.WriteLine(logMessage);
            }
        }

        public void StartInConsole()
        {
            OnStart(null);
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine();
            OnStop();
        }

    }
}
