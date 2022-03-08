using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppExcample.Service
{
    public static partial class BluetoothService
    {
        static UUID Uuid => UUID.FromString("00001105-0000-1000-8000-00805f9B34FB");
        static List<BluetoothDevice> BondedDevices => BluetoothAdapter.DefaultAdapter.BondedDevices.ToList();
        static Dictionary<string, BluetoothSocket> SocketMap = new Dictionary<string, BluetoothSocket>();

        public static partial void Open()
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter != null)
            {
                if (!mBluetoothAdapter.IsEnabled)
                {
                    mBluetoothAdapter.Enable();
                }               
            }
        }

        public static partial List<string> GetDevices()
        {
            return BondedDevices.Select(p => p.Name).ToList();
        }

        public static partial bool Connected(string deviceName)
        {
            try
            {
                if(SocketMap.ContainsKey(deviceName)) return true; 
                var device = BondedDevices.FirstOrDefault(d => d.Name == deviceName);
               // var socket = device.CreateInsecureRfcommSocketToServiceRecord(device.GetUuids().ElementAt(0).Uuid);
                var socket = device.CreateRfcommSocketToServiceRecord(Uuid);
                socket.Connect();
                SocketMap.Add(deviceName, socket);
                return true;
            }
            catch
            {

            }
            return false;
        }

        public static partial bool Send(string deviceName,string message)
        {
            try
            {
                if (SocketMap.ContainsKey(deviceName) is false) return false;
                var socket = SocketMap[deviceName];
                byte[] byteArray = Encoding.ASCII.GetBytes(message);
                socket.OutputStream.WriteAsync(byteArray, 0, byteArray.Length);
                return true;
            }
            catch
            {

            }
            return false;           
        }

        public static partial void Listen(string deviceName, ParameterizedThreadStart connected)
        {
            Task.Run(() => 
            {
                BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                BluetoothServerSocket serverSock = mBluetoothAdapter.ListenUsingRfcommWithServiceRecord("Bluetooth", Uuid);
                try
                {
                    BluetoothSocket sock = serverSock.Accept(10000);
                    //serverSock.Close();//服务器获得连接后腰及时关闭ServerSocket
                    //启动新的线程，开始数据传输
                    Thread t = new Thread(connected);
                    t.Start(sock);
                }
                catch (Exception ex)
                {
                    serverSock.Close();
                    Listen(deviceName, connected);
                }
            });      
        }

        //public static partial async Task Listen(string deviceName, ParameterizedThreadStart connected)
        //{
        //    Thread t = new Thread(Monitor);
        //    t.Start(connected);
        //}

        //private static void Monitor(object connected)
        //{
        //    if(connected is ParameterizedThreadStart action)
        //    {
        //        BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        //        BluetoothServerSocket serverSock = mBluetoothAdapter.ListenUsingRfcommWithServiceRecord("Bluetooth", Uuid);
        //        BluetoothSocket sock = serverSock.Accept(10000);
        //        Thread t = new Thread(action);
        //        t.Start(sock);
        //    }
        //}
    }
}
