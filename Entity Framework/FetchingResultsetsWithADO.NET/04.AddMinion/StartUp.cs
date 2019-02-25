namespace _04.AddMinion
{
    using _04.AddMinion.IO;
    using InitialSetup;
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            string[] minionsInfo = ConsoleReader.ReadLine().Split();
            string[] villainInfo = ConsoleReader.ReadLine().Split();

            string minionName = minionsInfo[1];
            int minionAge = int.Parse(minionsInfo[2]);
            string townName = minionsInfo[3];

            string villainName = villainInfo[1];

            var connection = new SqlConnection(Configuration.connectionString);

            using (connection)
            {
                connection.Open();

                int townId = GetTownId(townName, connection);
                int villainId = GetVillainId(villainName, connection);
                int minionId = InsertMinionAndGetId(minionName, minionAge, townId, connection);

                AssignMinionToVillain(minionId, villainId, connection);
                ConsoleWriter.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                connection.Close();
            }
        }

        private static void AssignMinionToVillain(int minionId, int villainId, SqlConnection connection)
        {
            string insertMinionsVillainSql = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @VillainId)";

            using (SqlCommand command = new SqlCommand(insertMinionsVillainSql, connection))
            {
                command.Parameters.AddWithValue("@MinionId", minionId);
                command.Parameters.AddWithValue("@VillainId", villainId);

                command.ExecuteNonQuery();         
            }
        }

        private static int InsertMinionAndGetId(string minionName, int minionAge, int townId, SqlConnection connection)
        {
            string insertMinionSql = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            var command = new SqlCommand(insertMinionSql, connection);

            using (command)
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", minionAge);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }

            string getMinionIdSql = "SELECT Id FROM minions WHERE Name = @name";

            using (SqlCommand sqlCommand = new SqlCommand(getMinionIdSql, connection))
            {
                sqlCommand.Parameters.AddWithValue("@name", minionName);

                return  (int)sqlCommand.ExecuteScalar();
            }


        }

        private static int GetVillainId(string villainName, SqlConnection connection)
        {
            string villainSql = @"SELECT Id FROM Villains WHERE Name = @name";

            var command = new SqlCommand(villainSql, connection);
            using (command)
            {
                command.Parameters.AddWithValue("@name", villainName);
                if (command.ExecuteScalar() == null)
                {
                    InsertIntoVillains(villainName, connection);
                }

                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertIntoVillains(string villainName, SqlConnection connection)
        {
            string insertVillain = "INSERT INTO Villains (Name) VALUES (@villainName)";

            var command = new SqlCommand(insertVillain, connection);
            using (command)
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                command.ExecuteNonQuery();
                ConsoleWriter.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static int GetTownId(string townName, SqlConnection connection)
        {
            string townSql = @"SELECT Id FROM Towns WHERE Name = @name";

            var command = new SqlCommand(townSql, connection);
            using (command)
            {
                command.Parameters.AddWithValue("@name", townName);
                if (command.ExecuteScalar() == null)
                {
                    InsertIntoTownsNewTown(townName, connection);
                }

                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertIntoTownsNewTown(string townName, SqlConnection connection)
        {
            string insertTown = "INSERT INTO Towns (Name) VALUES (@townName)";

            var command = new SqlCommand(insertTown, connection);
            using (command)
            {
                command.Parameters.AddWithValue("@townName", townName);
                command.ExecuteNonQuery();
                ConsoleWriter.WriteLine($"Town {townName} was added to the database.");
            }
        }
    }
}
