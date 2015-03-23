using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Background;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using QiMata.BluetoothSmartDemo.Bluetooth.Helpers;
using QiMata.BluetoothSmartDemo.Bluetooth.Services;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using QiMata.BluetoothSmartDemo.WinPhone.Services;
using Xamarin.Forms;
using Device = QiMata.BluetoothSmartDemo.Bluetooth.Models.Device;

[assembly: Dependency(typeof(BluetoothSmartService))]
namespace QiMata.BluetoothSmartDemo.WinPhone.Services
{
    public class BluetoothSmartService : IBluetoothSmartService
    {
        private ObservableCollection<Device> _devices;
        private CancellationTokenSource _cancellationTokenSource;

        public BluetoothSmartService()
        {
            _devices = new ObservableCollection<Device>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Bluetooth.Models.Device> Devices
        {
            get { return _devices; }
        }

        public async Task StartScanning()
        {
            using (_cancellationTokenSource = new CancellationTokenSource())
            {
                var cancellationToken = _cancellationTokenSource.Token;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                RaiseScanningStateChanged(true);
                while (cancellationToken.IsCancellationRequested && stopwatch.Elapsed < TimeSpan.FromSeconds(10))
                {
                    foreach (DeviceInformation di in
                            await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelector()))
                    {
                        BluetoothLEDevice bleDevice = await BluetoothLEDevice.FromIdAsync(di.Id);
                        if (_devices.All(x => x.Address != bleDevice.BluetoothAddress.ToString()))
                        {
                            _devices.Add(ConvertDevice(bleDevice));
                        }
                    }
                }
                RaiseScanningStateChanged(false);
            }
        }

        private Device ConvertDevice(BluetoothLEDevice bleDevice)
        {
            return new Device
            {
                Address = bleDevice.BluetoothAddress.ToString(),
                Connected = false,
                Name = bleDevice.Name,
                NativeDevice = bleDevice
            };
        }

        public async Task StopScanning()
        {
            RaiseScanningStateChanged(false);
            _cancellationTokenSource.Cancel();
        }

        //cant find it!
        public bool IsEnabled
        {
            get { return true; }
        }

        //can't find it
        public bool BluetoothSmartAvailable
        {
            get { return true; }
        }


        public async Task ConnectToDevice(Bluetooth.Models.Device device)
        {
            var nativeDevice = (BluetoothLEDevice)device.NativeDevice;
            device.Services = new List<Service>();
            foreach (var gattService in nativeDevice.GattServices)
            {
                Service service = new Service
                {
                    Device = device,
                    Id = gattService.Uuid,
                    NativeService = gattService,
                    Characteristics = new List<Characteristic>()
                };
                device.Services.Add(service);

                foreach (var gattCharacteristic in gattService.GetCharacteristics(Guid.Parse("6e400002-b5a3-f393-e0a9-e50e24dcca9e")))
                {
                    Characteristic characteristic = new Characteristic
                    {
                       Id = gattCharacteristic.Uuid,
                       NativeCharacteristic = gattCharacteristic,
                       Service = service,
                       Descriptors = new List<Descriptor>()
                    };
                    service.Characteristics.Add(characteristic);
                    foreach (var gattDescriptor in gattCharacteristic.GetDescriptors(Guid.Empty))
                    {
                        Descriptor descriptor = new Descriptor
                        {
                            Id = gattDescriptor.Uuid,
                            Characteristic = characteristic,
                            NativeDescriptor = gattDescriptor
                        };
                        characteristic.Descriptors.Add(descriptor);
                    }
                }
            }
        }

        public async Task WriteCharacteristicValue(Bluetooth.Models.Characteristic characteristic, byte[] value)
        {
            var nativeCharacteristic = (GattCharacteristic) characteristic.NativeCharacteristic;
            await nativeCharacteristic.WriteValueAsync(value.AsBuffer(), GattWriteOption.WriteWithoutResponse);
        }

        public byte[] ReadCharacteristic(Bluetooth.Models.Characteristic characteristic)
        {
            var nativeCharacteristic = (GattCharacteristic) characteristic.NativeCharacteristic;
            return nativeCharacteristic.ReadValueAsync(BluetoothCacheMode.Uncached).GetResults().Value.ToArray();
        }

        public event ScanningStateChanged ScanningStateChanged;

        private void RaiseScanningStateChanged(bool state)
        {
            if (ScanningStateChanged != null)
                ScanningStateChanged(this, new ScanningStateEventArgs(state.ToScanningState()));
        }

    }
}
