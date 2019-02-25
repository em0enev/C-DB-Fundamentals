namespace _03.MinionNames
{

    using InitialSetup;
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(Configuration.connectionString);
            int villainId = int.Parse(Console.ReadLine());

            using (connection)
            {
                connection.Open();
                connection.ChangeDatabase("MinionsDB");

                string villainName = GetVillainName(villainId, connection);

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {villainName}");
                    PrintNames(villainId, connection);
                }

                connection.Close();
            }
        }

        private static void PrintNames(int villainId, SqlConnection connection)
        {
            string minionsSql = @"Select name, age from Minions as m JOIN MinionsVillains as mv on mv.MinionId = m.Id where mv.VillainId = @Id";

            SqlCommand command = new SqlCommand(minionsSql, connection);
            command.Parameters.AddWithValue("@Id", villainId);

            using (command)
            {
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                    }
                    else
                    {
                        int row = 1;
                        while (reader.Read())
                        {
                            Console.WriteLine($"{row++}. {reader[0]} {reader[1]}");
                        }
                    }
                }
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection)
        {
            string nameSql = @"SELECT * FROM Villains where id = @id";

            SqlCommand command = new SqlCommand(nameSql, connection);

            using (command)
            {
                command.Parameters.AddWithValue("@id", villainId);
                if (command.ExecuteScalar() != null)
                {
                    return command.ExecuteScalar().ToString();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
