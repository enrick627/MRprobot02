using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using System.Linq;
using static Android.Graphics.ColorSpace;
using System.Threading;

namespace MRprobot.Droid
{
    [Activity(Label = "MRprobot", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        BluetoothConnection myConnection = new BluetoothConnection();
        private ThreadStart listener;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            //Haal de button uit de layout bron
            //verbind hieraan een event
            
            Button BleuthootConnect = FindViewById<Button>(Resource.Id.button1);
            Button BleuthootDisconnect = FindViewById<Button>(Resource.Id.button2);


            BluetoothSocket _socket = null;


            System.Threading.Thread listenThread = new System.Threading.Thread(listener);
            listenThread.Abort();

            BleuthootConnect.Click += delegate
            {

                listenThread.Start();
                myConnection = new BluetoothConnection();
                myConnection.getAdapter();
                myConnection.thisAdapter.StartDiscovery();

                try
                {
                    myConnection.getDevice();
                    myConnection.thisDevice.SetPairingConfirmation(false);

                    myConnection.thisDevice.SetPairingConfirmation(true);
                    myConnection.thisDevice.CreateBond();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("connect to device", ex);
                }
                myConnection.thisAdapter.CancelDiscovery();

                try
                { _socket = myConnection.thisDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")); } //the UUID of HC-05 and HC-06 is the same
                catch (Exception)
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Error");
                    alert.SetMessage("Please go to settings and connect with the bluetooth module at first.");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        // Ok button click taak: alert verdwijnt!
                    });
                    alert.Show();
                }

                myConnection.thisSocket = _socket;

                try
                {
                    myConnection.thisSocket.Connect();

                    buttonConnect.Text = "Connected to the Arduino!";
                    buttonDisconnect.Enabled = true;
                    buttonConnect.Enabled = false;

                }
                catch (Exception ex)
                { Console.WriteLine("connect to device", ex); }
            };

            buttonDisconnect.Click += delegate
            {

                try
                {
                    buttonConnect.Enabled = true;

                    myConnection.thisDevice.Dispose();

                    myConnection.thisSocket.OutputStream.WriteByte(200);
                    myConnection.thisSocket.OutputStream.Close();

                    myConnection.thisSocket.Close();

                    myConnection = new BluetoothConnection();
                    _socket = null;

                    buttonConnect.Text = "Not connected to the Arduino!";
                }
                catch { }
            };



           
        }
        
        public class BluetoothConnection
        {

            public void getAdapter() { this.thisAdapter = BluetoothAdapter.DefaultAdapter; }


            
            //LET OP!!!!!!! bd.name aanpassen aan de naam van de module bij ons project is dit hc-05 dus dit is momenteel correct.
            public void getDevice() { this.thisDevice = (from bd in this.thisAdapter.BondedDevices where bd.Name == "HC-05" select bd).FirstOrDefault(); }

            public BluetoothAdapter thisAdapter { get; set; }
            public BluetoothDevice thisDevice { get; set; }

            public BluetoothSocket thisSocket { get; set; }
        }
    }
}