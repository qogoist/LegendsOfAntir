using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Room
    {
        [JsonProperty]
        private String description;
        public List<Character> characters;
        public List<Item> items;
        public Dictionary<Direction, Room> exits;

        public Room(){}

        public void Show()
        {
            Console.WriteLine(this.description);
            
            Console.WriteLine("You spot these items: ");
            foreach (Item item in items)
            {
                item.Show();
            }

            Console.WriteLine("And these people: ");
            foreach (Character character in this.characters)
            {
                if (character is NPC)
                    character.Show();
            }

            Console.WriteLine("You can go: ");
            foreach (var exit in this.exits)
            {
                Console.WriteLine(exit.Key);
            }
        }
    }
}