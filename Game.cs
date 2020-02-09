using System;
using System.Linq;
using System.Collections.Generic;

namespace LegendsOfAntir
{
    class Game
    {
        public List<Item> items;
        public Player player;
        public List<Room> rooms;
        public List<Character> characters;
        public List<DialogueNode> dialogueNodes;
        public int lowerDifficulty;
        public int higherDifficulty;
        public Dictionary<Character, int> initiative;

        public bool stop;

        public Game() { }

        public void GameLoop()
        {
            bool moved = true;
            do
            {
                if (moved)
                {
                    Room currentRoom = player.currentRoom;
                    currentRoom.Show();
                }
                moved = this.GetPlayerCommand();
            } while (!stop);
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
            foreach (var entry in initiative)
            {
                if (entry.Key is NPC)
                {
                    NPC npc = (NPC)entry.Key;
                    if (npc.status == CharacterStatus.Hostile)
                        return true;
                }
            }

            return false;
        }

        public void Fight()
        {
            foreach (Character character in player.currentRoom.characters)
            {
                int result = character.SkillCheck(character.attributes[Attribute.Agility]);
                initiative.Add(character, result);
            }

            bool fighting = true;

            while (fighting && !stop)
            {
                foreach (var entry in initiative.OrderByDescending(x => x.Value))
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
                        NPCTurn((NPC)character);
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
            initiative.Clear();

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
                            NPC target = null;
                            foreach (var entry in initiative)
                            {
                                if (entry.Key.name.ToLower().Equals(option))
                                    target = (NPC)entry.Key;
                            }
                            player.Attack(target);
                            choosing = false;
                            break;

                        case "2":
                        case "flee":
                            if (player.Flee())
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

        public void NPCTurn(NPC npc)
        {
            switch (npc.status)
            {
                case CharacterStatus.Dead:
                    initiative.Remove(npc);
                    break;

                case CharacterStatus.Ally:
                    foreach (var pair in initiative)
                    {
                        if (pair.Key is NPC)
                        {
                            NPC target = (NPC)pair.Key;
                            if (target.status == CharacterStatus.Hostile)
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
                    npc.Attack(player);
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
                        player.Dialogue(option);
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
                        player.Move((Direction)Enum.Parse(typeof(Direction), option, true));
                        return true;

                    case "quit":
                    case "q":
                        Program.SaveGameFIle();
                        stop = true;
                        break;

                    case "take":
                    case "t":
                        player.TakeItem(option);
                        break;

                    case "drop":
                    case "d":
                        player.DropItem(option);
                        break;

                    case "use":
                    case "u":
                        player.UseItem(option);
                        break;

                    case "inventory":
                    case "i":
                        player.ShowInventory();
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
            stop = true;
        }

        public void AddDeathListener()
        {
            this.player.deathEvent += OnDeathEvent;
        }
    }
}