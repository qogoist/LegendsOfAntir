using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LegendsOfAntir
{
    class Program
    {
        public static Game _game;

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
            bool running = true;
            do
            {
                ShowMenu();
                running = GetChoice();
                _game.GameLoop();
            } while (running);
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
                        _game = LoadGameFile("Ressources\\game.json");
                        CreateCharacter();
                        _game.AddDeathListener();
                        choosing = false;
                        break;

                    case "2":
                        _game = LoadGameFile("Ressources\\save.json");
                        _game.AddDeathListener();
                        choosing = false;
                        break;

                    case "3":
                        Console.Write("Closing the Game.");
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
            try
            {
                Player player = _game.player;
                Console.WriteLine("What is your name?");
                Console.Write("> ");

                string input = Console.ReadLine();
                player.name = input;

                Console.WriteLine("How do you look?");
                Console.Write("> ");

                input = Console.ReadLine();
                player.description = input;

                Console.WriteLine("Order these attributes by importance, from most important to least.");
                Console.WriteLine("Agility, Brawn, Smarts");
                Console.Write("> ");

                string[] inputArray = Console.ReadLine().ToLower().Split(",");
                if (inputArray.Length != 3)
                    throw new Exception();

                Attribute first = (Attribute)Enum.Parse(typeof(Attribute), inputArray[0].Trim(), true);
                Attribute second = (Attribute)Enum.Parse(typeof(Attribute), inputArray[1].Trim(), true);
                Attribute third = (Attribute)Enum.Parse(typeof(Attribute), inputArray[2].Trim(), true);

                player.attributes.Add(first, 5);
                player.attributes.Add(second, 3);
                player.attributes.Add(third, 1);

                player.hp = player.attributes[Attribute.Brawn] * 5;

                foreach (Room room in _game.rooms)
                {
                    if (room.characters.Contains(player))
                        player.currentRoom = room;
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Something went wrong. Please try again.");
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
            string jsonString = JsonConvert.SerializeObject(_game, Formatting.Indented, _jsonSettings);
            File.WriteAllText("Ressources\\save.json", jsonString);
        }

    }
}
