using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using QiMata.BluetoothSmartDemo.Bluetooth.Services;
using QiMata.BluetoothSmartDemo.Droid.Services;
using Xamarin.Forms;
using Device = QiMata.BluetoothSmartDemo.Bluetooth.Models.Device;

[assembly: Dependency(typeof(BluetoothSmartService))]
namespace QiMata.BluetoothSmartDemo.Droid.Services
{
    public class BluetoothSmartService : Java.Lang.Object, Android.Bluetooth.BluetoothAdapter.ILeScanCallback,
        IBluetoothSmartService
    {
        private ObservableCollection<Device> _devices;
        internal BluetoothManager _bluetoothManager;
        internal BluetoothAdapter _bluetoothAdapter;
        internal IDictionary<Device, BluetoothGatt> _gattServices;
        internal AutoResetEvent _deviceConnectedEvent;

        public BluetoothSmartService()
        {
            _devices = new ObservableCollection<Device>();
            _bluetoothManager = (BluetoothManager) Android.App.Application.Context.GetSystemService("bluetooth");
            _bluetoothAdapter = _bluetoothManager.Adapter;
            _gattServices = new ConcurrentDictionary<Device, BluetoothGatt>();
            _deviceConnectedEvent = new AutoResetEvent(false);
        }

        public ObservableCollection<Device> Devices
        {
            get { return _devices; }
        }

        public async Task StartScanning()
        {
            this._bluetoothAdapter.StartLeScan(this);
            RaiseScanningStateChanged(true);
            await Task.Delay(TimeSpan.FromSeconds(10));
            this._bluetoothAdapter.StopLeScan(this);
            RaiseScanningStateChanged(false);
        }

        public async Task StopScanning()
        {
            RaiseScanningStateChanged(false);
            this._bluetoothAdapter.StopLeScan(this);
        }

        public bool IsEnabled
        {
            get { return _bluetoothAdapter.IsEnabled; }
        }

        public bool BluetoothSmartAvailable
        {
            get { return _bluetoothAdapter != null; }
        }

        public async Task ConnectToDevice(Device device)
        {
            var gatt = ((BluetoothDevice)device.NativeDevice).ConnectGatt(Android.App.Application.Context, true, new GattCallback(this));
            _gattServices[device] = gatt;
            await Task.Run(() => _deviceConnectedEvent.WaitOne());
            _deviceConnectedEvent.Reset();
        }

        public event ScanningStateChanged ScanningStateChanged;

        private void RaiseScanningStateChanged(bool state)
        {
            if (ScanningStateChanged != null)
                ScanningStateChanged(this, new ScanningStateEventArgs(state.ToScanningState()));
        }

        #region ILeScanCallback Implementation

        public void OnLeScan(Android.Bluetooth.BluetoothDevice device, int rssi, byte[] scanRecord)
        {
            if (_devices.All(x => x.Address != device.Address))
            {
                _devices.Add(ConvertDevice(device));
            }
        }

        private Device ConvertDevice(Android.Bluetooth.BluetoothDevice device)
        {
            return new Device
            {
                Address = device.Address,
                Name = device.Name,
                NativeDevice =  device
            };
        }

        #endregion

        public async Task WriteCharacteristicValue(Characteristic characteristic, byte[] value)
        {
            var gatt = _gattServices[characteristic.Service.Device];
            var nativeCharacteristic = gatt.Services.SelectMany(x => x.Characteristics)
                .First(x => Guid.Parse(x.Uuid.ToString()) == characteristic.Id);
            nativeCharacteristic.SetValue(value);
            gatt.WriteCharacteristic(nativeCharacteristic);
        }

        public byte[] ReadCharacteristic(Characteristic characteristic)
        {
            var gatt = _gattServices[characteristic.Service.Device];
            var nativeCharacteristic = gatt.Services.SelectMany(x => x.Characteristics)
                .First(x => Guid.Parse(x.Uuid.ToString()) == characteristic.Id);
            return nativeCharacteristic.GetValue();
        }
    }
}