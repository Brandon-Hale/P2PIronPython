using System;

namespace DataWebService.Models.Generator
{
    public static class GenerateEntries
    {
        private static readonly Random _r = new Random();

        public static Client GenerateRandomClient()
        {
            return new Client
            {
                Username = GenerateRandomUsername(),
                IPAddress = GenerateRandomIP(),
                Port = GenerateRandomPort()
            };
        }

        public static Job GenerateRandomJob()
        {
            return new Job
            {
                Id = GenerateRandomID(),
                Status = GenerateRandomStatus(),
                Code = "print(Hello world)",
                Result = "Hello world"
            };
        }

        private static string GenerateRandomUsername()
        {
            return _r.Next(1000, 9999).ToString();
        }

        private static string GenerateRandomIP()
        {
            int p1 = _r.Next(10, 99);
            int p2 = _r.Next(10, 99);
            int p3 = _r.Next(10, 99);
            int p4 = _r.Next(10, 99);
            string ip = p1 + "." + p2 + "." + p3 + "." + p4;
            return ip;
        }

        private static int GenerateRandomPort()
        {
            return _r.Next(10, 200);
        }

        private static int IdCounter = 1000;
        private static int GenerateRandomID()
        {
            return IdCounter++; // 4-digit random transaction ID
        }

        private static string GenerateRandomStatus()
        {
            string[] status = { "Completed", "In Progress", "Available", };
            return status[_r.Next(status.Length)];
        }
    }
}
