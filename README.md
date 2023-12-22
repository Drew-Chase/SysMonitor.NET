# SysMonitor.NET Library Documentation

SysMonitor.NET is a powerful .NET library designed to seamlessly integrate real-time system resource monitoring capabilities into your C# applications. This comprehensive documentation will guide you through the installation, setup, and utilization of the SysMonitor.NET library.

## Prerequisites

Before you begin, ensure that you have the following prerequisites in place:

- **.NET Core or .NET Framework**: Make sure that you have either .NET Core or .NET Framework installed on your system.
- **SysMonitor.NET Library**: Add the SysMonitor.NET library as a reference to your project.

## Getting Started

### Installation

To install the SysMonitor.NET library, use either the NuGet Package Manager or the .NET CLI:

```bash
# NuGet Package Manager
Install-Package SysMonitor.NET

# .NET CLI
dotnet add package SysMonitor.NET
```

### Usage

1. Import the necessary namespaces in your C# code:

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
       // Access CPU information
   }

   if (data.RAM != null)
   {
       // Access RAM information
   }

   if (data.Disk != null)
   {
       // Access Disk information
   }

   if (data.Networking != null)
   {
       // Access Networking information
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

7. Implement any additional logic as needed for your application.

## Resource Data Options

The SysMonitor.NET library provides the following data options to monitor various system resources:

### 1. `ResourceResult` Structure

The `ResourceResult` structure aggregates CPU, RAM, Disk, and Networking monitoring results. It includes the following properties:

- `CPU`: CPU utilization information of type `CPUResult`.
- `RAM`: RAM utilization information of type `RAMResult`.
- `Disk`: Disk read and write information of type `RWData`.
- `Networking`: Networking read and write information of type `RWData`.

#### Methods:

- `ToJson()`: Converts the `ResourceResult` structure to a `JObject` for easy serialization to JSON.
- `ToString()`: Converts the `ResourceResult` structure to a JSON-formatted string.

### 2. `CPUResult` Class

The `CPUResult` class represents CPU utilization information with properties:

- `System`: Percentage of CPU utilization by the system.
- `Application`: Percentage of CPU utilization by the application.
- `Min`: Minimum CPU utilization.
- `Max`: Maximum CPU utilization.

### 3. `RAMResult` Class

The `RAMResult` class represents RAM utilization information with properties:

- `System`: Amount of RAM used by the system.
- `Application`: Amount of RAM used by the application.
- `Min`: Minimum RAM usage.
- `Max`: Maximum RAM usage.

### 4. `RWData` Structure

The `RWData` structure represents read and write data for Disk and Networking with properties:

- `Read`: Amount of data read.
- `Write`: Amount of data written.

### Example

Here's a detailed example showcasing the usage of the SysMonitor.NET library. View the full example project [here](/Example/Program.cs) for a complete implementation.

```csharp
// Import necessary namespaces
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
                    double systemRamUsage = ramInfo.System / (double)ramInfo.Max;
                    double appRamUsage = ramInfo.Application / (double)ramInfo.Max;

                    Console.WriteLine($"RAM: \n" +
                                      $"\tSystem - {ramInfo.System} / {ramInfo.Max} ({systemRamUsage:P2})\n" +
                                      $"\tApplication - {ramInfo.Application} / {ramInfo.Max} ({appRamUsage:P2})");
                }

                if (data.Disk != null)
                {
                    RWData? diskInfo = data.Disk;
                    Console.WriteLine($"Disk: \n" +
                                      $"\tSystem - r:{diskInfo?.Read}/s w:{diskInfo?.Write}/s");
                }

                if (data.Networking != null)
                {
                    RWData? networkInfo = data.Networking;
                    Console.WriteLine($"Networking: \n" +
                                      $"\tSystem - r:{networkInfo?.Read}/s w:{networkInfo?.Write}/s");
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

Feel free to tailor the example to suit your specific application requirements. The SysMonitor.NET library offers flexibility and extensibility for monitoring various system resources with ease.