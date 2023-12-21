using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMonitor.NET;

[Flags]
public enum EResourceType
{
    NONE = 0,
    CPU = 1,
    RAM = 2,
    DISK = 3,
    NETWORK = 8,
}
