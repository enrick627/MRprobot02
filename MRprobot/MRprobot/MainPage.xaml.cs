using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MRprobot
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }
        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Arm());
        }
        private async void NavigateButton_OnWielenClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Wielen());
        }
        private async void Bleuthootconnection(string pin)
        {
            try
            {
                var bluetoothDeviceReceiver = new BluetoothDiscoveryReceiver(pin);
                var intentFilter = new Android.Content.IntentFilter(Android.Bluetooth.BluetoothDevice.ActionPairingRequest);
                intentFilter.Priority = (int)Android.Content.IntentFilterPriority.HighPriority;
                var intent = MainActivity.Instance.RegisterReceiver(bluetoothDeviceReceiver, intentFilter);
            }
            catch
            {
                // log error
            }
        }
    }
}
