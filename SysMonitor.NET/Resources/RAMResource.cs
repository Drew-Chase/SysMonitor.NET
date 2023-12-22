using Hardware.Info;
using System.Diagnostics;

namespace SysMonitor.NET.Resources;

internal class RAMResource : ResourceItemBase<ulong>
{
    private readonly IHardwareInfo hardwareInfo = new HardwareInfo();

    public RAMResource() : base("RAM", 0, 0)
    {
        hardwareInfo.RefreshMemoryList();
        hardwareInfo.MemoryList.ForEach(memory =>
        {
            MaxValue += memory.Capacity;
        });
        Result = new RAMResult() { Max = MaxValue, Min = MinValue };
    }

    public override ulong GetApplicationUsage()
    {
        ulong value = (ulong)Process.GetCurrentProcess().WorkingSet64;
        ((RAMResult)Result).Application = value;
        return value;
    }

    public override ulong GetSystemUsage()
    {
        ulong value = (ulong)Process.GetProcesses().Sum(p => p.WorkingSet64);
        ((RAMResult)Result).System = value;
        return value;
    }
}