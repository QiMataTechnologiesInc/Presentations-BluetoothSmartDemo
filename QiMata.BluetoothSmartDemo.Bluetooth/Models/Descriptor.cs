using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QiMata.BluetoothSmartDemo.Bluetooth.Models
{
    public class Descriptor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public Object NativeDescriptor { get; set; }

        public Characteristic Characteristic { get; set; }
    }
}
