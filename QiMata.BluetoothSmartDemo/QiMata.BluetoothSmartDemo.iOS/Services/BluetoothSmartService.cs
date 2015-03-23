using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreBluetooth;
using Foundation;
using QiMata.BluetoothSmartDemo.Bluetooth.Helpers;
using QiMata.BluetoothSmartDemo.Bluetooth.Services;
using UIKit;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using Xamarin.Forms;
using Device = QiMata.BluetoothSmartDemo.Bluetooth.Models.Device;
using QiMata.BluetoothSmartDemo.iOS.Services;

[assembly: Dependency(typeof(BluetoothSmartService))]
namespace QiMata.BluetoothSmartDemo.iOS.Services
{
    internal class BluetoothSmartService : IBluetoothSmartService
    {
        private ObservableCollection<Device> _devices;
        private readonly AutoResetEvent _stateChanged = new AutoResetEvent(false);
        private readonly AutoResetEvent _deviceConnected = new AutoResetEvent(false);
        private CBCentralManager _centralManager;

        public BluetoothSmartService()
        {
            _devices = new ObservableCollection<Device>();
            _centralManager = new CBCentralManager(CoreFoundation.DispatchQueue.MainQueue);
            _centralManager.DiscoveredPeripheral += _centralManager_DiscoveredPeripheral;
            _centralManager.UpdatedState += _centralManager_UpdatedState;
            _centralManager.ConnectedPeripheral += _centralManager_ConnectedPeripheral;
        }

        private void _centralManager_UpdatedState(object sender, EventArgs e)
        {
            Debug.WriteLine("State Changed to " + ((CBCentralManager) sender).State);
            this._stateChanged.Set();
        }

        private void _centralManager_ConnectedPeripheral(object sender, CBPeripheralEventArgs e)
        {
            Debug.WriteLine("Peripheral Connected");
            _devices.Single(x => ((CBPeripheral)x.NativeDevice).Identifier == e.Peripheral.Identifier).Connected = true;
            _deviceConnected.Set();
        }

        private void _centralManager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            if (e != null && e.Peripheral != null && e.Peripheral.Identifier != null)
            {
                this._devices.Add(new Device
                {
                    Address = e.Peripheral.Identifier.AsString(),
                    Name = e.Peripheral.Name,
                    NativeDevice = e.Peripheral
                });
            }
        }

        private async Task WaitForState(CBCentralManagerState state)
        {
            while (this._centralManager.State != state)
            {
                await Task.Run(() => this._stateChanged.WaitOne());
            }
        }

        private async Task WaitForConnection()
        {
            await Task.Run(() => this._deviceConnected.WaitOne(TimeSpan.FromSeconds(10)));
            this._deviceConnected.Reset();
        }

        #region IBluetoothLEService Implementation

        public async Task StartScanning()
        {
            await WaitForState(CBCentralManagerState.PoweredOn);
            _centralManager.ScanForPeripherals(peripheralUuids: null);
            RaiseScanningStateChanged(true);
            await Task.Delay(10*1000);
            _centralManager.StopScan();
            RaiseScanningStateChanged(false);
        }

#pragma warning disable 1998
        public async Task StopScanning()
#pragma warning restore 1998
        {
            _centralManager.StopScan();
            RaiseScanningStateChanged(false);
        }

        public bool IsEnabled
        {
            get { return _centralManager.State != CBCentralManagerState.Unauthorized; }
        }

        public async Task ConnectToDevice(Device device)
        {
            if (device.Connected)
            {
                return;
            }
            var nativeDevice = (CBPeripheral) device.NativeDevice;
            _centralManager.ConnectPeripheral(nativeDevice);
            await WaitForConnection();
            await MapDevice(device, nativeDevice);
        }


        public async Task WriteCharacteristicValue(Characteristic characteristic, byte[] value)
        {
            var nativeDevice = (CBPeripheral) characteristic.Service.Device.NativeDevice;
            var nativeCharacteristic = (CBCharacteristic) characteristic.NativeCharacteristic;
            nativeDevice.WriteValue(NSData.FromArray(value), nativeCharacteristic,
                CBCharacteristicWriteType.WithResponse);
        }

