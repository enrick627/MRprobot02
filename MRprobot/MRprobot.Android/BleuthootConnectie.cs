using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MRprobot.Droid
{
    class BleuthootConnectie
    {
        

    }
    namespace Jenx.AutomaticBluetoothPairDemo.Droid.Services
    {
        [BroadcastReceiver]
        [IntentFilter(new[] { Android.Bluetooth.BluetoothDevice.ActionPairingRequest }, Priority = (int)IntentFilterPriority.HighPriority)]
        public class BluetoothDiscoveryReceiver : BroadcastReceiver
        {
            private readonly string _pin;

            public BluetoothDiscoveryReceiver()
            {
            }

            public BluetoothDiscoveryReceiver(string pin)
            {
                _pin = pin;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;

                if (action == Android.Bluetooth.BluetoothDevice.ActionPairingRequest)
                {
                    var device = (Android.Bluetooth.BluetoothDevice)intent.GetParcelableExtra(Android.Bluetooth.BluetoothDevice.ExtraDevice);
                    var extraPairingVariant = intent.GetIntExtra(Android.Bluetooth.BluetoothDevice.ExtraPairingVariant, 0);

                    switch (extraPairingVariant)
                    {
                        case Android.Bluetooth.BluetoothDevice.PairingVariantPin:
                            if (TrySetPin(device, _pin))
                                InvokeAbortBroadcast();
                            break;
                        case Android.Bluetooth.BluetoothDevice.PairingVariantPasskeyConfirmation:
                            break;
                    }
                }
            }

            private static bool TrySetPin(Android.Bluetooth.BluetoothDevice device, string pin)
            {
                try
                {
                    return device.SetPin(PinToByteArray(pin));
                }
                catch
                {
                    return false;
                }
            }

            private static byte[] PinToByteArray(string pin)
            {
                return System.Text.Encoding.UTF8.GetBytes(pin);
            }
        }
    }
}