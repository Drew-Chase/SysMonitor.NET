using Newtonsoft.Json.Linq;
using SysMonitor.NET.Resources;

namespace SysMonitor.NET;

public class ResourceMonitor
{
    public static ResourceMonitor Default { get; } = new(TimeSpan.FromSeconds(1), EResourceType.CPU | EResourceType.RAM | EResourceType.DISK | EResourceType.NETWORK);

    public delegate void ResourceMonitorUpdateHandler(ResourceMonitor sender, ResourceResult data);

    public event ResourceMonitorUpdateHandler? OnUpdate;

    private readonly CPUResource? CPU = null;
    private readonly RAMResource? RAM = null;
    private readonly NetworkResource? NETWORKING = null;
    private readonly DiskResource? DISK = null;
    private readonly TimeSpan updateFrequency;
    private bool isRunning = false;

    public ResourceMonitor(TimeSpan updateFrequency, EResourceType flag)
    {
        this.updateFrequency = updateFrequency;
        if (flag.HasFlag(EResourceType.CPU))
        {
            CPU = new();
        }
        if (flag.HasFlag(EResourceType.RAM))
        {
            RAM = new();
        }
        if (flag.HasFlag(EResourceType.DISK))
        {
            DISK = new();
        }
        if (flag.HasFlag(EResourceType.NETWORK))
        {
            NETWORKING = new();
        }
    }

    public void Start()
    {
        isRunning = true;
        Run();
    }

    public void Stop()
    {
        isRunning = false;
    }

    private async Task Run()
    {
        while (isRunning)
        {
            Task.WaitAll(CPU?.UpdateAsync() ?? Task.CompletedTask, RAM?.UpdateAsync() ?? Task.CompletedTask, DISK?.UpdateAsync() ?? Task.CompletedTask, NETWORKING?.UpdateAsync() ?? Task.CompletedTask);
            OnUpdate?.Invoke(this, GetResult());
            await Task.Delay(updateFrequency);
        }
    }

    private ResourceResult GetResult()
    {
        return new()
        {
            CPU = (CPUResult?)CPU?.Result,
            RAM = (RAMResult?)RAM?.Result,
            Disk = (RWData?)DISK?.Result,
            Networking = (RWData?)NETWORKING?.Result,
        };
    }

    public JObject ToJson()
    {
        return GetResult().ToJson();
    }

    public override string ToString()
    {
        return GetResult().ToString();
    }
}