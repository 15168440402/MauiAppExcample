using Android.Content;
using Android.Hardware;
using Android.Icu.Text;
using Android.Media;
using Android.Runtime;
using Android.Views;
using Java.Util;
using Application = Android.App.Application;
using File = Java.IO.File;

namespace MauiAppExcample
{
    public class CameraPreview : SurfaceView, ISurfaceHolderCallback
    {
        public ISurfaceHolder mHolder;
        private Camera mCamera;
        public static CameraPreview Current;

        public static int MEDIA_TYPE_IMAGE = 1;
        public static int MEDIA_TYPE_VIDEO = 2;
        private static int mOptVideoWidth = 1920;  // 默认视频帧宽度
        private static int mOptVideoHeight = 1080;
        private File outputMediaFileUri;
        private String outputMediaFileType;

        public CameraPreview(Context context) :base(context)
        {
            mHolder = this.Holder;
            mHolder.AddCallback(this);
            Current = this;
        }

        private static Camera GetCameraInstance()
        {
            Camera c = null;
            try
            {
                c = Camera.Open();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Debug(MobileBarcodeScanner.TAG, ex.ToString());
            }
            return c;
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            mCamera = GetCameraInstance();
            try
            {
                mCamera.SetPreviewDisplay(holder);
                mCamera.StartPreview();
                GetCameraOptimalVideoSize(); // 找到最合适的分辨率
            }
            catch (IOException e)
            {
                Android.Util.Log.Debug(MobileBarcodeScanner.TAG, "Error setting camera preview: " + e.ToString());
            }
        }

        private void GetCameraOptimalVideoSize()
        {
            try
            {
                Camera.Parameters parameters = mCamera.GetParameters();
                List<Camera.Size> mSupportedPreviewSizes = parameters.SupportedPreviewSizes.ToList();
                List<Camera.Size> mSupportedVideoSizes = parameters.SupportedVideoSizes.ToList();
                //Camera.Size optimalSize = CameraHelper.getOptimalVideoSize(mSupportedVideoSizes,
                //        mSupportedPreviewSizes, getWidth(), getHeight());
                //mOptVideoWidth = optimalSize.width;
                //mOptVideoHeight = optimalSize.height;
                //Log.d(TAG, "prepareVideoRecorder: optimalSize:" + mOptVideoWidth + ", " + mOptVideoHeight);
            }
            catch (Exception e)
            {
                Android.Util.Log.Debug(MobileBarcodeScanner.TAG, "getCameraOptimalVideoSize: ", e);
            }
        }

        public void surfaceDestroyed(ISurfaceHolder holder)
        {
            mHolder.RemoveCallback(this);
            mCamera.SetPreviewCallback(null);
            mCamera.StopPreview();
            mCamera.Release();
            mCamera = null;
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height)
        {
            
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            
        }


        private MediaRecorder mMediaRecorder;

        public bool StartRecording()
        {
            if (PrepareVideoRecorder())
            {
                mMediaRecorder.Start();
                return true;
            }
            else
            {
                ReleaseMediaRecorder();
            }
            return false;
        }

        public void StopRecording()
        {
            if (mMediaRecorder != null)
            {
                mMediaRecorder.Stop();
            }
            ReleaseMediaRecorder();
        }

        public bool isRecording()
        {
            return mMediaRecorder != null;
        }

        private bool PrepareVideoRecorder()
        {
            if (null == mCamera)
            {
                mCamera = GetCameraInstance();
            }
            mMediaRecorder = new MediaRecorder();

            mCamera.Unlock();
            mMediaRecorder.SetCamera(mCamera);

            mMediaRecorder.SetAudioSource(Android.Media.AudioSource.Default);
            mMediaRecorder.SetVideoSource(Android.Media.VideoSource.Camera);

            mMediaRecorder.SetProfile(CamcorderProfile.Get(CamcorderQuality.High));

            mMediaRecorder.SetVideoSize(mOptVideoWidth, mOptVideoHeight);
            mMediaRecorder.SetOutputFile(GetOutputMediaFile(MEDIA_TYPE_VIDEO).ToString());
            mMediaRecorder.SetPreviewDisplay(mHolder.Surface);

            try
            {
                mMediaRecorder.Prepare();
            }
            catch (Exception e)
            {              
                ReleaseMediaRecorder();
                return false;
            }
            return true;
        }

        private void ReleaseMediaRecorder()
        {
            if (mMediaRecorder != null)
            {
                mMediaRecorder.Reset();
                mMediaRecorder.Release();
                mMediaRecorder = null;
                mCamera.Lock() ;
            }
        }

        private File GetOutputMediaFile(int type)
        {
            File mediaStorageDir = new File(FileSystem.CacheDirectory);
            if (!mediaStorageDir.Exists())
            {
                if (!mediaStorageDir.Mkdirs())
                {
                    
                    return null;
                }
            }
            string timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
            File mediaFile;
            if (type == MEDIA_TYPE_IMAGE)
            {
                mediaFile = new File(mediaStorageDir.Path + File.Separator +
                        "IMG_" + timeStamp + ".jpg");
                outputMediaFileType = "image/*";
            }
            else if (type == MEDIA_TYPE_VIDEO)
            {
                mediaFile = new File(mediaStorageDir.Path + File.Separator +
                        "VID_" + timeStamp + ".mp4");
                outputMediaFileType = "video/*";
            }
            else
            {
                return null;
            }
            outputMediaFileUri = mediaFile;
            return mediaFile;
        }
    }
}
