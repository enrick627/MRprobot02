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
    public partial class Arm : ContentPage
    {
        public Arm()
        {
            InitializeComponent();
        }
        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        public static int ClassIdProperty(Arm buttonUP)
        {
            throw new NotImplementedException();
        }
    }
}