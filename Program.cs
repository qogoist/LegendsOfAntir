using System;
using System.IO;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Program
    {
        public static Game _game;

        static void Main(string[] args)
        {
            ShowWelcome();
            ShowMenu();

            bool running = true;
            while (running)
            {
                running = GetChoice();
            }

            _game.GameLoop();

        }

        static void ShowMenu()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("1. Play Game");
            Console.WriteLine("2. Exit");
        }

        static void ShowWelcome()
        {
            Console.WriteLine("Welcome to Legends of Antir.");
        }

        static bool GetChoice()
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _game = LoadGameFile();
                    return false;

                case "2":
                    Console.Write("Closing the Game.");
                    return false;

                default:
                    Console.WriteLine("Unknown command, please try again.");
                    return true;
            }
        }

        static Game LoadGameFile()
        {
            string jsonString = File.ReadAllText("Ressources\\game.json");
            Game game = JsonConvert.DeserializeObject<Game>(jsonString, new JsonSerializerSettings {PreserveReferencesHandling = PreserveReferencesHandling.All});
            return game;
        }

    }
}
