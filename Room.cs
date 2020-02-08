using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Room
    {
        [JsonProperty]
        private String description;
        [JsonProperty]
        private List<Character> characters;
        [JsonProperty]
        private List<Item> items;
        [JsonProperty]
        private Dictionary<Direction, Room> exits;

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
            foreach (Character character in characters)
            {
                character.Show();
            }
        }
    }
}