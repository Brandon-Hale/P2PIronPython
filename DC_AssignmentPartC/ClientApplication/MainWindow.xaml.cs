using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ServiceModel;
using System.ComponentModel;
using ClientServer;
using DLL;
using System.Net;
using System.Threading;
using System.ServiceModel.Channels;
using RestSharp;
using System.Diagnostics;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private ClientServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<ClientServer.ClientServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:400/ClientApplication";
            foobFactory = new ChannelFactory<ClientServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }
    }
}
