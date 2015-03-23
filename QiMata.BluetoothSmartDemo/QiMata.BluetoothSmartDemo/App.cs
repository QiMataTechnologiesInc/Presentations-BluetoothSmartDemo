using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QiMata.BluetoothSmartDemo.ViewModels.Helpers;
using QiMata.BluetoothSmartDemo.Views;
using Xamarin.Forms;

namespace QiMata.BluetoothSmartDemo
{
    public class App : Application
    {
        public App()
        {
            base.MainPage = ViewLocator.BasePage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
