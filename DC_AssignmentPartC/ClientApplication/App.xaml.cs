using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            NavigationWindow navigationWindow = new NavigationWindow();
            navigationWindow.Content = new LoginPage();
            navigationWindow.Width = 830;
            navigationWindow.Height = 550;
            MainWindow = navigationWindow;
            navigationWindow.Show();
        }
    }
}

