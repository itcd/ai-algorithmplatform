using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using Perspective.Core;
using System.Windows.Media;
using System.Reflection;

namespace PerspectiveDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Localization test
            //------------------
            // string culture = "fr-FR";
            string culture = "en-US";
            // string culture = "ca-ES";
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo(culture);
        }

        private string _filename = "PerspectiveDemo.dat";

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            IsolatedStorageHelper.SaveToUserStoreForDomain(this.Properties, _filename);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IsolatedStorageHelper.LoadFromUserStoreForDomain(this.Properties, _filename);

            MainWindow mw = new MainWindow();
            mw.Show();
        }

    }
}
