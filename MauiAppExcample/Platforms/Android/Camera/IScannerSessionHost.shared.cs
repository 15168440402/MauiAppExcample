using System;
namespace MauiAppExcample
{
	public interface IScannerSessionHost
	{
		MobileBarcodeScanningOptions ScanningOptions { get; }
	}
}