        public byte[] ReadCharacteristic(Characteristic characteristic)
        {
            var nativeDevice = (CBPeripheral) characteristic.Service.Device.NativeDevice;
            var nativeCharacteristic = (CBCharacteristic) characteristic.NativeCharacteristic;
            nativeDevice.ReadValue(nativeCharacteristic);
            return nativeCharacteristic.Value.ToArray(); //Non optimal if it wasn't for small payload
        }

        #endregion

        private AutoResetEvent _servicesDiscovered = new AutoResetEvent(false);
        private AutoResetEvent _characteristicsDiscovered = new AutoResetEvent(false);
        private AutoResetEvent _descriptorDiscovered = new AutoResetEvent(false);

        private async Task MapDevice(Device device, CBPeripheral nativeDevice)
        {
            nativeDevice.DiscoveredService += nativeDevice_DiscoveredService;

            nativeDevice.DiscoverServices();
            await Task.Run(() => _servicesDiscovered.WaitOne(TimeSpan.FromSeconds(10)));

            nativeDevice.DiscoveredService -= nativeDevice_DiscoveredService;
            _servicesDiscovered.Reset();
            
            foreach (var cbService in nativeDevice.Services)
            {
                nativeDevice.DiscoveredCharacteristic += nativeDevice_DiscoveredCharacteristic;

                nativeDevice.DiscoverCharacteristics(cbService);
                await Task.Run(() => _characteristicsDiscovered.WaitOne(TimeSpan.FromSeconds(10)));

                nativeDevice.DiscoveredCharacteristic -= nativeDevice_DiscoveredCharacteristic;
                _characteristicsDiscovered.Reset();

                var service = new Service()
                {
                    Id = BluetoothConverter.ConvertBluetoothLeUuid(cbService.UUID.Uuid),
                    Device = device,
                    Characteristics = new List<Characteristic>()
                };

                foreach (var cbCharacteristic in cbService.Characteristics)
                {
                    var characteristic = await ConvertCharacteristic(cbCharacteristic);
                    characteristic.Service = service;
                    service.Characteristics.Add(characteristic);
                }
                service.Device = device;
                device.Services.Add(service);
            }
        }

        void nativeDevice_DiscoveredCharacteristic(object sender, CBServiceEventArgs e)
        {
            _characteristicsDiscovered.Set();
        }

        void nativeDevice_DiscoveredService(object sender, NSErrorEventArgs e)
        {
            _servicesDiscovered.Set();
        }

        private async Task<Characteristic> ConvertCharacteristic(CBCharacteristic nativeCharacteristic)
        {
            var device = nativeCharacteristic.Service.Peripheral;
            device.DiscoveredDescriptor += device_DiscoveredDescriptor;

            device.DiscoverDescriptors(nativeCharacteristic);
            await Task.Run(() => _descriptorDiscovered.WaitOne(TimeSpan.FromSeconds(10)));

            device.DiscoveredDescriptor -= device_DiscoveredDescriptor;
            _descriptorDiscovered.Reset();

            var characteristic = new Characteristic()
            {
                Id = BluetoothConverter.ConvertBluetoothLeUuid(nativeCharacteristic.UUID.Uuid),
                Descriptors = nativeCharacteristic.Descriptors.Select(ConvertDescriptor).ToList(),
                NativeCharacteristic = nativeCharacteristic
            };
            foreach (var descriptor in characteristic.Descriptors)
            {
                descriptor.Characteristic = characteristic;
            }
            return characteristic;
        }

        void device_DiscoveredDescriptor(object sender, CBCharacteristicEventArgs e)
        {
            _descriptorDiscovered.Set();
        }

        private Descriptor ConvertDescriptor(CBDescriptor nativeDescriptor)
        {
            return new Descriptor
            {
                Id = BluetoothConverter.ConvertBluetoothLeUuid(nativeDescriptor.UUID.Uuid),
                NativeDescriptor = nativeDescriptor
            };
        }

        public System.Collections.ObjectModel.ObservableCollection<Bluetooth.Models.Device> Devices
        {
            get { return _devices; }
        }

        public bool BluetoothSmartAvailable
        {
            get
            {
                return _centralManager.State != CBCentralManagerState.Unknown
                       && _centralManager.State == CBCentralManagerState.Unsupported;
            }
        }

        public event ScanningStateChanged ScanningStateChanged;

        private void RaiseScanningStateChanged(bool state)
        {
            if (ScanningStateChanged != null)
                ScanningStateChanged(this, new ScanningStateEventArgs(state.ToScanningState()));
        }
    }
}