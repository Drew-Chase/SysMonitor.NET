namespace SysMonitor.NET;

[Flags]
public enum EResourceType
{
    CPU =  0b_00010,
    RAM =  0b_00100,
    DISK = 0b_01000,
    NETWORK = 0b_10000,
}
