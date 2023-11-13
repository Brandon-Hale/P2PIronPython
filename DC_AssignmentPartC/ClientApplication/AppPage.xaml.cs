using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using DLL;
using System.ServiceModel;
using ClientServer;
using System.Threading;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for AppPage.xaml
    /// </summary>
    public partial class AppPage : Page
    {
        private ClientServerInterface foob;
        private Timer updateTimer;
        private string username;
        private string ip;
        private int port;
        public AppPage(int port, string ip, string username)
        {
            InitializeComponent();
            ChannelFactory<ClientServer.ClientServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = $"net.tcp://localhost:{port}/ClientApplication";
            foobFactory = new ChannelFactory<ClientServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            this.port = port;
            this.ip = ip;
            this.username = username;
            this.Unloaded += AppPage_Unloaded;

            updateTimer = new Timer(UpdateCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private async void StartJobButton_Click(object sender, RoutedEventArgs e)
        {
            //pass job to server (upload job) then users can connect to that server and finish job, which returns answer back
            string pythonCode = PythonCodeTextBox.Text;
            string output = ExecutePythonCode(pythonCode);

            JobResultsListBox.Items.Add("Completed");
            JobStatusTextBlock.Text = output;

            Job job = new Job
            {
                Code = pythonCode,
                Result = output,
                Status = "Completed"
            };

            Boolean created = await Task.Run(() => foob.CreateJob(job));

            if (created == true)
            {
                MessageBox.Show("Job added Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Job failed to add!", "Failure", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            foob.UploadJob(job.Code);
        }

        private string ExecutePythonCode(string pythonCode)
        {
            ScriptEngine engine = Python.CreateEngine();
            string result = "";
            try
            {
                ScriptScope scope = engine.CreateScope();

                engine.Execute(pythonCode, scope);
                dynamic answer = scope.GetVariable("answer");
                result = answer.ToString();

                JobStatusTextBlock.Text = "Completed";
            }
            catch (Exception ex)
            {
                JobStatusTextBlock.Text = "Failed";
                result = "Error: " + ex.Message;
            }

            return result;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try //not right yet but mabye works
            {
                string ipAddress = ServerIpTextBox.Text;
                int port = int.Parse(ServerPortTextBox.Text);

                ChannelFactory<ClientServer.ClientServerInterface> foobFactoryClient;
                NetTcpBinding tcpClient = new NetTcpBinding();
                string URLClient = $"net.tcp://localhost:{port}/ClientApplication";
                foobFactoryClient = new ChannelFactory<ClientServerInterface>(tcpClient, URLClient);
                foob = foobFactoryClient.CreateChannel();
                foob.CallConsole(username, port);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void AppPage_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Boolean deletedClient = foob.DeleteClient(username);
                Console.WriteLine($"Deleted client status: {deletedClient}");
                foreach (var process in Process.GetProcessesByName("ClientServer"))
                {
                    process.Kill();
                }
                Application.Current.Shutdown();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (deletedClient)
                    {
                        MessageBox.Show("Client deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Client failed deletion", "Failure", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Deleted client status: {ex}");
            }
        }

        private void ClientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsListBox.SelectedItem != null)
            {
                string username = ClientsListBox.SelectedItem.ToString();
                Client client = foob.GetClientByUsername(username);

                if (client != null)
                {
                    UsernameTextBox.Text = client.Username;
                    ServerIpTextBox.Text = client.IPAddress;
                    ServerPortTextBox.Text = client.Port.ToString();
                }
                else
                {
                    MessageBox.Show("Client not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void UpdateCallback(object state) //update GUI texts quickly
        {
            try
            {
                List<Client> clients = foob.GetClients();
                List<Job> jobs = foob.GetJobs();

                Dispatcher.Invoke(() =>
                {
                    displayClients(clients);
                    displayJobs(jobs);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void displayClients(List<Client> clients)
        {
            ClientsListBox.Items.Clear();

            foreach (Client client in clients)
            {
                string details = client.Username;
                ClientsListBox.Items.Add(details);
            }
        }

        private void displayJobs(List<Job> jobs)
        {
            JobResultsListBox.Items.Clear();

            foreach (Job job in jobs)
            {
                JobResultsListBox.Items.Add(job.Status);
            }
        }
    }
}
