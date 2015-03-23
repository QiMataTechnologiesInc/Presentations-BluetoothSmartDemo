using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.Bluetooth.Services;
using QiMata.BluetoothSmartDemo.ViewModels.Helpers;
using QiMata.BluetoothSmartDemo.ViewModels.ListItems;
using Xamarin.Forms;
using Device = QiMata.BluetoothSmartDemo.Bluetooth.Models.Device;

namespace QiMata.BluetoothSmartDemo.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        private IBluetoothSmartService _bluetoothSmartService;
        private bool _isScanning;

        public MainPageViewModel(IBluetoothSmartService bluetoothSmartService)
        {
            _bluetoothSmartService = bluetoothSmartService;
            _bluetoothSmartService.Devices.CollectionChanged += ConnectedDevices_CollectionChanged;
            
            _devices = new ObservableCollection<BluetoothListItemViewModel>();
            _startScanning = new Command(StartScan);
            _stopScanning = new Command(EndScan);
            _bluetoothSmartService.ScanningStateChanged += BluetoothLeServiceOnScanningStateChanged;
        }

        private void BluetoothLeServiceOnScanningStateChanged(object sender, ScanningStateEventArgs scanningStateEventArgs)
        {
            _isScanning = scanningStateEventArgs.ScanningState.ToBool();
            RaisePropertyChanged(() => this.IsScanning);
        }

        void ConnectedDevices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Device bluetoothLeDevice;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    bluetoothLeDevice = e.NewItems[0] as Device;
                    if (bluetoothLeDevice != null)
                        _devices.Add(new BluetoothListItemViewModel(bluetoothLeDevice));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    bluetoothLeDevice = e.OldItems[0] as Device;
                    if (bluetoothLeDevice != null)
                    {
                        var bluetootheListItem = _devices.Single(x => x.Address == bluetoothLeDevice.Address);
                        _devices.Remove(bluetootheListItem);
                    }
                    break;
            }
        }


        private ObservableCollection<BluetoothListItemViewModel> _devices; 

        public ObservableCollection<BluetoothListItemViewModel> Devices
        {
            get { return _devices; }
        }

        private Command _startScanning;

        public Command StartScanning
        {
            get
            {
                return _startScanning;
            }
        }

        private Command _stopScanning;

        public Command StopScanning
        {
            get
            {
                return _stopScanning;
            }
        }

        public string Title
        {
            get { return "Bluetooth Demo App"; }
        }

        private async void StartScan()
        {
           _devices.Clear();
            _bluetoothSmartService.Devices.Clear();
           await _bluetoothSmartService.StartScanning();            
        }

        private async void EndScan()
        {
            await _bluetoothSmartService.StopScanning();
        }

        public bool IsScanning
        {
            get { return _isScanning; }
        }
    }
}
