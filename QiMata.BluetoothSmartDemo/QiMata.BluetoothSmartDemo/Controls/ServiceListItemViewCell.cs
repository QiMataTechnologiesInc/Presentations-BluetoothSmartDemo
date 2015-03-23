using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.Controls
{
    public class ServiceListItemViewCell<T> : ViewCell
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<ServiceListItemViewCell<T>, string>(p => p.Text, String.Empty, BindingMode.OneWay);

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty,value); }
        }

        public ObservableCollection<T> ItemsSource
        {
            get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create<RepeaterView<T>, ObservableCollection<T>>(p => p.ItemsSource, new ObservableCollection<T>(), BindingMode.OneWay);
    }
}
