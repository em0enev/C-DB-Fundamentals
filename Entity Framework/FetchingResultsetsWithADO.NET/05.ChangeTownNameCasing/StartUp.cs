namespace _05.ChangeTownNameCasing
{
    using InitialSetup;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    class StartUp
    {
        static void Main(string[] args)
        {
            string nameOfCountry = Console.ReadLine();

            var connection = new SqlConnection(Configuration.connectionString);

            using (connection)
            {
                connection.Open();

                int? countryId = GetCountryId(nameOfCountry, connection);

                if (countryId == null)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    List<string> townNames = GetTownNames(countryId, connection);
                    Console.WriteLine($"{townNames.Count} town names were affected. ");
                    Console.WriteLine(string.Join(", ",townNames));
                }
               

                connection.Close();
            }
        }

        private static List<string> GetTownNames(int? countryId, SqlConnection connection)
        {
            var townNames = new List<string>();

            string townsSql = @"SELECT Name FROM Towns WHERE CountryCode = @id";
            var command = new SqlCommand(townsSql, connection);

            using (command)
            {
                command.Parameters.AddWithValue("@id", countryId);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            string townName = (string)reader[0].ToString().ToUpper();
                            townNames.Add(townName);
                        }
                    }
                }
                return townNames;
            }
        }

        private static int? GetCountryId(string nameOfCountry, SqlConnection connection)
        {
            string countrySql = @"SELECT Id FROM Countries WHERE Name = @name";

            var command = new SqlCommand(countrySql, connection);
            using (command)
            {
                command.Parameters.AddWithValue("@name", nameOfCountry);

                return (int?)command.ExecuteScalar();
            }
        }
    }
}
