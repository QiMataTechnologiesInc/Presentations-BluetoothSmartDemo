using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.Bluetooth.Services;
using QiMata.BluetoothSmartDemo.Views;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo.ViewModels.Helpers
{
    static class ViewLocator
    {
        private static NavigationPage _basePage;
        private static MainPage _mainPage;
        private static DeviceDetailPage _deviceDetailPage;
        private static CharacteristicWritePage _characteristicWritePage;
        private static ReadCharacteristicPage _readCharacteristicPage;

        public static NavigationPage BasePage {
            get
            {
                return _basePage ?? (_basePage = new NavigationPage(MainPage));
            }}
        public static MainPage MainPage {
            get
            {
                return _mainPage ?? (_mainPage = new MainPage());
            }}
        public static CharacteristicWritePage CharacteristicWritePage { get
        {
            return _characteristicWritePage ?? (_characteristicWritePage = new CharacteristicWritePage());
        }}

        public static DeviceDetailPage  DeviceDetailPage
        {
            get
            {
                return _deviceDetailPage ?? (_deviceDetailPage = new DeviceDetailPage());
            }
        }

        public static ReadCharacteristicPage ReadCharacteristicPage
        {
            get { return _readCharacteristicPage ?? (_readCharacteristicPage = new ReadCharacteristicPage()); }
        }
    }
}
