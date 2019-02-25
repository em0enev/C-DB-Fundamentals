namespace _02.VillainNames
{
    using InitialSetup;
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(Configuration.connectionString);

            using (connection)
            {
                connection.Open();
                connection.ChangeDatabase("MinionsDB");

                string villainsInfo = @"SELECT v.Name, count(MinionId) as MinionsCount FROM Villains as v JOIN MinionsVillains as mv on v.Id = mv.VillainId GROUP BY v.Name HAVING count(MinionId) >= 3 ORDER BY MinionsCount DESC";

                SqlCommand command = new SqlCommand(villainsInfo, connection);
                using (command) 
                {
                    SqlDataReader reader = command.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        { 
                            string name =(string)reader[0];
                            int numberOfMinions =(int)reader[1];
                            Console.WriteLine($"{name} - {numberOfMinions}");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
