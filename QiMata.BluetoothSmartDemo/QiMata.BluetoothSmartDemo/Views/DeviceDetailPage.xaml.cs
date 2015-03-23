using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.ViewModels.Helpers;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.Views
{
    public partial class DeviceDetailPage : ContentPage
    {
        public DeviceDetailPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Current.DeviceDetailViewModel;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView) sender).SelectedItem = null;
        }
    }
}
