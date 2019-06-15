using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.POCO
{
    [Flags]
    public enum Zone
    {
        None = 0,
        RESERVE = 1,
        EPC = 2,
        TID = 4,
        USER = 8
    }
}
