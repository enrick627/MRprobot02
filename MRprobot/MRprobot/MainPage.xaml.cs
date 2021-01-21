﻿using Android.Bluetooth;
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
            

            
        }
    }
}
