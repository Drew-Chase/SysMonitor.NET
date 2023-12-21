namespace SysMonitor.NET.Resources;

public class NetworkResource : ResourceItemBase<RWData>
{
    public NetworkResource() : base("Networking", new(ulong.MaxValue, ulong.MaxValue), new(0, 0))
    {
    }

    public override RWData GetApplicationUsage()
    {
        throw new NotImplementedException();
    }

    public override RWData GetSystemUsage()
    {
        throw new NotImplementedException();
    }
}
