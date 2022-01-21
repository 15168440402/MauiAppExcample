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
		public static partial List<string>? GetDevices();
		public static partial string Send(string name);
		public static partial void Listen(ParameterizedThreadStart connected);
	}
}
