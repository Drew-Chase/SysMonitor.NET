using Newtonsoft.Json;

namespace SysMonitor.NET.Resources;

public struct ResourceResult
{
    public CPUResult CPU { get; set; }
    public RAMResult RAM { get; set; }
    public DiskResult Disk { get; set; }
    public NetworkingResult Networking { get; set; }

    public override readonly string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
public struct CPUResult
{
    public double System { get; set; }
    public double Application { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}

public struct RAMResult
{
    public double System { get; set; }
    public double Application { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}

public struct DiskResult
{
    public double System { get; set; }
    public double Application { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}

public struct NetworkingResult
{
    public double System { get; set; }
    public double Application { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}
