using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Java.Util;
using Android.Bluetooth;
using System.Threading.Tasks;
using static Android.Net.Sip.SipSession;
using System.Linq;
using Android.Content.PM;

namespace MRprobot.Droid
{
    [Activity(Label = "MRprobot", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //code van de volgende website gehaald http://alejandroruizvarela.blogspot.com/2014/01/bluetooth-arduino-xamarinandroid.html
        BluetoothConnection myConnection = new BluetoothConnection();
        ToggleButton tgConnect;
        TextView Result;
        private Java.Lang.String dataToSend;
        private BluetoothAdapter mBluetoothAdapter = null;
        private BluetoothSocket btSocket = null;
        private Stream outStream = null;
        private Stream inStream = null;
        protected override void OnCreate(Bundle bundle)
        {
            
  
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);

            //Haal de button uit de layout bron
            //verbind hieraan een event
            Arm ButtonUP = new Arm();

            
            Button BleuthootConnect = FindViewById<Button>(Resource.Id.);
            Button BleuthootDisconnect = FindViewById<Button>(Arm.ClassIdProperty(ButtonUP));
            Button ButtonOmhoog = FindViewById<Button>(Arm.ClassIdProperty(ButtonUP));
            Button ButtonVooruit = FindViewById<Button>(Wielen.ClassIdProperty(ButtonFront));
            //Button ButtonAchteruit = FindViewById<Button>(Resource.Id.button4);
            //Button ButtonRechts = FindViewById<Button>(Resource.Id.button5);
           

            
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

                    Console.WriteLine(ex.Message);
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