using Microsoft.AspNetCore.Http.Connections;
using System.Data.SQLite;
using DataWebService.Models;

namespace DataWebService.Data
{
    public class ClientManager
    {
        private static string connectionString = "Data Source=mydatabase.db; Version=3";

        public static bool CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        CREATE TABLE Client (
                            Username TEXT PRIMARY KEY,
                            IPAddress TEXT,
                            Port INTEGER
                        )";
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Table create Successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool Insert(Client client)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                    INSERT INTO Client (Username, IPAddress, Port)
                    VALUES (@Username, @IPAddress, @Port)";

                        command.Parameters.AddWithValue("@Username", client.Username);
                        command.Parameters.AddWithValue("@IPAddress", client.IPAddress);
                        command.Parameters.AddWithValue("@Port", client.Port);

                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool DeleteByName(string username)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM Client WHERE Username = @Username";
                        command.Parameters.AddWithValue("@Username", username);

                        int rowsDeleted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false; //nothing deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteByIP(string ip)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM Client WHERE IPAddress = @IPAddress";
                        command.Parameters.AddWithValue("@IPAddress", ip);

                        int rowsDeleted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false; //nothing deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static bool Update(Client client)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"UPDATE Client SET IPAddress = @IPAddress, Port = @Port WHERE Username = @Username";

                        command.Parameters.AddWithValue("@Username", client.Username);
                        command.Parameters.AddWithValue("@IPAddress", client.IPAddress);
                        command.Parameters.AddWithValue("@Port", client.Port);

                        int rowsUpdated = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false; //no rows updated

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; //failed
            }
        }

        public static List<Client> GetAll()
        {
            List<Client> clientList = new List<Client>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Client";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Client client = new Client();
                                client.Username = reader["Username"].ToString();
                                client.IPAddress = reader["IPAddress"].ToString();
                                client.Port = Convert.ToInt32(reader["Port"]);

                                clientList.Add(client);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return clientList;
        }

        public static Client GetByUsername(string username)
        {
            Client client = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Client WHERE Username = @Username";
                        command.Parameters.AddWithValue("@Username", username);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                client = new Client
                                {
                                    Username = reader["Username"].ToString(),
                                    IPAddress = reader["IPAddress"].ToString(),
                                    Port = Convert.ToInt32(reader["Port"])
                                };
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return client;
        }

        public static Client GetByAddress(string address)
        {
            Client client = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Client WHERE IPAddress = @IPAddress";
                        command.Parameters.AddWithValue("@IPAddress", address);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                client = new Client
                                {
                                    Username = reader["Username"].ToString(),
                                    IPAddress = reader["IPAddress"].ToString(),
                                    Port = Convert.ToInt32(reader["Port"])
                                };
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return client;
        }
    }
}
