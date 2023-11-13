using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientServer;
using DLL;
using static Community.CsharpSqlite.Sqlite3;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private ClientServerInterface foob;
        public LoginPage()
        {
            InitializeComponent();
            ChannelFactory<ClientServer.ClientServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:400/ClientServer";
            foobFactory = new ChannelFactory<ClientServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Client newClient = new Client
            {
                Username = UsernameTextBox.Text.ToString(),
                IPAddress = IpAddressTextBox.Text.ToString(),
                Port = Convert.ToInt32(PortTextBox.Text)
            };

            Boolean createdClient = await Task.Run(() => foob.CreateClient(newClient));

            if (createdClient == true)
            {
                MessageBox.Show("Client added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ProcessStartInfo startInfo = new ProcessStartInfo("ClientServer.exe");//Starts a thread running the server on the GUI's port number
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.Arguments = $"{newClient.Port} {newClient.IPAddress}";
                Console.WriteLine($"Server Start at {newClient.Port}, {newClient.IPAddress}");
                Process.Start(startInfo);
                Console.ReadLine();

                AppPage appPage = new AppPage(newClient.Port, newClient.IPAddress, newClient.Username);
                NavigationService.Navigate(appPage);
            }
            else
            {
                MessageBox.Show("Client Failed", "Failure", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
