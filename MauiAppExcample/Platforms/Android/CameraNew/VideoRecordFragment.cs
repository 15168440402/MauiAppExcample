using Android.App;
using Android.OS;
using Android.Views;
using View = Android.Views.View;
using Button = Android.Widget.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;
using Application = Android.App.Application;

namespace MauiAppExcample.Platforms.Android.CameraNew
{
    public class VideoRecordFragment : Fragment
    {
        private static String TAG = "rustAppVideoFrag";

        private Button mCaptureBtn;
        private CameraPreview mCameraPreview;

        public static VideoRecordFragment newInstance()
        {
            return new VideoRecordFragment();
        }

        public void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public View onCreateView(LayoutInflater inflater, ViewGroup container,Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_backstack, container, false);
        }

        public void onViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            mCaptureBtn = (Button)view.FindViewById(Resource.Id.mtrl_calendar_main_pane);
            //mCaptureBtn.setOnClickListener(mOnClickListener);// 录制键

            mCameraPreview = new CameraPreview(Application.Context);
            FrameLayout preview = (FrameLayout)view.FindViewById(Resource.Id.cache_measures);
            preview.AddView(mCameraPreview);
        }
    }
}
