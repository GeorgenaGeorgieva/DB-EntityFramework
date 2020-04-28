namespace VillainNames
{
    using System;
    using System.Data.SqlClient;
    
    class StartUp
    {
        private static string connectionString =
            "Server=.;" +
            "Database=MinionsDB;" +
            "Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            connection.Open();

            using (connection)
            {
                string queryText = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                     FROM Villains AS v 
                                     JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                     GROUP BY v.Id, v.Name 
                                     HAVING COUNT(mv.VillainId) > 3 
                                     ORDER BY COUNT(mv.VillainId)";

                SqlCommand command = new SqlCommand(queryText, connection);

                SqlDataReader readResult = command.ExecuteReader();

                using (readResult)
                {
                    while (readResult.Read())
                    {
                        Console.WriteLine($"{readResult["Name"]} - {readResult["MinionsCount"]}");
                    }
                }
            }
        }
    }
}
