using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace SysMonitor.NET.Resources;

public class NetworkResource : ResourceItemBase<RWData>
{
    private PerformanceCounter[] sent;
    private PerformanceCounter[] received;

    public NetworkResource() : base("Networking", new(ulong.MaxValue, ulong.MaxValue), new(0, 0))
    {
        if (OperatingSystem.IsWindows())
        {
            PerformanceCounterCategory category = new("Network Interface");
            sent = category.GetInstanceNames().Select(i => new PerformanceCounter("Network Interface", "Bytes Sent/sec", i)).ToArray();
            received = category.GetInstanceNames().Select(i => new PerformanceCounter("Network Interface", "Bytes Received/sec", i)).ToArray();
        }
    }

    public override RWData GetApplicationUsage()
    {
        return new(0, 0);
    }

    public override RWData GetSystemUsage()
    {
        RWData data = new(0, 0);

        if (OperatingSystem.IsWindows())
        {
            ulong r = 0;
            ulong s = 0;

            for (int i = 0; i < Math.Min(sent.Length, received.Length); i++)
            {
                s += (ulong)sent[i].NextValue();
                r += (ulong)received[i].NextValue();
            }

            data = new(r, s);
        }
        else if (OperatingSystem.IsLinux())
        {
            string command = "ip -j -s link";
            ProcessStartInfo processInfo = new("bash", $"-c \"{command}\"")
            {
                RedirectStandardOutput = true,
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
            process.WaitForExit();

            if (process.ExitCode != 0 || string.IsNullOrWhiteSpace(content))
            {
                return (RWData)(Result = data);
            }

            ulong r = 0;
            ulong s = 0;

            foreach (JObject json in JArray.Parse(content).Cast<JObject>())
            {
                if (json["flags"] is JArray flags)
                {
                    if (flags.Any(f => f.ToString().Equals("LOOPBACK")))
                    {
                        continue;
                    }
                }
                r += json["stats64"]?["rx"]?["bytes"]?.ToObject<ulong>() ?? 0;
                s += json["stats64"]?["tx"]?["bytes"]?.ToObject<ulong>() ?? 0;
                r += json["stats"]?["rx"]?["bytes"]?.ToObject<ulong>() ?? 0;
                s += json["stats"]?["tx"]?["bytes"]?.ToObject<ulong>() ?? 0;
            }
            data = new(r, s);
        }
        return (RWData)(Result = data);
    }
}