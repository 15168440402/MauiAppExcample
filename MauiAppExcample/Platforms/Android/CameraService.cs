using Android.Content;
using Android.Runtime;
using Android.Hardware.Camera2;
using Android.Content.PM;
using Android;
using AndroidX.Core.App;
using Android.Provider;

namespace MauiAppExcample.Service
{
    public partial class CameraService 
    {
        Context Context { get; set; }

        public CameraService()
        {
            Context = Android.App.Application.Context;
        }

        /// <summary>
        ///  判断是否具备拍照功能
        /// </summary>
        /// <returns></returns>
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        /// <summary>
        /// 检测权限
        /// </summary>
        /// <returns></returns>
        private bool HasPermission()
        {
            if (ActivityCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) != Permission.Granted && ActivityCompat.CheckSelfPermission(Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                MainActivity.Current.RequestPermissions(new string[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, 200);
                //ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, 200);
                return false;
            }
            return true;
        }

        public partial void OpenCamera()
        {
            if (IsThereAnAppToTakePictures() == false) return;
            if (HasPermission() == false) return;

            var cameraManager = Android.App.Application.Context.GetSystemService(Context.CameraService).JavaCast<CameraManager>();
            var cameraId = cameraManager.GetCameraIdList()[0];
            cameraManager.OpenCamera(cameraId, new CameraStateCallback(), null);
        }

        public class CameraStateCallback : CameraDevice.StateCallback
        {
            public override void OnDisconnected(CameraDevice camera)
            {

            }

            public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
            {

            }

            public override void OnOpened(CameraDevice camera)
            {

            }
        }
    }
}
