# SysMonitor.NET Library Documentation

SysMonitor.NET is a .NET library that provides a simple and convenient way to monitor system resources such as CPU, RAM, Disk, and Networking. It allows developers to integrate real-time monitoring capabilities into their applications. This documentation will guide you through the process of using the SysMonitor.NET library in your C# application.

## Prerequisites

Make sure you have the following prerequisites before using SysMonitor.NET:

- .NET Core or .NET Framework installed.
- The SysMonitor.NET library added as a reference to your project.

## Getting Started

### Installation

1. Install the SysMonitor.NET library using NuGet Package Manager:

   ```bash
   Install-Package SysMonitor.NET
   ```

   Or using .NET CLI:

   ```bash
   dotnet add package SysMonitor.NET
   ```

### Usage

1. Import the required namespaces in your C# code:

   ```csharp
   using SysMonitor.NET;
   ```

2. Create a `ResourceMonitor` instance:

   ```csharp
   ResourceMonitor monitor = ResourceMonitor.Default;
   // or
   TimeSpan updateFrequency = TimeSpan.FromSeconds(1);
   EResourceType monitorFlags = EResourceType.CPU | EResourceType.RAM | EResourceType.DISK | EResourceType.NETWORK;
   ResourceMonitor monitor = new ResourceMonitor(updateFrequency, monitorFlags);
   ```

3. Subscribe to the `OnUpdate` event to receive real-time resource data:

   ```csharp
   monitor.OnUpdate += (sender, data) =>
   {
       // Handle the resource data updates here
   };
   ```

4. Inside the event handler, update your application UI or log the resource data as needed. The provided example clears the console and prints CPU, RAM, Disk, and Networking information.

   ```csharp
   if (data.CPU != null)
   {
       // Print CPU information
   }

   if (data.RAM != null)
   {
       // Print RAM information
   }

   if (data.Disk != null)
   {
       // Print Disk information
   }

   if (data.Networking != null)
   {
       // Print Networking information
   }
   ```

5. Start the monitor to begin collecting and updating resource data:

   ```csharp
   monitor.Start();
   ```

6. To stop monitoring and release resources, call the `Stop` method:

   ```csharp
   monitor.Stop();
   ```

7. Add any additional logic as needed for your application.

## Resource Data Options

The SysMonitor.NET library provides the following data options to monitor various system resources:

### 1. `ResourceResult` Structure

The `ResourceResult` structure aggregates the results of CPU, RAM, Disk, and Networking monitoring. It includes the following properties:

- `CPU`: A nullable property of type `CPUResult` that contains CPU utilization information.
- `RAM`: A nullable property of type `RAMResult` that contains RAM utilization information.
- `Disk`: A nullable property of type `RWData` that contains Disk read and write information.
- `Networking`: A nullable property of type `RWData` that contains Networking read and write information.

#### Methods:

- `ToJson()`: Converts the `ResourceResult` structure to a `JObject` for easy serialization to JSON.
- `ToString()`: Converts the `ResourceResult` structure to a JSON-formatted string.

### 2. `CPUResult` Class

The `CPUResult` class represents CPU utilization information. It includes the following properties:

- `System`: The percentage of CPU utilization by the system.
- `Application`: The percentage of CPU utilization by the application.
- `Min`: The minimum CPU utilization.
- `Max`: The maximum CPU utilization.

### 3. `RAMResult` Class

The `RAMResult` class represents RAM utilization information. It includes the following properties:

- `System`: The amount of RAM used by the system.
- `Application`: The amount of RAM used by the application.
- `Min`: The minimum RAM usage.
- `Max`: The maximum RAM usage.

### 4. `RWData` Structure

The `RWData` structure represents read and write data for Disk and Networking. It includes the following properties:

- `Read`: The amount of data read.
- `Write`: The amount of data written.


### Example

Here's a complete example that demonstrates the usage of the SysMonitor.NET library:

```csharp
// Import necessary namespaces
using Chase.CommonLib.Math;
using SysMonitor.NET;
using SysMonitor.NET.Resources;

namespace Example
{
    internal class Program
    {
        private static void Main()
        {
            // Create a ResourceMonitor instance
            ResourceMonitor monitor = ResourceMonitor.Default;

            // Subscribe to the OnUpdate event
            monitor.OnUpdate += (sender, data) =>
            {
                // Access resource data using the data options
                Console.Clear();

            if (data.CPU != null)
            {
                CPUResult cpuInfo = data.CPU;
                Console.WriteLine($"CPU: \n" +
                                  $"\tSystem - {cpuInfo.System:P2}\n" +
                                  $"\tApplication - {cpuInfo.Application:P2}");
            }

            if (data.RAM != null)
            {
                RAMResult ramInfo = data.RAM;
                double systemRamUsage = ramInfo.System / (double)(ramInfo.Max);
                double appRamUsage = ramInfo.Application / (double)(ramInfo.Max);

                Console.WriteLine($"RAM: \n" +
                                  $"\tSystem - {AdvancedFileInfo.SizeToString((long)(ramInfo.System))} / " +
                                  $"{AdvancedFileInfo.SizeToString((long)(ramInfo.Max))} ({systemRamUsage:P2})\n" +
                                  $"\tApplication - {AdvancedFileInfo.SizeToString((long)(ramInfo.Application))} / " +
                                  $"{AdvancedFileInfo.SizeToString((long)(ramInfo.Max))} ({appRamUsage:P2})");
            }

            if (data.Disk != null)
            {
                RWData? diskInfo = data.Disk;
                Console.WriteLine($"Disk: \n" +
                                  $"\tSystem - r:{AdvancedFileInfo.SizeToString((long)(diskInfo?.Read ?? 0))}/s " +
                                  $"w:{AdvancedFileInfo.SizeToString((long)(diskInfo?.Write ?? 0))}/s");
            }

            if (data.Networking != null)
            {
                RWData? networkInfo = data.Networking;
                Console.WriteLine($"Networking: \n" +
                                  $"\tSystem - r:{AdvancedFileInfo.SizeToString((long)(networkInfo?.Read ?? 0))}/s " +
                                  $"w:{AdvancedFileInfo.SizeToString((long)(networkInfo?.Write ?? 0))}/s");
            }
            };

            // Start monitoring
            monitor.Start();

            // Wait for user input to end the program
            Console.ReadLine();

            // Stop monitoring and release resources
            monitor.Stop();
        }
    }
}

```