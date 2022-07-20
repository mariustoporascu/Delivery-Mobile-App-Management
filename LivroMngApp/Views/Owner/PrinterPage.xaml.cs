using LivroMngApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroMngApp.Views
{
    public partial class PrinterPage : ContentPage
    {
        public PrinterPage()
        {
            InitializeComponent();
        }
        private async void selectPrinterButton_Clicked(object sender, EventArgs e)
        {
            var devices = DependencyService.Get<IBluetoothPrinterService>().GetAvailableDevices();
            if (devices != null && devices.Count > 0)
            {
                var choices = devices.Select(d => d.Title).ToArray();
                string action = await Application.Current.MainPage.DisplayActionSheet("Select printer device.", "Cancel", null, choices);
                if (choices.Contains(action))
                {
                    SelectDevice(action);
                }
            }
            else
            {
                await DisplayAlert("Select Printer", "No device.", "OK");
            }
        }

        private void printQrButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IBluetoothPrinterService>().PrintQR(printBox.Text);
        }

        private void printTextButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IBluetoothPrinterService>().PrintText(printBox.Text);
        }

        void SelectDevice(string printerName)
        {
            if (DependencyService.Get<IBluetoothPrinterService>().SetCurrentDevice(printerName))
            {
                var current = DependencyService.Get<IBluetoothPrinterService>().GetCurrentDevice();
                if (current != null)
                {
                    printerNameEntry.Text = current.Title;
                    printQrButton.IsEnabled = printTextButton.IsEnabled = true;
                }
            }
        }
    }
}