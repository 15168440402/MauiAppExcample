using Android.Bluetooth;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppExcample.Service
{
    public static partial class BluetoothService
    {
        static List<BluetoothDevice> _pairedDevices;
        static string _uuid=Guid.NewGuid().ToString();

        public static partial void Open()
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter != null)
            {
                if (!mBluetoothAdapter.IsEnabled)
                {
                    Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                    MainActivity.Current.StartActivityForResult(enableIntent, 1);
                }               
            }
        }

        public static partial List<string>? GetDevices()
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            _pairedDevices = mBluetoothAdapter.BondedDevices.ToList();
            return _pairedDevices.Select(p => p.Name).ToList();
        }

        public static partial string Send(string name)
        {
            string? output;
            var device = _pairedDevices.First(p => p.Name == name);
            ParcelUuid uuid = device.GetUuids().ElementAt(0);
            var sock = device.CreateInsecureRfcommSocketToServiceRecord(uuid.Uuid);
            sock.Connect();
            Thread t = new Thread(Connected);
            t.Start(sock);

            //ParcelUuid uuid = device.GetUuids().ElementAt(0);
            //var mmsSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid.Uuid);
            //mmsSocket.Connect();
            if (sock.IsConnected)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes("Sample Text");
                sock.OutputStream.Write(byteArray, 0, byteArray.Length);
                output = $"蓝牙： {name} Connected Successfully。";
            }
            else
            {
                output = $"蓝牙： {name}  Connected Errorr";
                return output;
            }
            //using var datastream = sock.OutputStream;

            //byte[] byteArray = Encoding.ASCII.GetBytes("Sample Text");

            //datastream.Write(byteArray, 0, byteArray.Length);

            return $"{output}发送测试数据：Sample Text 成功！";
        }

        public static partial void Listen(ParameterizedThreadStart connected)
        {
            Task.Run(() =>
            {
                BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                BluetoothServerSocket serverSock = mBluetoothAdapter.ListenUsingRfcommWithServiceRecord("Bluetooth", Java.Util.UUID.FromString(_uuid));
                BluetoothSocket sock = serverSock.Accept();
                //serverSock.Close();//服务器获得连接后腰及时关闭ServerSocket
                                   //启动新的线程，开始数据传输
                Thread t = new Thread(connected);
                t.Start(sock);
            });
        }

        public static void Connected(object obj)
        {
            if (obj is BluetoothSocket sock)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes("Sample Text");
                sock.OutputStream.Write(byteArray, 0, byteArray.Length);
            }
        }
    }
}
