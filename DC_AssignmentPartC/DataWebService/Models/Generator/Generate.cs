using DataWebService.Data;

namespace DataWebService.Models.Generator
{
    public class Generate
    {
        public static List<Client> clients = new List<Client>();
        public static List<Job> jobs = new List<Job>();

        public static void GenerateData()
        {
            for (int i = 0; i < 4; i++)
            {
                Client client = GenerateEntries.GenerateRandomClient();
                clients.Add(client);

                Job job = GenerateEntries.GenerateRandomJob();
                jobs.Add(job);
            }
        }

        public static void GenerateTables()
        {
            ClientManager.CreateTable();
            JobManager.CreateTable();
        }

        public static void InsertData()
        {
            GenerateData();

            foreach (Client client in clients)
            {
                ClientManager.Insert(client);
            } 
            foreach (Job job in jobs)
            {
                JobManager.Insert(job);
            }
        }
    }
}
