using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppExcample.Service
{
	public static partial class BluetoothService
	{
		public static partial void Open();
		public static partial List<string> GetDevices();
		public static partial bool Connected(string deviceName);
		public static partial bool Send(string deviceName, string message);
		public static partial void Listen(string deviceName,ParameterizedThreadStart connected);
	}
}
