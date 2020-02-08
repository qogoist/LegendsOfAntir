using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Character
    {
        [JsonProperty]
        private String name;
        [JsonProperty]
        private String description;
        [JsonProperty]
        private Room currentRoom;
        [JsonProperty]
        private List<Item> inventory;
        [JsonProperty]
        private int hp;
        [JsonProperty]
        private Dictionary<Attribute, int> attributes;
        [JsonProperty]
        private Weapon weapon;
        [JsonProperty]
        private Armor armor;


        public Character() { }

        public void Show()
        {
            Console.WriteLine(name + ": " + description);
        }

        public void Move(Direction destination)
        {

        }

        public void Attack(Character target)
        {

        }

        public void Flee()
        {

        }

        public void DropItem(String name)
        {

        }

        public void TakeItem(String name)
        {

        }

        public void UseItem(String name)
        {

        }

        public void DropAllItems()
        {
            foreach (Item item in inventory)
            {
                //this.DropItem(item.name);
            }
        }

        public int SkillCheck(int mod)
        {
            Random random = new Random();
            int result = random.Next(1, 9);
            result += random.Next(1, 9);
            result += mod;

            return result;
        }
    }
}