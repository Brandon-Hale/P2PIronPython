using DataWebService.Models;
using System.Data.SQLite;

namespace DataWebService.Data
{
    public class JobManager
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
                    CREATE TABLE Job (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Status TEXT,
                        Code TEXT,
                        Result TEXT
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

        public static bool Insert(Job job)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO Job (Status, Code, Result)
                        VALUES (@Status, @Code, @Result)";

                        command.Parameters.AddWithValue("@Status", job.Status);
                        command.Parameters.AddWithValue("@Code", job.Code);
                        command.Parameters.AddWithValue("@Result", job.Result);

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

        public static bool Delete(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM Job WHERE ID = @ID";
                        command.Parameters.AddWithValue("@ID", id);

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

        public static bool Update(Job job)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"UPDATE Job SET Status = @Status, Code = @Code, Result = @Result WHERE ID = @ID";

                        command.Parameters.AddWithValue("@ID", job.Id);
                        command.Parameters.AddWithValue("@Status", job.Status);
                        command.Parameters.AddWithValue("@Code", job.Code);
                        command.Parameters.AddWithValue("@Result", job.Result);

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

        public static List<Job> GetAll()
        {
            List<Job> jobs = new List<Job>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Job";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Job job = new Job();
                                job.Id = Convert.ToInt32(reader["ID"]);
                                job.Status = reader["Status"].ToString();
                                job.Code = reader["Code"].ToString();
                                job.Result = reader["Result"].ToString();

                                jobs.Add(job);
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

            return jobs;
        }

        public static Job GetByID(int id)
        {
            Job job = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Job WHERE ID = @ID";
                        command.Parameters.AddWithValue("@ID", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                job.Id = Convert.ToInt32(reader["ID"]);
                                job.Status = reader["Status"].ToString();
                                job.Code = reader["Code"].ToString();
                                job.Result = reader["Result"].ToString();
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

            return job;
        }
    }
}