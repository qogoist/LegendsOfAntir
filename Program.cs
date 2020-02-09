using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LegendsOfAntir
{
    class Program
    {
        public static Game Game;

        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ObjectCreationHandling = ObjectCreationHandling.Auto
        };

        private static void Main(string[] args)
        {
            ShowWelcome();
            bool menu = true;
            do
            {
                ShowMenu();
                menu = GetChoice();
                Game.Stop = false;
                Game.GameLoop();
            } while (menu);
        }

        static void ShowMenu()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. Exit");
        }

        static void ShowWelcome()
        {
            Console.WriteLine("Welcome to Legends of Antir.");
        }

        static bool GetChoice()
        {
            bool choosing = true;
            while (choosing)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Game = LoadGameFile("Ressources\\game.json");
                        CreateCharacter();
                        Game.AddDeathListener();
                        choosing = false;
                        break;

                    case "2":
                        try
                        {
                            Game = LoadGameFile("Ressources\\save.json");
                            Game.AddDeathListener();
                            choosing = false;
                        }
                        catch (System.Exception)
                        {
                            Console.WriteLine("No saved game found. Please start a new game.");
                        }
                        break;

                    case "3":
                        Console.Write("Closing the Game.");
                        Environment.Exit(0);
                        return false;

                    default:
                        Console.WriteLine("Unknown command, please try again.");
                        choosing = true;
                        break;
                }
            }

            return true;
        }

        static void CreateCharacter()
        {
            bool creating = true;

            while (creating)
            {
                try
                {
                    Player player = Game.Player;
                    Console.WriteLine("What is your name?");
                    Console.Write("> ");

                    string input = Console.ReadLine();
                    player.Name = input;

                    Console.WriteLine("Order these attributes by importance, from most important to least.");
                    Console.WriteLine("Agility, Brawn, Smarts");
                    Console.Write("> ");

                    string[] inputArray = Console.ReadLine().ToLower().Split(",");
                    if (inputArray.Length != 3)
                        throw new Exception();

                    Attribute first = (Attribute)Enum.Parse(typeof(Attribute), inputArray[0].Trim(), true);
                    Attribute second = (Attribute)Enum.Parse(typeof(Attribute), inputArray[1].Trim(), true);
                    Attribute third = (Attribute)Enum.Parse(typeof(Attribute), inputArray[2].Trim(), true);

                    player.Attributes.Add(first, 5);
                    player.Attributes.Add(second, 3);
                    player.Attributes.Add(third, 1);

                    player.Hp = player.Attributes[Attribute.Brawn] * 5;

                    foreach (Room room in Game.Rooms)
                    {
                        if (room.Characters.Contains(player))
                            player.CurrentRoom = room;
                    }

                    creating = false;
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Something went wrong. Please try again.");
                }
            }
        }

        static Game LoadGameFile(string path)
        {
            string jsonString = File.ReadAllText(path);
            Game game = JsonConvert.DeserializeObject<Game>(jsonString, _jsonSettings);
            return game;
        }

        public static void SaveGameFIle()
        {
            string jsonString = JsonConvert.SerializeObject(Game, Formatting.Indented, _jsonSettings);
            File.WriteAllText("Ressources\\save.json", jsonString);
        }
    }
}
