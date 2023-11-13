namespace DLL
{
    public class Client
    {

        public string Username { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }

        public Client() 
        {
            Username = "Defaut";
            IPAddress = "0.0.0.0";
            Port = 0;
        }
    }
}
