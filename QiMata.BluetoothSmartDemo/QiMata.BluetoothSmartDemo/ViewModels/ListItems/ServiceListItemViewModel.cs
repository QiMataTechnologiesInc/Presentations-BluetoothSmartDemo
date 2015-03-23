using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.Bluetooth.Helpers;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.ViewModels.ListItems
{
    class ServiceListItemViewModel
    {
        private const int CharacteristicListItemvViewHeight = 120;

        private Service _serviceModel;

        public ServiceListItemViewModel(Service serviceModel)
        {
            _serviceModel = serviceModel;

        }

        public string Name
        {
            get { return _serviceModel.Name ?? (_serviceModel.Name = BluetoothConverter.ServiceNameConverter(_serviceModel.Id)); }
        }

        public ObservableCollection<CharacteristicListItemViewModel> Characteristics
        {
            get { return new ObservableCollection<CharacteristicListItemViewModel>(_serviceModel.Characteristics.Select(x => new CharacteristicListItemViewModel(x))); }
        }

        public double ViewHeight
        {
            get {
                if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
                {
                    return Math.Max(120,CharacteristicListItemvViewHeight * Characteristics.Count);
                    
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
