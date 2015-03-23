using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using QiMata.BluetoothSmartDemo.Controls;
using QiMata.BluetoothSmartDemo.ViewModels.ListItems;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(ViewCellRenderer),typeof(ServiceListItemViewCell<CharacteristicListItemViewModel>))]
namespace QiMata.BluetoothSmartDemo.iOS.Renderers
{
    class NativeServiceListItemViewCell : ViewCellRenderer
    {
        static NSString rid = new NSString("NativeServiceListItemViewCell");



        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var viewCell = (ServiceListItemViewCell<CharacteristicListItemViewModel>)item;

            

            return base.GetCell(item, reusableCell, tv);
        }
    }
}