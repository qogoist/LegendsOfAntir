using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using static LegendsOfAntir.Program;

namespace LegendsOfAntir
{
    class Character
    {
        public String Name;
        [JsonProperty]
        private String _description;
        public Room CurrentRoom;
        public List<Item> Inventory;
        public int Hp;
        public Dictionary<Attribute, int> Attributes;
        public Weapon Weapon;
        public Armor Armor;


        public Character() { }

        public void Show()
        {
            Console.WriteLine(Name + ": " + _description);
        }

        public virtual void Move(Direction direction)
        {
            Room destination = this.CurrentRoom.Exits[direction];

            if (destination == null)
            {
                Console.WriteLine("There is nowhere to go in that direction.");
                return;
            }

            this.CurrentRoom.Characters.Remove(this);
            this.CurrentRoom = destination;
            destination.Characters.Add(this);

        }

        public void Attack(Character target)
        {
            int attributeNum;
            int damage;

            if (this.Weapon == null)
            {
                attributeNum = this.Attributes[Attribute.Brawn];
                damage = 0;
            }
            else
            {
                attributeNum = this.Attributes[this.Weapon.UsedAttribute];
                damage = this.Weapon.Damage;
            }

            int result = SkillCheck(attributeNum);

            if (result < Program.Game.LowerDifficulty)
            {
                Console.WriteLine(this.Name + " missed " + target.Name);
                return;
            }
            else if (result > Program.Game.HigherDifficulty)
            {
                damage += attributeNum;
            }
            else
            {
                damage = (damage + attributeNum) / 2;
            }

            int armor = 0;
            if (target.Armor != null)
                armor = target.Armor.Protection;

            damage -= armor;
            target.Hp -= damage;

            Console.WriteLine(this.Name + " hit " + target.Name + " and dealt " + damage + " damage.");

            if (target.Hp > 0)
                return;

            switch (target)
            {
                case Player p:
                    Program.Game.Player.TriggerDeath();
                    break;

                case Npc n:
                    Npc npcTarget = (Npc)target;
                    npcTarget.Status = CharacterStatus.Dead;
                    npcTarget.DropAllItems();

                    Console.WriteLine(npcTarget.Name + " died.");

                    Program.Game.Characters.Remove(npcTarget);
                    npcTarget.CurrentRoom.Characters.Remove(npcTarget);
                    break;
            }
        }

        public bool Flee()
        {
            int mod = this.Attributes[Attribute.Smarts];
            int result = SkillCheck(mod);

            if (result < Program.Game.LowerDifficulty)
            {
                Console.WriteLine(this.Name + " fails to flee.");
                return false;
            }
            else if (result < Program.Game.HigherDifficulty && result > Program.Game.LowerDifficulty)
            {
                Npc attacker = null;

                foreach (var entry in Program.Game.Initiative)
                {
                    if (entry.Key is Npc)
                    {
                        Npc npc = (Npc)entry.Key;

                        if (npc.Status == CharacterStatus.Hostile)
                        {
                            attacker = npc;
                            break;
                        }
                    }
                }

                Console.WriteLine(this.Name + " attempts to flee, but " + attacker.Name + " manages to hit them first.");
                attacker.Attack(this);
            }

            Direction direction = GetRandomDirection();

            Console.WriteLine(this.Name + " manages to flee to the " + direction);

            this.Move(direction);

            return true;
        }

        public void ShowInventory()
        {
            Console.WriteLine("You have the following items in your inventory: ");
            foreach (Item item in this.Inventory)
            {
                item.Show();
            }
        }

        public void DropItem(String name)
        {
            Item dropItem = FindItem(name);

            if (dropItem == null)
            {
                Console.WriteLine("The item you tried to drop is not in your inventory.");
                return;
            }

            this.Inventory.Remove(dropItem);

            if (dropItem.Equals(this.Weapon))
                this.Weapon = null;
            if (dropItem.Equals(this.Armor))
                this.Armor = null;

            this.CurrentRoom.Items.Add(dropItem);
            Console.WriteLine(this.Name + " dropped " + name);

        }

        public void TakeItem(String name)
        {
            Item pickItem = null;

            foreach (Item item in this.CurrentRoom.Items)
            {
                if (item.Name.Equals(name))
                    pickItem = item;
            }

            if (pickItem == null)
            {
                Console.WriteLine("The item you tried to take is not in this room.");
                return;
            }

            this.CurrentRoom.Items.Remove(pickItem);
            this.Inventory.Add(pickItem);
            Console.WriteLine(this.Name + " took " + name);
        }

        public void UseItem(String name)
        {
            Item useItem = FindItem(name);

            if (useItem == null)
            {
                Console.WriteLine("The item you tried to use is not in your inventory.");
                return;
            }

            Type itemType = useItem.GetType();

            switch (useItem)
            {
                case Armor a:
                    this.Armor = (Armor)useItem;
                    Console.WriteLine("You are now wearing " + this.Armor.Name + ".");
                    break;

                case Weapon w:
                    this.Weapon = (Weapon)useItem;
                    Console.WriteLine("You are now using " + this.Weapon.Name + " in fights.");
                    break;

                default:
                    Console.WriteLine("You cannot use this item right now.");
                    break;

                case null:
                    Console.WriteLine("The item you tried to use is not in your inventory.");
                    break;
            }
        }

        public void GiveItem(Character character, String item)
        {
            Item giveItem = FindItem(item);

            if (giveItem == null)
            {
                Console.WriteLine("The item you tried to give is not in your inventory.");
                return;
            }

            this.Inventory.Remove(giveItem);
            character.Inventory.Add(giveItem);
            Console.WriteLine(this.Name + " has given " + giveItem.Name + " to " + character.Name + ".");

        }

        public void DropAllItems()
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                DropItem(Inventory[i].Name);
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

        private Item FindItem(String name)
        {
            Item returnItem = null;

            foreach (Item item in this.Inventory)
            {
                if (item.Name.Equals(name))
                    returnItem = item;
            }

            return returnItem;
        }

        public Direction GetRandomDirection()
        {
            Random random = new Random();
            Direction direction;
            do
            {
                direction = (Direction)random.Next(0, 4);
            } while (!this.CurrentRoom.Exits.ContainsKey(direction));

            return direction;
        }
    }
}