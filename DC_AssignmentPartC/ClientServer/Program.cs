using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ClientServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Initialising System...");

                ServiceHost host;
                NetTcpBinding tcp = new NetTcpBinding();

                host = new ServiceHost(typeof(ClientServerImplementation));
                host.AddServiceEndpoint(typeof(ClientServerInterface), tcp, "net.tcp://0.0.0.0:400/ClientServer");
                host.Open();
                Console.WriteLine("System Online");
                Console.ReadLine();
                host.Close();
            }
            else
            {
                Console.WriteLine($"Initialising System for Client {args[0]}, {args[1]}");

                ServiceHost host;
                NetTcpBinding tcp = new NetTcpBinding();
                host = new ServiceHost(typeof(ClientServerImplementation));
                host.AddServiceEndpoint(typeof(ClientServerInterface), tcp, $"net.tcp://0.0.0.0:{args[0]}/ClientApplication");
                host.Open();
                Console.WriteLine("System Online");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
