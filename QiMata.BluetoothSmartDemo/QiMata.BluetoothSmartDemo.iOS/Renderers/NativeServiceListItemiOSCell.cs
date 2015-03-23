using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using QiMata.BluetoothSmartDemo.Controls;
using QiMata.BluetoothSmartDemo.ViewModels.ListItems;
using UIKit;

namespace QiMata.BluetoothSmartDemo.iOS.Renderers
{
    class NativeServiceListItemiOSCell : UITableViewCell
    {
        private ServiceListItemViewCell<CharacteristicListItemViewModel> _listItemViewCell;

        public NativeServiceListItemiOSCell(ServiceListItemViewCell<CharacteristicListItemViewModel> listItemViewCell)
        {
            _listItemViewCell = listItemViewCell;


        }


    }
}