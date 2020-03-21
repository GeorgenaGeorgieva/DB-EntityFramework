using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _5._Change_Town_Names_Casing
{
    class StartUp
    {
        private static string stringConnection =
            "Server=GEORGENA-PC\\SQLEXPRESS;" +
            "Database=MinionsDB;" +
            "Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(stringConnection);

        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            connection.Open();

            List<string> towns = GetTowns(countryName);

            if (towns.Count > 0)
            {
                int countryId = GetCurrentCountryId(countryName);
                UpdateTownNamesToUpperCase(countryId);

                Console.WriteLine($"[{string.Join(", ", towns).ToUpper()}]");
                Console.WriteLine($"{towns.Count} town names were affected.");
            }
            else
            {
                Console.WriteLine("No town names were affected.");
            }
        }

        private static void UpdateTownNamesToUpperCase(int countryId)
        {
            string queryUpdate = @"UPDATE Towns
                                   SET Name = UPPER(Name)
                                   WHERE CountryCode = @countryId";

            SqlCommand commandUpdateTowns = new SqlCommand(queryUpdate, connection);

            using (commandUpdateTowns)
            {
                commandUpdateTowns.Parameters.AddWithValue("@countryId", countryId);
                commandUpdateTowns.ExecuteNonQuery();
            }
        }

        private static int GetCurrentCountryId(string countryName)
        {
            string queryGetCountryId = "SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName";

            SqlCommand commandGetCountryId = new SqlCommand(queryGetCountryId, connection);

            using (commandGetCountryId)
            {
                commandGetCountryId.Parameters.AddWithValue("@countryName", countryName);
                return (int)commandGetCountryId.ExecuteScalar();
            }
        }

        private static List<string> GetTowns(string countryName)
        {
            string queryTowns = @" SELECT t.Name 
                                   FROM Towns as t
                                   JOIN Countries AS c ON c.Id = t.CountryCode
                                   WHERE c.Name = @countryName";

            SqlCommand commandGetTowns = new SqlCommand(queryTowns, connection);

            using (commandGetTowns)
            {
                List<string> towns = new List<string>();

                commandGetTowns.Parameters.AddWithValue("@countryName", countryName);
                SqlDataReader readTowns = commandGetTowns.ExecuteReader();

                using (readTowns)
                {
                    while (readTowns.Read())
                    {
                        towns.Add((string)readTowns["Name"]);
                    }

                    readTowns.Close();
                }

                return towns;
            }
        }
    }
}
