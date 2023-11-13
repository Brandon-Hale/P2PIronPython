using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.CompilerServices;
using DLL;
using RestSharp;
using Newtonsoft.Json;
using System.Net;

namespace ClientServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ClientServerImplementation : ClientServerInterface
    {
        private uint LogNumber = 0;
        private static List<Client> clientList = new List<Client>();
        private static List<Job> jobList = new List<Job>();
        RestClient restClient = new RestClient("http://localhost:5164/api");
        List<String> jobs = new List<String>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(string logString)
        {
            LogNumber++;
            string logMessage = $"Log {LogNumber}: {logString}";
            Console.WriteLine(logMessage);
        }

        public List<Client> GetClients()
        {
            RestRequest restRequest = new RestRequest($"client/getall");
            RestResponse restResponse = restClient.Get(restRequest);
            clientList = JsonConvert.DeserializeObject<List<Client>>(restResponse.Content);

            return clientList;
        }

        public List<Job> GetJobs()
        {
            RestRequest restRequest = new RestRequest($"job/getall");
            RestResponse restResponse = restClient.Get(restRequest);
            jobList = JsonConvert.DeserializeObject<List<Job>>(restResponse.Content);

            return jobList;

        }

        public Client GetClientByUsername(string username)
        {
            Client getclient = null;
            foreach (Client client in clientList)
            {
                if (client.Username.Equals(username))
                {
                    RestRequest restRequest = new RestRequest($"client/username/{username}", Method.Get);
                    RestResponse restResponse = restClient.Get(restRequest);
                    getclient = JsonConvert.DeserializeObject<Client>(restResponse.Content);           
                }
            }
            return getclient;
        }

        public Boolean CreateClient(Client newClient)
        {
            Boolean created = true;

            foreach (Client client in clientList)
            {
                if (client.Username.Equals(newClient.Username))
                {
                    created = false;
                }
            }

            if (created == true) 
            {
                RestRequest restRequest = new RestRequest($"client/post", Method.Post);
                restRequest.AddJsonBody(newClient);
                RestResponse restResponse = restClient.Post(restRequest);

                return created;
            }

            return created;
        }

        public Boolean DeleteClient(string username)
        {
            Boolean deleted = true;

            foreach (Client client in clientList)
            {
                if (client.Username.Equals(username))
                {
                    RestRequest restRequest = new RestRequest($"client/deletename/{username}", Method.Delete);
                    RestResponse restResponse = restClient.Delete(restRequest);
                    return deleted;
                }
            }
            return false;

        }

        public Boolean CreateJob(Job newJob)
        {
            Boolean created = true;

            foreach (Job job in jobList)
            {
                if (job.Id == newJob.Id)
                {
                    created = false;
                }
            }

            if (created == true)
            {
                RestRequest restRequest = new RestRequest($"job/post", Method.Post);
                restRequest.AddJsonBody(newJob);
                RestResponse restResponse = restClient.Post(restRequest);

                return created;
            }

            return created;
        }

        public void CallConsole(string username, int port)
        {
            Console.WriteLine($"Connected to client: Client: {username} + Port: {port}");
        }

        public void UploadJob(string code)
        {
            jobs.Add(code);
            Console.WriteLine(code);
        }


    }
}
