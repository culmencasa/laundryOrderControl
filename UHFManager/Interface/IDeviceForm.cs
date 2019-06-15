using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UHFManager.POCO;

namespace UHFManager.Interface
{
    public interface IDeviceForm
    {
        void DoWork();

        void StopWork();

        DeviceUsageEnum Function { get; }

    }
}
