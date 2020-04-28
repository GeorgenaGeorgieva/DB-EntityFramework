namespace IncreaseMinionAge
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    class StartUp
    {
        private static string connectionString = "" +
            "Server=.;" +
            "Database=MinionsDB;" +
            "Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            connection.Open();

            int[] inputMinionIds = Console.ReadLine()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();

            foreach (var id in inputMinionIds)
            {
                UpdateMinionsTableFromDB(id);
            }

            PrintingAllMinionsFromDatabase();
        }

        private static void UpdateMinionsTableFromDB(int minionId)
        {
            string queryUpdateNameAndAge = @"UPDATE Minions
                                             SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                             WHERE Id = @Id";

            SqlCommand commandUpdateNameAndAge = new SqlCommand(queryUpdateNameAndAge, connection);

            using (commandUpdateNameAndAge)
            {
                commandUpdateNameAndAge.Parameters.AddWithValue("@Id", minionId);
                int affectedRows = commandUpdateNameAndAge.ExecuteNonQuery();

                Console.WriteLine($"({affectedRows} rows affected)");
            }
        }

        private static void PrintingAllMinionsFromDatabase()
        {
            string queryFindAllMinions = "SELECT Name, Age FROM Minions";

            SqlCommand commandFindAllMinions = new SqlCommand(queryFindAllMinions, connection);

            using (commandFindAllMinions)
            {
                SqlDataReader reader = commandFindAllMinions.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
                    }
                }
            }
        }
    }
}
