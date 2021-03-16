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
using Android.Content.PM;

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
            Button ButtonVooruit = FindViewById<Button>(Resource.Id.button3);

            BluetoothSocket _socket = null;


            System.Threading.Thread listenThread = new System.Threading.Thread(listener);
            listenThread.Abort();

            BleuthootConnect.Click += delegate
            {
                //deze code heb ik van de volgende website:https://www.instructables.com/3-LED-Backlight-Xamarin-and-Arduino-With-HC05/
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

                }
                myConnection.thisAdapter.CancelDiscovery();

                _socket = myConnection.thisDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                myConnection.thisSocket = _socket;

                //   System.Threading.Thread.Sleep(500);
                try {
                    myConnection.thisSocket.Connect();
                    //deze kan later nog worden gemaakt voorlopig geeft dit nog een error.
                    //Connected.Text = "Verbonden!";
                    BleuthootConnect.Enabled = true;
                    BleuthootDisconnect.Enabled = false;

                    if (listenThread.IsAlive == false)
                    {
                        listenThread.Start();
                    }


                }
                catch (Exception CloseEX)
                {

                }


                myConnection.thisSocket = _socket;

                try
                {
                    myConnection.thisSocket.Connect();

                    BleuthootConnect.Text = "verbonden met de arduino!!! kusjes Enrick xxx";
                    BleuthootDisconnect.Enabled = true;
                    BleuthootConnect.Enabled = false;
                    if (listenThread.IsAlive == false)
                    {
                        listenThread.Start();
                    }
                }
                catch (Exception ex)
                { Console.WriteLine("connect to device", ex); }
            };

            BleuthootDisconnect.Click += delegate
            {

                try
                {
                    //is de connectie aan ja of nee?
                    BleuthootConnect.Enabled = true;

                    myConnection.thisDevice.Dispose();
                    //dit moet 187 zijn want dit is de nummer om te verbinden met de bleuthoot module HC05.
                    myConnection.thisSocket.OutputStream.WriteByte(187);
                    myConnection.thisSocket.OutputStream.Close();

                    myConnection.thisSocket.Close();

                    myConnection = new BluetoothConnection();
                    _socket = null;

                    BleuthootConnect.Text = "Not connected to the Arduino!";
                }
                catch { }
            };


            ButtonVooruit.Click += delegate
            {
                try
                {
                    //De nummer die moet worden doorgegeven voor vooruit de gaan is 4.
                    myConnection.thisSocket.OutputStream.WriteByte(4);
                    myConnection.thisSocket.OutputStream.Close();
                }
                catch (Exception ex)
                {


                }

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