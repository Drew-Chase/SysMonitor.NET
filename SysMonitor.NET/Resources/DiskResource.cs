using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace SysMonitor.NET.Resources;

public class DiskResource : ResourceItemBase<RWData>
{
    PerformanceCounter readCounter;
    PerformanceCounter writeCounter;
    public DiskResource() : base("Disk", new(ulong.MaxValue, ulong.MaxValue), new(0, 0))
    {
        if (OperatingSystem.IsWindows())
        {
            readCounter = new("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            writeCounter = new("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
        }
    }

    public override RWData GetApplicationUsage()
    {
        return new(0, 0);
    }

    public override RWData GetSystemUsage()
    {
        string driveLetter = DriveInfo.GetDrives().First(i => i.DriveType.Equals(DriveType.Fixed)).Name;
        RWData data = new(0, 0);

        if (OperatingSystem.IsWindows())
        {
            data = new((ulong)readCounter.NextValue(), (ulong)writeCounter.NextValue());
        }
        else if (OperatingSystem.IsLinux())
        {
            /*
             * IOSTAT - Thirdparty software that needs to be installed!
             * -d - Report the device utilization report
             * -k - Display statistics in kilobytes per second instead of blocks per second.
             * -x 1 - Also makes it only run once?
             * -c 1 - Only runs once
             * -o JSON - Output in JSON format
             */
            string command = $"iostat -d -k -x 1 -c 1 -o JSON";
            ProcessStartInfo processInfo = new("bash", $"-c \"{command}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process process = new() { StartInfo = processInfo };
            string content = "";
            process.OutputDataReceived += (sender, data) =>
            {
                if (!string.IsNullOrWhiteSpace(data.Data))
                {
                    content += data.Data;
                }
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            JObject json = JObject.Parse(content);
            long reads = 0;
            long writes = 0;
            if (json?["sysstat"]?["hosts"] is JArray hosts)
            {
                foreach (JObject host in hosts.Cast<JObject>())
                {
                    if (host?["statistics"] is JArray stats)
                    {
                        foreach (JObject stat in stats.Cast<JObject>())
                        {
                            if (stat?["disk"] is JArray disks)
                            {
                                foreach (JObject disk in disks.Cast<JObject>())
                                {
                                    if (disk?["name"]?.ToString().Equals(driveLetter) ?? false)
                                    {
                                        reads += (long)(disk?["rkB/s"]?.ToObject<double>() ?? 0) * 1024;
                                        writes += (long)(disk?["wkB/s"]?.ToObject<double>() ?? 0) * 1024;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            string command = $"iotop"; // TODO: REPLACE THIS
            ProcessStartInfo processInfo = new("bash", $"-c \"{command}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process process = new() { StartInfo = processInfo };
            process.Start();
        }
        return (RWData)(Result = data);
    }
}
