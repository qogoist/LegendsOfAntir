using System;
using System.Collections.Generic;

namespace LegendsOfAntir
{
    class Game
    {
        public List<Room> rooms;
        public List<Character> characters;
        public List<Item> items;
        public List<DialogueNode> dialogueNodes;
        public List<Answer> answers;
        public int lowerDifficulty;
        public int higherDifficulty;
        public List<Character> initiative;
        public Player player;

        public Game() { }

        public void GameLoop()
        {
            bool running = true;
            do
            {
                Room currentRoom = player.currentRoom;
                currentRoom.Show();
                running = this.GetPlayerCommand();
            } while (running);

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
            foreach (NPC character in initiative)
            {
                if (character.status == CharacterStatus.Hostile)
                    return true;
            }

            return false;
        }

        public void Fight()
        {
            Console.WriteLine("Not implemented yet!");
            //TODO: Implement
        }

        public bool GetPlayerCommand()
        {
            try
            {
                Console.Write("> ");
                string input = Console.ReadLine().ToLower();
                string[] inputArray = input.Split(" ");
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
                        break;

                    case "move":
                    case "m":
                        player.Move((Direction)Enum.Parse(typeof(Direction), option));
                        break;

                    case "quit":
                    case "q":
                        return false;

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
            catch (System.Exception)
            {
                Console.WriteLine("Your command was false or incomplete. Please try again.");
                return this.GetPlayerCommand();
            }

            return true;
        }


    }
}