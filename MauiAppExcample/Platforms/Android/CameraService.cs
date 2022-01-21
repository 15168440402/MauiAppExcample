using Android.Content;
using Android.Runtime;
using Android.Hardware.Camera2;
using Android.Content.PM;
using Android;
using AndroidX.Core.App;
using Android.Provider;

namespace MauiAppExcample.Service
{
    public static partial class CameraService 
    {

        public static Context Context { get; set; }

        static CameraService()
        {
            Context = Android.App.Application.Context;
        }

        /// <summary>
        ///  判断是否具备拍照功能
        /// </summary>
        /// <returns></returns>
        private static bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        /// <summary>
        /// 检测权限
        /// </summary>
        /// <returns></returns>
        private static bool HasPermission(Android.App.Activity activity)
        {
            if (ActivityCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) != Permission.Granted && ActivityCompat.CheckSelfPermission(Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                //MainActivity.Current.RequestPermissions(new string[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, 200);
                ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, 200);
                return false;
            }
            return true;
        }

        public static partial void OpenCamera()
        {
            if (IsThereAnAppToTakePictures() == false) return;
            if (HasPermission(MainActivity.Current) == false) return;

            var cameraManager = Android.App.Application.Context.GetSystemService(Context.CameraService).JavaCast<CameraManager>();
            var cameraId = cameraManager.GetCameraIdList()[0];
            cameraManager.OpenCamera(cameraId, new CameraStateCallback(), null);
        }

        public static partial async Task<string?> TakePhotoAsync()
        {
            if (IsThereAnAppToTakePictures() == false) return null;
            if (HasPermission(MainActivity.Current) == false) return null; 
            var photo = await MediaPicker.CapturePhotoAsync();
            return await LoadPhotoAsync(photo);
        }

        public static partial async Task<string?> QR()
		{
            //if (IsThereAnAppToTakePictures() == false) return null;
            //if (HasPermission(MainActivity.Current) == false) return null;
            //var photo = await MediaPicker.CapturePhotoAsync();
            //if (photo == null) return null;
            //using var stream = await photo.OpenReadAsync();
            //string decodedString = new QRCodeDecoder().decode(new ThoughtWorks.QRCode.Codec.Data.QRCodeBitmapImage(new Bitmap("")), Encoding.UTF8);
            var scanner = new MobileBarcodeScanner();
            var result = await scanner.Scan();
            return result.Text;
            //return "";
        }

        static async Task<string?> LoadPhotoAsync(FileResult photo)
        {
            if (photo == null) return null;
            // save the file into local storage
            var newFile = System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using var stream = await photo.OpenReadAsync();
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);

            return newFile;
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
