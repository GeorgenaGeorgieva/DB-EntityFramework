namespace AddMinion
{
    using System;
    using System.Data.SqlClient;
    
    class StartUp
    {
        private static string connectionString =
            "Server=.;" +
            "Database=MinionsDB;" +
            "Integrated Security=true";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            string[] minionInformation = Console.ReadLine()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string minionName = minionInformation[1];
            int minionAge = int.Parse(minionInformation[2]);
            string minionTown = minionInformation[3];


            string[] villainInformation = Console.ReadLine()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string villainName = villainInformation[1];

            connection.Open();

            string queryFindVillain = @"SELECT Id FROM Villains WHERE Name = @Name";
            string queryFindTown = @"SELECT Id FROM Towns WHERE Name = @Name";
            string queryFindMinion = @"SELECT Id FROM Minions WHERE Name = @Name";

            int? villainId = GetId(queryFindVillain, villainName);
            int? townId = GetId(queryFindTown, minionTown);
            int? minionId = GetId(queryFindMinion, minionName);


            if (villainId == null)
            {
                AddVillainToDatabaseIfItIsNotExist(villainName);
                villainId = GetId(queryFindVillain, villainName);
            }

            if (townId == null)
            {
                AddTownToDatabaseIfItIsNotExist(minionTown);
                townId = GetId(queryFindTown, minionTown);
            }

            if (minionId == null)
            {
                AddMinionToDatabaseIfItIsNotExist(minionName, minionAge, (int)townId);
                minionId = GetId(queryFindMinion, minionName);
            }

            MinionSubordination((int)villainId, (int)minionId, minionName, villainName);
        }

        private static int? GetId(string query, string Name)
        {
            SqlCommand command = new SqlCommand(query, connection);

            using (command)
            {
                command.Parameters.AddWithValue("@Name", Name);
                return (int?)command.ExecuteScalar();
            }
        }

        private static void AddVillainToDatabaseIfItIsNotExist(string villainName)
        {
            string queryAddVillainToDB = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
            SqlCommand addVillainCommand = new SqlCommand(queryAddVillainToDB, connection);

            using (addVillainCommand)
            {
                addVillainCommand.Parameters.AddWithValue("@villainName", villainName);
                addVillainCommand.ExecuteNonQuery();

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static void AddTownToDatabaseIfItIsNotExist(string minionTown)
        {
            string queryAddTownToDB = @"INSERT INTO Towns (Name) VALUES (@Name)";
            SqlCommand addTownToDB = new SqlCommand(queryAddTownToDB, connection);

            using (addTownToDB)
            {
                addTownToDB.Parameters.AddWithValue("@Name", minionTown);
                addTownToDB.ExecuteNonQuery();

                Console.WriteLine($"Town {minionTown} was added to the database.");
            }
        }

        private static void AddMinionToDatabaseIfItIsNotExist(string minionName, int minionAge, int townId)
        {
            string queryAddMinionToDB = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
            SqlCommand addMinionToDB = new SqlCommand(queryAddMinionToDB, connection);

            using (addMinionToDB)
            {
                addMinionToDB.Parameters.AddWithValue("@name", minionName);
                addMinionToDB.Parameters.AddWithValue("@age", minionAge);
                addMinionToDB.Parameters.AddWithValue("@townId", townId);
                addMinionToDB.ExecuteNonQuery();
            }
        }

        private static void MinionSubordination(int villainId, int minionId, string minionName, string villainName)
        {
            string querySubordination = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";
            SqlCommand subordinationCommand = new SqlCommand(querySubordination, connection);

            using (subordinationCommand)
            {
                subordinationCommand.Parameters.AddWithValue("@villainId", villainId);
                subordinationCommand.Parameters.AddWithValue("@minionId", minionId);

                int affectedRows = 0;

                try
                {
                    affectedRows = subordinationCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine($"{minionName} is already a slave of {villainName}");
                }

                if (affectedRows > 0)
                {
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }
            }
        }
    }
}

