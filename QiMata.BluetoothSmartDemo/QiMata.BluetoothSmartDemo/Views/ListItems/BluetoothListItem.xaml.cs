using QiMata.BluetoothSmartDemo.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.ViewModels.ListItems;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.Views.ListItems
{
    public partial class BluetoothListItem : ViewCell
    {
        public BluetoothListItem()
        {
            InitializeComponent();
        }

        private void BluetoothListItem_OnTapped(object sender, EventArgs e)
        {
            var vm = this.BindingContext as BluetoothListItemViewModel;
            if (vm != null) vm.NavigateToDetails.Execute(null);
        }
    }
}
