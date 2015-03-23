using QiMata.BluetoothSmartDemo.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Current.MainPageViewModel;
        }
    }
}
