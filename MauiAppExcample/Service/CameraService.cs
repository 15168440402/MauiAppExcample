using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppExcample.Service
{
    public static partial class CameraService
    {
        public static partial void OpenCamera();
        public static partial Task<string?> TakePhotoAsync();
        public static partial Task<string?> QR();
    }
}
