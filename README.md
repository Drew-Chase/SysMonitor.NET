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
   using Chase.CommonLib.Math;
   using SysMonitor.NET;
   ```

2. Create a `ResourceMonitor` instance:

   ```csharp
   ResourceMonitor monitor = ResourceMonitor.Default;
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
   Console.Clear();

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

### Example

Here's a complete example that demonstrates the usage of the SysMonitor.NET library:

```csharp
// Include necessary namespaces
using Chase.CommonLib.Math;
using SysMonitor.NET;

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
                // Update UI or log resource data
                Console.Clear();

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

## Conclusion

With SysMonitor.NET, you can easily integrate system resource monitoring into your C# applications, providing valuable insights into the performance of the underlying hardware. Customize the example code to suit your specific use case and enhance the monitoring capabilities of your applications.