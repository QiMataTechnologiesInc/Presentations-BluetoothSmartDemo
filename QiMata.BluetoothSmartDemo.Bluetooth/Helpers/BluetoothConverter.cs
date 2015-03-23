using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiMata.BluetoothSmartDemo.Bluetooth.Helpers
{
    public static class BluetoothConverter
    {
        private const string ExtendedBLEUUID = "-0000-1000-8000-00805F9B34FB";

        public static Guid ConvertBluetoothLeUuid(string discoveredUuid)
        {
            var uuid = FixUuid(discoveredUuid);
            return Guid.Parse(uuid);
        }

        public static byte[] ConvertHexString(string value)
        {
            return BitConverter.GetBytes(int.Parse(value, NumberStyles.HexNumber)).Where(x => x != 0).ToArray();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static string FixUuid(string original)
        {
            if (original.Length == 4)
            {
                return "0000" + original + ExtendedBLEUUID;
            }
            else if (original.Length == 8)
            {
                return original + ExtendedBLEUUID;
            }
            else
            {
                return original;
            }
        }

        public static string ServiceNameConverter(Guid serviceId)
        {
            switch (serviceId.ToString())
            {
                case "00001811-0000-1000-8000-00805f9b34fb":
                    return "AlertNotification";
                case "0000180f-0000-1000-8000-00805f9b34fb":
                    return "Battery";
                case "00001810-0000-1000-8000-00805f9b34fb":
                    return "BloodPressure";
                case "00001805-0000-1000-8000-00805f9b34fb":
                    return "CurrentTime";
                case "00001818-0000-1000-8000-00805f9b34fb":
                    return "CyclingPower";
                case "00001816-0000-1000-8000-00805f9b34fb":
                    return "CyclingSpeedAndCadence";
                case "0000180a-0000-1000-8000-00805f9b34fb":
                    return "DeviceInformation";
                case "00001800-0000-1000-8000-00805f9b34fb":
                    return "GenericAccess";
                case "00001801-0000-1000-8000-00805f9b34fb":
                    return "GenericAttribute";
                case "00001808-0000-1000-8000-00805f9b34fb":
                    return "Glucose";
                case "00001809-0000-1000-8000-00805f9b34fb":
                    return "HealthThermometer";
                case "0000180d-0000-1000-8000-00805f9b34fb":
                    return "HeartRate";
                case "00001812-0000-1000-8000-00805f9b34fb":
                    return "HumanInterfaceDevice";
                case "00001802-0000-1000-8000-00805f9b34fb":
                    return "ImmediateAlert";
                case "00001803-0000-1000-8000-00805f9b34fb":
                    return "LinkLoss";
                case "00001819-0000-1000-8000-00805f9b34fb":
                    return "LocationAndNavigation";
                case "00001807-0000-1000-8000-00805f9b34fb":
                    return "NextDstChange";
                case "0000180e-0000-1000-8000-00805f9b34fb":
                    return "PhoneAlertStatus";
                case "00001806-0000-1000-8000-00805f9b34fb":
                    return "ReferenceTimeUpdate";
                case "00001814-0000-1000-8000-00805f9b34fb":
                    return "RunningSpeedAndCadence";
                case "00001813-0000-1000-8000-00805f9b34fb":
                    return "ScanParameters";
                case "00001804-0000-1000-8000-00805f9b34fb":
                    return "TxPower";
                default:
                    return serviceId.ToString();
            }
        }
    }
}
