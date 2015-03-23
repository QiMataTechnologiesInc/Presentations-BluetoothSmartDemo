using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using Xamarin.Forms.Platform.Android;

namespace QiMata.BluetoothSmartDemo.Droid.Services
{
    class GattCallback : BluetoothGattCallback
    {
        private BluetoothSmartService _bluetoothLeService;

        public GattCallback(BluetoothSmartService bluetoothLeService)
        {
            _bluetoothLeService = bluetoothLeService;
        }


        public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            base.OnConnectionStateChange(gatt, status, newState);

            var device = this._bluetoothLeService.Devices.SingleOrDefault(x => x.Address == gatt.Device.Address);

            if (device == null)
            {
                return; //not sure why i end up here but for a demo, its not worth investigating
            }

            switch (newState)
            {
                case ProfileState.Connected:
                    device.Connected = true;
                    gatt.DiscoverServices();
                    break;
                case ProfileState.Connecting:
                    break;
                case ProfileState.Disconnected:
                    device.Connected = false;
                    break;
                case ProfileState.Disconnecting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("newState");
            }
        }

        public override void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
        {
            base.OnServicesDiscovered(gatt, status);

            var device = GetCommonDevice(gatt);

            foreach (var service in gatt.Services)
            {
                var serviceModel = ConvertService(service);
                serviceModel.Device = device;
                device.Services.Add(serviceModel);
            }

            _bluetoothLeService._deviceConnectedEvent.Set();
        }

        public override void OnDescriptorRead(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
        {
            base.OnDescriptorRead(gatt, descriptor, status);

            var device = GetCommonDevice(gatt);

            //TODO: implement
            //var commonDescriptor = device.Services.SelectMany(x => x.Characteristics).SelectMany(x => x.Descriptors).Single(x => x.Id == descriptor.Uuid.)
        }

        public override void OnDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
        {
            base.OnDescriptorWrite(gatt, descriptor, status);
        }

        public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            base.OnCharacteristicWrite(gatt, characteristic, status);
        }

        public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            base.OnCharacteristicRead(gatt, characteristic, status);
        }

        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            base.OnCharacteristicChanged(gatt, characteristic);
        }

        private QiMata.BluetoothSmartDemo.Bluetooth.Models.Service ConvertService(BluetoothGattService service)
        {
            var serviceModel = new QiMata.BluetoothSmartDemo.Bluetooth.Models.Service
            {
                Id = Guid.Parse(service.Uuid.ToString()),
                Characteristics  = service.Characteristics.Select(ConvertCharacteristic).ToList()
            };
            foreach (var characteristic in serviceModel.Characteristics)
            {
                characteristic.Service = serviceModel;
            }
            return serviceModel;
        }

        private Characteristic ConvertCharacteristic(BluetoothGattCharacteristic characteristic)
        {
            var characteristicModel = new Characteristic
            {
                Id = Guid.Parse(characteristic.Uuid.ToString()),
                CanRead = characteristic.Permissions == GattPermission.Read,
                CanWrite = characteristic.Permissions == GattPermission.Write,
                CanUpdate = characteristic.Permissions == GattPermission.Write,
                Descriptors = characteristic.Descriptors.Select(x => ConvertDescriptor(x)).ToList()
            };
            foreach (var descriptor in characteristicModel.Descriptors)
            {
                descriptor.Characteristic = characteristicModel;
            }
            return characteristicModel;
        }

        private Descriptor ConvertDescriptor(BluetoothGattDescriptor descriptor)
        {
            return new Descriptor
            {
                Id = Guid.Parse(descriptor.Uuid.ToString()),
                //Value = descriptor.GetValue().ToString() //TODO: update
            };
        }

        private Device GetCommonDevice(BluetoothGatt gatt)
        {
            return this._bluetoothLeService.Devices.Single(x => x.Address == gatt.Device.Address);
        }
    }
}