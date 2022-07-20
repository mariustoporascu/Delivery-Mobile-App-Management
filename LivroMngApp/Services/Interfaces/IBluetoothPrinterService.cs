using System.Collections.Generic;
using System.Threading.Tasks;
using LivroMngApp.Models;

namespace LivroMngApp.Services
{
    public interface IBluetoothPrinterService
    {
        DeviceInfo GetCurrentDevice();

        List<DeviceInfo> GetAvailableDevices();

        bool SetCurrentDevice(string printerName);

        Task PrintText(string content);

        void PrintQR(string content);
    }
}