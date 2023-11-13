using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DLL;

namespace ClientServer
{
    [ServiceContract]
    public interface ClientServerInterface
    {
        [OperationContract]
        List<Client> GetClients();
        [OperationContract]
        List<Job> GetJobs();
        [OperationContract]
        Boolean CreateClient(Client newClient);
        [OperationContract]
        Boolean DeleteClient(string username);
        [OperationContract]
        Client GetClientByUsername(string username);
        [OperationContract]
        Boolean CreateJob(Job newJob);
        [OperationContract]
        void CallConsole(string username, int port);
        [OperationContract]
        void UploadJob(string code);

    }
}
