using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace PlayGround
{
    public static class DataTableExtensions
    {
        public static string ToJson(this DataTable table)
        {
            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }
    }
    internal class DBContext
    {
        private string _connectionString { get; init; }

        public DBContext(string conn)
        {
            if(string.IsNullOrEmpty(conn)) throw new ArgumentNullException(nameof(conn));
            _connectionString = conn;
        }

        public DataTable MakeQuery(string queryStr, SqlParameter[] parameters)
        {
            DataTable output = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(output);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                        // Further error handling
                    }
                }
            }
            return output;
        }

        public DataTable MakeQuery(string queryStr)
        {
            DataTable output = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(output);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                        // Handle the exception as needed
                    }
                }
            }

            return output;
        }

        public int ExecuteNonQuery(string queryStr, SqlParameter[] parameters)
        {
            int affectedRows = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                        // Consider more detailed exception handling or logging
                    }
                }
            }
            return affectedRows;
        }

        public int ExecuteNonQuery(string queryStr)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                        // Handle the exception as needed
                    }
                }
            }

            return affectedRows;
        }
    }
}
