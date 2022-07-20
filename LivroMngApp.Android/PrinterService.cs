using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using LivroMngApp.Droid;
using LivroMngApp.Models;
using LivroMngApp.Services;
using Android.Util;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(PrinterService))]
namespace LivroMngApp.Droid
{
    public class PrinterService : IBluetoothPrinterService
    {
        private BluetoothDevice _connectedDevice;
        private static Context context = global::Android.App.Application.Context;
        public PrinterService()
        {
        }

        public List<DeviceInfo> GetAvailableDevices()
        {
            if (BluetoothAdapter.DefaultAdapter != null && BluetoothAdapter.DefaultAdapter.IsEnabled)
            {
                List<DeviceInfo> result = new List<DeviceInfo>();
                foreach (var pairedDevice in BluetoothAdapter.DefaultAdapter.BondedDevices)
                {
                    result.Add(new DeviceInfo
                    {
                        Title = pairedDevice.Name,
                        MacAddress = pairedDevice.Address
                    });
                }
                return result;
            }
            return null;
        }

        public DeviceInfo GetCurrentDevice()
        {
            if (_connectedDevice != null)
            {
                return new DeviceInfo
                {
                    Title = _connectedDevice.Name,
                    MacAddress = _connectedDevice.Address
                };
            }
            return null;
        }

        public bool SetCurrentDevice(string printerName)
        {
            try
            {
                BluetoothManager BTManager = (BluetoothManager)context.GetSystemService(Context.BluetoothService);
                if (BTManager.Adapter != null && BTManager.Adapter.IsEnabled)
                {
                    foreach (var pairedDevice in BTManager.Adapter.BondedDevices)
                    {
                        if (pairedDevice.Name.ToLower().Contains("printer"))
                        {
                            _connectedDevice = pairedDevice;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public void PrintQR(string content)
        {
            SendCommandToPrinter("qr", content);
        }

        public async Task PrintText(string content)
        {
            await SendCommandToPrinter("plain", content);
        }

        async Task SendCommandToPrinter(string type, string content)
        {
            if (SetCurrentDevice(String.Empty))
            {
                if (string.IsNullOrEmpty(content)) return;
                Printer print = new Printer();
                if (_connectedDevice != null)
                {
                    await print.Print(type, content, _connectedDevice);
                }
                else
                {
                    Log.Debug("SunmiPrinter", "No device available.");
                }
            }
        }
    }
}