using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Room
    {
        [JsonProperty]
        private String _description;
        public List<Character> Characters;
        public List<Item> Items;
        public Dictionary<Direction, Room> Exits;

        public Room(){}

        public void Show()
        {
            Console.WriteLine(this._description);
            
            Console.WriteLine("You spot these items: ");
            foreach (Item item in Items)
            {
                Console.Write("  - ");
                item.Show();
            }

            Console.WriteLine("And these people: ");
            foreach (Character character in this.Characters)
            {
                if (character is Npc)
                {
                    Console.Write("  - ");
                    character.Show();
                }
            }

            Console.WriteLine("You can go: ");
            foreach (var exit in this.Exits)
            {
                Console.Write("  - ");
                Console.WriteLine(exit.Key);
            }
        }
    }
}