using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MRprobot
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Wielen : ContentPage
    {
        public Wielen()
        {
            InitializeComponent();
        }
        private async void NavigateButton_OnWielenClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}