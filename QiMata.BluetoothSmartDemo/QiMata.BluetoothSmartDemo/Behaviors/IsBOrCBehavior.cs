using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.Behaviors
{
    public class IsBOrCBehavior : Behavior<Entry>
    {
        private static int b = 98;
        private static int c = 99;

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            int result;
            bool isValid = int.TryParse(args.NewTextValue, NumberStyles.HexNumber, new NumberFormatInfo(), out result);
            if (isValid)
            {
                isValid = result == c || result == b;
            }
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
