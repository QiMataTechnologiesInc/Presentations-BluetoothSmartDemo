using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiMata.BluetoothSmartDemo.Bluetooth.Models
{
    public class Service
    {
        public Service()
        {
            Characteristics = new List<Characteristic>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Object NativeService { get; set; }

        public ICollection<Characteristic> Characteristics { get; set; }

        public Device Device { get; set; }
    }
}
