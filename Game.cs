using System;
using System.Linq;
using System.Collections.Generic;

namespace LegendsOfAntir
{
    class Game
    {
        public List<Item> Items;
        public Player Player;
        public List<Room> Rooms;
        public List<Character> Characters;
        public List<DialogueNode> DialogueNodes;
        public int LowerDifficulty;
        public int HigherDifficulty;
        public Dictionary<Character, int> Initiative;

        public bool Stop;

        public Game() { }

        public void GameLoop()
        {
            bool moved = true;
            do
            {
                if (moved)
                {
                    Room currentRoom = Player.CurrentRoom;
                    currentRoom.Show();
                }
                moved = this.GetPlayerCommand();
            } while (!Stop);
        }

        public void ShowCommands()
        {
            Console.Write("You can use the following commands: ");
            Console.Write("t(a)lk [character name], ");
            Console.Write("(f)ight, ");
            Console.Write("(l)ook, ");
            Console.Write("(m)ove, ");
            Console.Write("(q)uit), ");
            Console.Write("(t)ake [item], ");
            Console.Write("(d)rop [item], ");
            Console.Write("(u)se [item], ");
            Console.Write("(i)nventory, ");
            Console.WriteLine("(c)ommands");
        }

        public bool CheckForHostiles()
        {
            foreach (var entry in Initiative)
            {
                if (entry.Key is Npc)
                {
                    Npc npc = (Npc)entry.Key;
                    if (npc.Status == CharacterStatus.Hostile)
                        return true;
                }
            }

            return false;
        }

        public void Fight()
        {
            foreach (Character character in Player.CurrentRoom.Characters)
            {
                int result = character.SkillCheck(character.Attributes[Attribute.Agility]);
                Initiative.Add(character, result);
            }

            bool fighting = true;

            while (fighting && !Stop)
            {
                foreach (var entry in Initiative.OrderByDescending(x => x.Value))
                {
                    var character = entry.Key;

                    if (character is Player)
                    {
                        bool fled = PlayerTurn();
                        if (fled)
                        {
                            fighting = false;
                            break;
                        }
                    }
                    else
                    {
                        NPCTurn((Npc)character);
                    }

                    bool enemyRemaining = CheckForHostiles();
                    if (!enemyRemaining)
                    {
                        fighting = false;
                        break;
                    }
                }
            }

            Console.WriteLine("The fight is over.");
            Initiative.Clear();

        }

        public bool PlayerTurn()
        {
            bool choosing = true;
            while (choosing)
            {
                try
                {
                    Console.WriteLine("It is your turn in combat, what do you do?");
                    Console.WriteLine("1. Attack [target].");
                    Console.WriteLine("2. Flee.");

                    Console.Write("> ");
                    string[] input = Console.ReadLine().ToLower().Split(" ", 2);
                    string command = input[0];
                    string option = "";

                    if (input.Length > 1)
                        option = input[1];

                    switch (command)
                    {
                        case "1":
                        case "attack":
                            Npc target = null;
                            foreach (var entry in Initiative)
                            {
                                if (entry.Key.Name.ToLower().Equals(option))
                                    target = (Npc)entry.Key;
                            }
                            Player.Attack(target);
                            choosing = false;
                            break;

                        case "2":
                        case "flee":
                            if (Player.Flee())
                                return true;
                            choosing = false;
                            break;

                        default:
                            Console.WriteLine("You entered an unrecognized command, please try again.");
                            break;

                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Your command was false or incomplete. Please try again.");
                    return PlayerTurn();
                }
            }
            return false;
        }

        public void NPCTurn(Npc npc)
        {
            switch (npc.Status)
            {
                case CharacterStatus.Dead:
                    Initiative.Remove(npc);
                    break;

                case CharacterStatus.Ally:
                    foreach (var pair in Initiative)
                    {
                        if (pair.Key is Npc)
                        {
                            Npc target = (Npc)pair.Key;
                            if (target.Status == CharacterStatus.Hostile)
                            {
                                npc.Attack(target);
                                break;
                            }
                        }
                    }
                    break;

                case CharacterStatus.Friendly:
                    npc.Flee();
                    break;

                case CharacterStatus.Hostile:
                    npc.Attack(Player);
                    break;
            }
        }

        public bool GetPlayerCommand()
        {
            try
            {
                Console.Write("> ");
                string input = Console.ReadLine().ToLower();
                string[] inputArray = input.Split(" ", 2);
                string command = inputArray[0];
                string option = "";

                if (inputArray.Length > 1)
                    option = inputArray[1];

                switch (command)
                {
                    case "talk":
                    case "a":
                        Player.Dialogue(option);
                        break;

                    case "fight":
                    case "f":
                        this.Fight();
                        break;

                    case "look":
                    case "l":
                        return true;

                    case "move":
                    case "m":
                        Player.Move((Direction)Enum.Parse(typeof(Direction), option, true));
                        return true;

                    case "quit":
                    case "q":
                        Program.SaveGameFIle();
                        Stop = true;
                        break;

                    case "take":
                    case "t":
                        Player.TakeItem(option);
                        break;

                    case "drop":
                    case "d":
                        Player.DropItem(option);
                        break;

                    case "use":
                    case "u":
                        Player.UseItem(option);
                        break;

                    case "inventory":
                    case "i":
                        Player.ShowInventory();
                        break;

                    case "commands":
                    case "c":
                        this.ShowCommands();
                        break;

                    default:
                        Console.WriteLine("You entered an unrecognized command, please try again.");
                        break;
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Your command was false or incomplete. Please try again.");
                Console.WriteLine(e);
                return this.GetPlayerCommand();
            }

            return false;
        }

        public void OnDeathEvent()
        {
            Console.WriteLine("You died.");
            Console.WriteLine("Game Over.");
            Stop = true;
        }

        public void AddDeathListener()
        {
            this.Player.deathEvent += OnDeathEvent;
        }
    }
}