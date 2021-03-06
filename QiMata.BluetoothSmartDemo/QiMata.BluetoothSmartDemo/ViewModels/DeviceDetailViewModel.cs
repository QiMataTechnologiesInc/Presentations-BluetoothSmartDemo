﻿using QiMata.BluetoothSmartDemo.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using QiMata.BluetoothSmartDemo.Bluetooth.Services;
using QiMata.BluetoothSmartDemo.ViewModels.ListItems;

namespace QiMata.BluetoothSmartDemo.ViewModels
{
    class DeviceDetailViewModel : BaseViewModel
    {
        private Device _device;
        private IBluetoothSmartService _bluetoothService;

        public DeviceDetailViewModel(IBluetoothSmartService bluetoothService)
        {
            _bluetoothService = bluetoothService;
        }

        public Device Device
        {
            set
            {
                _device = value;
                base.RaisePropertyChanged("");
            }
        }

        public string Title
        {
            get { return "Device Details"; }
        }

        public string Name
        {
            get { return _device.Name; }
        }

        public IEnumerable<ServiceListItemViewModel> Services
        {
            get { return _device.Services.Select(x => new ServiceListItemViewModel(x)); }
        }

        public async Task ConnectToDevice()
        {
            await _bluetoothService.ConnectToDevice(_device);
        }
    }
}
