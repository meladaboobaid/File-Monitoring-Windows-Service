# File Monitoring Service

A Windows Service designed to monitor a specific directory for new files in real-time. When a new file is detected, the service automatically renames it using a unique identifier (GUID) and transfers it to a designated destination folder, while maintaining a comprehensive log of all activities.

## Features

- **Real-Time Monitoring**: Utilizes `FileSystemWatcher` to instantly detect new file creations without constant polling.
- **Automated Processing**: Automatically moves detected files to a destination directory, renaming them with a newly generated GUID to prevent naming collisions.
- **Logging**: Keeps a timestamped log of service startup/shutdown, file detection, successful file transfers, and error handling.
- **Console Debugging**: Includes a console mode for easy debugging and testing out of the Windows Services environment.
- **Configurable Paths**: Easily configure source, destination, and log folders via `App.config`. Fallback default directories are created automatically if configuration is omitted.

## How It Works

1. **Initialization**: When the service starts, it reads folder paths from the application configuration. If the paths are not specified, it falls back to defaults (`C:\FileMonitoring\Source`, `C:\FileMonitoring\Destination`, `C:\FileMonitoring\Logs`) and ensures all directories exist.
2. **Monitoring**: A `FileSystemWatcher` is configured to monitor the `SourceFolder`. It hooks into the `Created` event. 
3. **File Transfer**: When a file is dropped into the source directory, the event triggers. The service generates a new mapping (e.g., `[GUID].extension`) and calls `File.Move()` to place the file in the `DestinationFolder`.
4. **Error Handling & Logging**: Operations are wrapped in a `try-catch` block. Every action, including startup, successful moves, and runtime errors, is written to `ServiceLog.txt` in the `LogFolder`.

## Configuration

Folder paths are managed in the `App.config` file. Add or modify the `appSettings` section:

If these keys are missing, the service defaults to `C:\FileMonitoring\...`.

## Running and Testing the Application

### 1. Console Mode (Debugging)
To ease development, the service can be run interactively in the console instead of deploying it as a Windows Service. 
Running the project directly from Visual Studio in Debug mode will trigger the `StartInConsole()` method, allowing you to view live log outputs in the terminal and stop the application by pressing `Enter`.

### 2. Windows Service Deployment
To run as a background service:
1. Build the project in **Release** mode.
2. Open the **Developer Command Prompt** as Administrator.
3. Use the `InstallUtil.exe` tool that comes with the .NET Framework:
4. Start the service through the **Windows Services Manager** (`services.msc`) or via command line:


## Requirements

- .NET Framework 4.8
- Visual Studio
- Windows OS (for Windows Service capabilities)
