using System;
using System.Data.SqlClient;

namespace _3._Minion_Names
{
    class StartUp
    {
        private static string connectionString =
           "Server=GEORGENA-PC\\SQLEXPRESS;" +
           "Database=MinionsDB;" +
           "Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            connection.Open();

            int villainId = int.Parse(Console.ReadLine());

            string queryVillainsName = $@"SELECT Name FROM Villains WHERE Id = {villainId}";

            SqlCommand command = new SqlCommand(queryVillainsName, connection);

            using (command)
            {

                string villainName = (string)command.ExecuteScalar();

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                    return;
                }

                Console.WriteLine($"Villain: {villainName}");
            }

            string queryMinions = $@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                       FROM MinionsVillains AS mv
                                       JOIN Minions As m ON mv.MinionId = m.Id
                                       WHERE mv.VillainId = {villainId}
                                       ORDER BY m.Name";

            command = new SqlCommand(queryMinions, connection);

            using (command)
            {
                SqlDataReader readMinions = command.ExecuteReader();

   
                using (readMinions)
                {
                    if (!readMinions.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                        return;
                    }

                    while (readMinions.Read())
                    {
                        Console.WriteLine($"{readMinions["RowNum"]}. {readMinions["Name"]} {readMinions["Age"]}");
                    }
                }
            }
        }
    }
}
