﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Webkit;
using Android.Widget;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace MauiAppExcample;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
	public static MainActivity Current { get; set; }

	protected override void OnCreate(Bundle savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		Platform.Init(this, savedInstanceState);
		Current = this;
	
		LinearLayout relativeLayout = FindViewById<LinearLayout>(Resource.Id.ll_controller);
		CameraPreview cameraPreview = new CameraPreview(this);
		var a = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
		cameraPreview.LayoutParameters = a;
		relativeLayout.AddView(cameraPreview);
	}

	public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
	{
		Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

		base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
	}
}
