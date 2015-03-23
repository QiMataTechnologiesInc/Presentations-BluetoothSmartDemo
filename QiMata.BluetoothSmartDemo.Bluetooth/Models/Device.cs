using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiMata.BluetoothSmartDemo.Bluetooth.Models
{
    public class Device
    {
        public Device()
        {
            Services = new List<Service>();
        }

        public string Address { get; set; }

        public string Name { get; set; }

        public bool Connected { get; set; }

        public Object NativeDevice { get; set; }

        public ICollection<Service> Services { get; set; }
    }
}
