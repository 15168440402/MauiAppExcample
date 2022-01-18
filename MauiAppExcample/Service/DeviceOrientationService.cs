using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppExcample.Service
{
    public enum DeviceOrientation
    {
        Undefined,
        Landscape,
        Portrait
    }

    public partial class DeviceOrientationService
    {
        public partial DeviceOrientation GetOrientation();
    }
}
