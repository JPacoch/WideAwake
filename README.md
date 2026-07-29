# ☕ WideAwake

<p align="center">
  <img src="assets/favicon.png" alt="WideAwake Logo" width="512">
</p>

A lightweight Windows utility that prevents your computer from sleeping, locking, or turning off the screen. Inspired by Caffeine, it pings the Windows API and simulates mouse movement in order to stay online with the OS and other apps running in the background.

## How it Works
WideAwake communicates directly with the Windows Kernel using the `SetThreadExecutionState` API. It tells the system that the display and the system are currently required, effectively pausing the idle timer.

## Installation / Usage
1. Download the latest `WideAwake.exe`
2. Double-click the file to run it.
3. Icon should appear in the app tray 

## Building from Source
If you want to build the `.exe` yourself:
1. Install the [.NET 10.0 SDK](https://dotnet.microsoft.com/download).
2. Clone this repo.
3. Run the following command in the project folder:
   ```powershell
   dotnet publish -c Release -r win-x64
   ```
4. The app should be ready under ```bin/Release/net8.0-windows/win-x64/publish/```
