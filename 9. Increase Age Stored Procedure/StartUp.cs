using System;
using System.Data.SqlClient;
using System.Linq;

namespace _9._Increase_Age_Stored_Procedure
{
    class StartUp
    {
        private static string connectionString = "" +
            "Server=GEORGENA-PC\\SQLEXPRESS;" +
            "Database=MinionsDB;" +
            "Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            connection.Open();

            int[] inputIds = Console.ReadLine()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            foreach (var id in inputIds)
            {
                UpdateMinionAge(id);
            }

            foreach (var id in inputIds)
            {
                PrintingCurrentMinion(id);
            }
        }

        private static void PrintingCurrentMinion(int id)
        {
            string queryFindMinion = "SELECT Name, Age FROM Minions WHERE Id = @Id";

            SqlCommand commandFindMinion = new SqlCommand(queryFindMinion, connection);

            using (commandFindMinion)
            {
                commandFindMinion.Parameters.AddWithValue("Id", id);
                SqlDataReader reader = commandFindMinion.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} – {reader["Age"]} years old");
                    }
                }
            }
        }

        private static void UpdateMinionAge(int id)
        {
            string queryExecuteProcedure = @"EXEC usp_GetOlder @id";

            SqlCommand commandExecuteProcedure = new SqlCommand(queryExecuteProcedure, connection);

            using (commandExecuteProcedure)
            {
                commandExecuteProcedure.Parameters.AddWithValue("id", id);
                int affectedRows = commandExecuteProcedure.ExecuteNonQuery();

                Console.WriteLine($"({affectedRows} rows affected)");
            }
        }
    }
}
