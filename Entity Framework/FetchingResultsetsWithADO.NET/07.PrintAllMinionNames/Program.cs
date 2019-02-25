using InitialSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _07.PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection(Configuration.connectionString);
            using (connection)
            {
                connection.Open();
                List<string> names = GetNames(connection);

                PrintNames(names);

                connection.Close();
            }

        }

        private static void PrintNames(List<string> names)
        {
            for (int i = 0; i < names.Count / 2; i++)
            {
                Console.WriteLine($"{i + 1}. {names[i]} ");
                Console.WriteLine($"{names.Count - i}. {names[names.Count - 1 - i]}");
            }
            if (names.Count % 2 != 0 )
            {
                Console.WriteLine(names[names.Count /2]);
            }
        }

        private static List<string> GetNames(SqlConnection connection)
        {
            string namesSql = @"SELECT Name FROM Minions";
            List<string> names = new List<string>();

            var command = new SqlCommand(namesSql, connection);

            using (command)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        names.Add((string)reader[0]);
                    }
                }
            }
            return names;
        }
    }
}
