using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using QiMata.BluetoothSmartDemo.Bluetooth.Services;

namespace QiMata.BluetoothSmartDemo.Design.Services
{
    class MockBluetoothService : IBluetoothSmartService
    {
        private ObservableCollection<Device> _devices;

        public MockBluetoothService()
        {
            _devices = new ObservableCollection<Device>();
        }

        public async Task StartScanning()
        {
            RaiseScanningStateChanged(true);
            foreach (var device in MockDevices())
            {
                await Task.Delay(10);
                _devices.Add(device);
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
            RaiseScanningStateChanged(false);
        }

        public async Task StopScanning()
        {
            RaiseScanningStateChanged(false);
        }

        public bool IsEnabled
        {
            get { return true; }
        }

        public async Task ConnectToDevice(Device device)
        {
            var connectedDevice = _devices.Single(x => x.Address == device.Address);
            await Task.Delay(10);
            RaiseScanningStateChanged(true);
            foreach (var service in MockServices())
            {
                connectedDevice.Services.Add(service);
            }
        }

        private IEnumerable<Service> MockServices()
        {
            return new List<Service>
            {
                new Service()
                {
                    Id = Guid.NewGuid(),
                    Characteristics = MockCharacteristics()
                },
                new Service()
                {
                    Id = Guid.NewGuid(),
                    Characteristics = MockCharacteristics()
                },new Service()
                {
                    Id = Guid.NewGuid(),
                    Characteristics = MockCharacteristics()
                },
            };
        }

        private ICollection<Characteristic> MockCharacteristics()
        {
            return new List<Characteristic>
            {
                new Characteristic()
                {
                    Id = Guid.NewGuid(),
                    CanRead = true,
                    CanWrite = true,
                    CanUpdate = true,
                    Descriptors = MockDescriptors()
                },
                new Characteristic()
                {
                    Id = Guid.NewGuid(),
                    CanRead = true,
                    CanWrite = true,
                    CanUpdate = true,
                    Descriptors = MockDescriptors()
                },
                new Characteristic()
                {
                    Id = Guid.NewGuid(),
                    CanRead = true,
                    CanWrite = true,
                    CanUpdate = true,
                    Descriptors = MockDescriptors()
                },
                new Characteristic()
                {
                    Id = Guid.NewGuid(),
                    CanRead = true,
                    CanWrite = true,
                    CanUpdate = true,
                    Descriptors = MockDescriptors()
                },
                new Characteristic()
                {
                    Id = Guid.NewGuid(),
                    CanRead = true,
                    CanWrite = true,
                    CanUpdate = true,
                    Descriptors = MockDescriptors()
                },
            };
        }

        private ICollection<Descriptor> MockDescriptors()
        {
            return new List<Descriptor>
            {
                new Descriptor
                {
                    Id = Guid.NewGuid()
                }
            };
        }

        public async Task WriteCharacteristicValue(Characteristic characteristic, byte[] value)
        {
            await Task.Delay(10);
        }

        public byte[] ReadCharacteristic(Characteristic characteristic)
        {
            return new byte[] { 63,64,65 };
        }

        private List<Device> MockDevices()
        {
            return new List<Device>
            {
                new Device
                {
                    Address = Guid.NewGuid().ToString(),
                    Name = "Device 1"
                },
                new Device
                {
                    Address = Guid.NewGuid().ToString(),
                    Name = "Device 2"
                },
                new Device
                {
                    Address = Guid.NewGuid().ToString(),
                    Name = "Device 3"
                },
                new Device
                {
                    Address = Guid.NewGuid().ToString(),
                    Name = "Device 4"
                },
            };
        }

        public System.Collections.ObjectModel.ObservableCollection<Device> Devices
        {
            get { return _devices; }
        }

        public bool BluetoothSmartAvailable
        {
            get { return true; }
        }

        public event ScanningStateChanged ScanningStateChanged;

        private void RaiseScanningStateChanged(bool state)
        {
            if (ScanningStateChanged != null) ScanningStateChanged(this, new ScanningStateEventArgs(state.ToScanningState()));
        }
    }
}
