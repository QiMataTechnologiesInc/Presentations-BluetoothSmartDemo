using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;

namespace QiMata.BluetoothSmartDemo.Bluetooth.Services
{
    public enum ScanningState
    {
        Unknown = 0,
        Scanning = 1,
        NotScanning = 2
    }

    public class ScanningStateEventArgs
    {
        private ScanningState _scanningState;

        public ScanningState ScanningState
        {
            get { return _scanningState; }
        }

        public ScanningStateEventArgs(ScanningState scanningState)
        {
            _scanningState = scanningState;
        }
    }

    public static class Extensions
    {
        public static bool ToBool(this ScanningState scanningState)
        {
            switch (scanningState)
            {
                case ScanningState.NotScanning:
                    return false;
                case ScanningState.Scanning:
                    return true;
                default:
                    throw new ArgumentException("Scanning state not convertable to boolean");
            }
        }

        public static ScanningState ToScanningState(this bool scanningState)
        {
            switch (scanningState)
            {
                case true:
                    return ScanningState.Scanning;
                case false:
                    return ScanningState.NotScanning;
                default:
                    return ScanningState.Unknown;
            }
        }
    }

    public delegate void ScanningStateChanged(object sender, ScanningStateEventArgs e);

    public interface IBluetoothSmartService
    {
        ObservableCollection<Device> Devices { get; }

        Task StartScanning();

        Task StopScanning();

        bool IsEnabled { get; }

        bool BluetoothSmartAvailable { get; }

        Task ConnectToDevice(Device device);

        Task WriteCharacteristicValue(Characteristic characteristic, byte[] value);

        byte[] ReadCharacteristic(Characteristic characteristic);

        event ScanningStateChanged ScanningStateChanged;
    }
}
