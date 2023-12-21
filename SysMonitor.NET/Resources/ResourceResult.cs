using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SysMonitor.NET.Resources;

public struct ResourceResult()
{
    public CPUResult? CPU { get; set; } = null;
    public RAMResult? RAM { get; set; } = null;
    public DiskResult? Disk { get; set; } = null;
    public NetworkingResult? Networking { get; set; } = null;

    public readonly JObject ToJson()
    {
        return JObject.FromObject(this);
    }

    public override readonly string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
public sealed class CPUResult : IResourceResult
{
    public double System { get; set; }
    public double Application { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}

public sealed class RAMResult : IResourceResult
{
    public ulong System { get; set; }
    public ulong Application { get; set; }
    public ulong Min { get; set; }
    public ulong Max { get; set; }
}

public sealed class DiskResult : IResourceResult
{
    public double System { get; set; }
    public double Application { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}

public sealed class NetworkingResult : IResourceResult
{
    public double System { get; set; }
    public double Application { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}
public interface IResourceResult { }