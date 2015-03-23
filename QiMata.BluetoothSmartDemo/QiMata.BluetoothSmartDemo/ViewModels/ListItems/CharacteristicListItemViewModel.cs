using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.BluetoothSmartDemo.Bluetooth.Models;
using Xamarin.Forms;
using QiMata.BluetoothSmartDemo.ViewModels.Helpers;

namespace QiMata.BluetoothSmartDemo.ViewModels.ListItems
{
    public class CharacteristicListItemViewModel
    {
        private Characteristic _characteristic;
        private Command _openWriteCommand;
        private Command _openReadCommand;

        public CharacteristicListItemViewModel(Characteristic characteristic)
        {
            _characteristic = characteristic;
        }

        public string Title
        {
            get { return "Characteristic Details"; }
        }

        public string Name
        {
            get { return _characteristic.Name ?? _characteristic.Id.ToString(); }
        }

        public bool CanRead
        {
            get { return _characteristic.CanRead; }
        }

        public bool CanWrite
        {
            get { return _characteristic.CanWrite; }
        }

        public IEnumerable<string> Descriptors
        {
            get { return _characteristic.Descriptors.Select(x => x.Name); }
        }

        public Command OpenWriteModalCommand
        {
            get { return _openWriteCommand ?? (_openWriteCommand = new Command(async () => await OpenWriteModal())); }
        }

        public async Task OpenWriteModal()
        {
            var navigationPage = ViewLocator.BasePage;
            var writeModalViewModel = ViewModelLocator.Current.WriteModalViewModel;
            writeModalViewModel.Characteristic = _characteristic;
            var writeModalPage = ViewLocator.CharacteristicWritePage;
            await navigationPage.PushAsync(writeModalPage);
        }

        public Command OpenReadPageCommand
        {
            get { return _openReadCommand ?? (_openReadCommand = new Command(async () => await OpenReadPage())); }
        }

        public async Task OpenReadPage()
        {
            var navigation = ViewLocator.BasePage;
            var readPageViewModel = ViewModelLocator.Current.ReadCharacteristicViewModel;
            readPageViewModel.Characteristic = _characteristic;
            var readPage = ViewLocator.ReadCharacteristicPage;
            await navigation.PushAsync(readPage);
        }
    }
}
