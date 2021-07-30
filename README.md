# Notificator :bell:

### Description
Notificator is a small program that makes requests to a specific URL in the background, receives a JSON message and displays it in Windows Notification Center for a specific user.

### Launching project
This program was developed in `Visual Studio 2017`. The following NuGet packages have also been added:
 - `Newtonsoft.Json` v13.0.1;
 - `System.Runtime` v4.0.0;
 - `System.Runtime.WindowsRuntime` v4.0.0;
 - `Legacy2CPSWorkaround` v1.0.0;
 - `Microsoft.Toolkit.Uwp.Notifications` v7.0.2.

### Configuration
The program has three configurable parameters, which are located in the `App.config` file in `bin/Release` folder:
 - `username` - Username of the user who owns the notifications;
 - `url` - URL, which returns a JSON message;
 - `interval` - Request interval in seconds.
 