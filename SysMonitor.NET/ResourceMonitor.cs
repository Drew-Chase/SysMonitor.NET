using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SysMonitor.NET.Resources;

namespace SysMonitor.NET;

public class ResourceMonitor
{
    public static ResourceMonitor Default { get; } = new(TimeSpan.FromSeconds(1), EResourceType.CPU | EResourceType.RAM | EResourceType.DISK | EResourceType.NETWORK);
    public delegate void ResourceMonitorUpdateHandler(ResourceMonitor sender, ResourceResult data);
    public event ResourceMonitorUpdateHandler? OnUpdate;

    private CPUResource? CPU = null;
    private readonly TimeSpan updateFrequency;
    private bool isRunning = false;
    public ResourceMonitor(TimeSpan updateFrequency, EResourceType flag)
    {
        this.updateFrequency = updateFrequency;

        if (flag.HasFlag(EResourceType.CPU))
            CPU = new();
        if (flag.HasFlag(EResourceType.RAM))
        {

        }
        if (flag.HasFlag(EResourceType.DISK))
        {

        }
        if (flag.HasFlag(EResourceType.NETWORK))
        {

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
    private void Run()
    {
        while (isRunning)
        {

            OnUpdate?.Invoke(this, GetResult());
            Thread.Sleep(updateFrequency);
        }
    }

    public ResourceResult GetResult()
    {
        return new()
        {
            CPU = new()
            {
                System = CPU?.GetSystemUsage() ?? 0,
                Application = CPU?.GetApplicationUsage() ?? 0,
                Min = CPU?.MinValue ?? 0,
                Max = CPU?.MaxValue ?? 0
            }
        };
    }

    public JObject ToJson()
    {
        return JObject.FromObject(GetResult());
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(ToJson());
    }
}
