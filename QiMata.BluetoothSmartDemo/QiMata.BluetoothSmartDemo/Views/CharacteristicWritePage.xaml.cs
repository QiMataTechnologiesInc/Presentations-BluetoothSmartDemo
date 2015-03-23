using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.ViewModels.Helpers;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.Views
{
    public partial class CharacteristicWritePage : ContentPage
    {
        public CharacteristicWritePage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Current.WriteModalViewModel;
        }
    }
}
