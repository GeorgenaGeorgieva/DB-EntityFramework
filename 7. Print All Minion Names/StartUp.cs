using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _7._Print_All_Minion_Names
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
            List<string> minionNames = new List<string>();

            ЕxtractionMinionNamesFromDB(minionNames);

            PrintAllMinionNamesInSpecificOrder(minionNames);
        }

        private static void ЕxtractionMinionNamesFromDB(List<string> minionNames)
        {
            connection.Open();

            string queryFindMinionNames = "SELECT Name FROM Minions";

            SqlCommand commandFindMinionNames = new SqlCommand(queryFindMinionNames, connection);

            using (commandFindMinionNames)
            {
                SqlDataReader reader = commandFindMinionNames.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        minionNames.Add((string)reader["Name"]);
                    }
                }
            }
        }

        private static void PrintAllMinionNamesInSpecificOrder(List<string> minionNames)
        {
            for (int i = 0; i < minionNames.Count / 2; i++)
            {
                Console.WriteLine(minionNames[i]);
                Console.WriteLine(minionNames[minionNames.Count - i - 1]);
            }

            if (minionNames.Count % 2 != 0)
            {
                Console.WriteLine(minionNames[minionNames.Count / 2]);
            }
        }
    }
}
