using InitialSetup;
using System;
using System.Data.SqlClient;

namespace _06.RemoveVillain
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection(Configuration.connectionString);

            int id = int.Parse(Console.ReadLine());
            using (connection)
            {
                connection.Open();

                string villainQuery = @"SELECT Name From Villains WHERE Id = @villainId";

                SqlCommand command = new SqlCommand(villainQuery, connection);
                string villainName;
                using (command)
                {
                    command.Parameters.AddWithValue("@villainId", id);
                     villainName = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found");
                        return;
                    }
                }

                int affectedRows = DeleteMinionsVillainsById(id, connection);
                DeleteVillainById(connection, id);

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{affectedRows} minions were released");
            }
        }

        private static void DeleteVillainById(SqlConnection connection, int id)
        {
            string deleteSql = @"DELETE FROM Villains WHERE Id = @villainID";
            SqlCommand command = new SqlCommand(deleteSql, connection);

            using (command)
            {
                command.Parameters.AddWithValue("@villainID", id);
                command.ExecuteScalar();
            }

        }

        private static int DeleteMinionsVillainsById(int id, SqlConnection connection)
        {
            string deleteSql = @"DELETE FROM MinionsVillains WHERE VillainId = @villainID";
            SqlCommand command = new SqlCommand(deleteSql, connection);

            using (command)
            {
                command.Parameters.AddWithValue("@villainId", id);
                return command.ExecuteNonQuery();
            }

        }
    }
}
