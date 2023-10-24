using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Casino;
using Casino.TwentyOne;

namespace TwentyOne
{
    internal class Program
    {
        private static char card;

        static void Main(string[] args)
        {
            const string casinoName = "Ubi Hotel and Casino";

            

            Console.WriteLine("Welcome to the {0}! \n--------------------------- \nWhat is your name? ",casinoName);
            string playerName = Console.ReadLine();

            if (playerName.ToLower() == "admin") 
            {
                List<ExceptionEntity> Exceptions = ReadExceptions();
                foreach (var exception in Exceptions) 
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine("\n");
                }
                Console.ReadLine();
                return;
            }

            bool validAnswer = false;
            int bank = 0;
            while (!validAnswer)
            {
                Console.WriteLine("How much money are you gambling with today?");
                validAnswer = int.TryParse(Console.ReadLine(), out bank);
                if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals!");
            }

            Console.WriteLine("---------------------------\nHello, {0}! Would you like to play a game of Blackjack?", playerName);
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" ||  answer == "yeah" || answer == "ya" || answer == "y" || answer == "true" || answer == "si") 
            {
                Console.WriteLine("\n\n");
                Player player = new Player(playerName, bank);
                player.Id = Guid.NewGuid();
                using (StreamWriter file = new StreamWriter(@"C:\Users\keato\Documents\GitHub\The-Tech-Academy-Basic-C-Sharp-Projects\basic_C#_programs\TwentyOne\Logs\log.txt", true))
                {
                    file.WriteLine(player.Id);
                    file.WriteLine(DateTime.Now);
                    file.WriteLine(card);
                }
                Game game = new TwentyOneGame();
                game += player;
                player.isActivePlaying = true;

                while (player.isActivePlaying && player.Balance > 0) 
                {
                    try
                    {
                        game.Play();
                    }
                    catch (FraudException ex)
                    {
                        Console.WriteLine(ex.Message);
                        UpdateDbWithException(ex);
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error has occured. Please contact your Systems Admin.");
                        UpdateDbWithException(ex);
                        Console.ReadLine();
                        return;
                    }
                }
                game -= player;
                Console.WriteLine("Thank you for playing!");
            }

            Console.WriteLine("---------------------------\n");
            Console.WriteLine("Bye.");

            Console.ReadLine();
        }
        private static void UpdateDbWithException(Exception ex)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TwentyOneGame;" +
                                        "Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
                                        "TrustServerCertificate=False;ApplicationIntent=ReadWrite;" +
                                        "MultiSubnetFailover=False";

            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES
                                    (@ExceptionType, @ExceptionMessage, @TimeStamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();
                command.Parameters["@ExceptionMessage"].Value = ex.Message;
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private static List<ExceptionEntity> ReadExceptions()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TwentyOneGame;" +
                                        "Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
                                        "TrustServerCertificate=False;ApplicationIntent=ReadWrite;" +
                                        "MultiSubnetFailover=False";

            string querryString = @"Select Id, ExceptionType, ExceptionMessage, TimeStamp From Exceptions";

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();

            using (SqlConnection connection = new SqlConnection( connectionString))
            {
                SqlCommand command = new SqlCommand(querryString, connection);


                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ExceptionEntity exception = new ExceptionEntity();
                    exception.Id = Convert.ToInt32(reader["id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"].ToString());

                    Exceptions.Add(exception);
                }
                connection.Close();
            }
            return Exceptions;
        }
    }
}