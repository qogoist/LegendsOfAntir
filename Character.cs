using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using static LegendsOfAntir.Program;

namespace LegendsOfAntir
{
    class Character
    {
        public String name;
        public String description;
        public Room currentRoom;
        public List<Item> inventory;
        public int hp;
        public Dictionary<Attribute, int> attributes;
        public Weapon weapon;
        public Armor armor;


        public Character() { }

        public void Show()
        {
            Console.WriteLine(name + ": " + description);
        }

        public virtual void Move(Direction direction)
        {
            Room destination = this.currentRoom.exits[direction];

            if (destination == null)
            {
                Console.WriteLine("There is nowhere to go in that direction.");
                return;
            }

            this.currentRoom.characters.Remove(this);
            this.currentRoom = destination;
            destination.characters.Add(this);

        }

        public void Attack(Character target)
        {
            int attributeNum;
            int damage;

            if (this.weapon == null)
            {
                attributeNum = this.attributes[Attribute.Brawn];
                damage = 0;
            }
            else
            {
                attributeNum = this.attributes[this.weapon.attribute];
                damage = this.weapon.damage;
            }

            int result = SkillCheck(attributeNum);

            if (result < _game.lowerDifficulty)
            {
                Console.WriteLine(this.name + " missed " + target.name);
                return;
            }
            else if (result > _game.higherDifficulty)
            {
                damage += attributeNum;
            }
            else
            {
                damage = (damage + attributeNum) / 2;
            }

            int armor = 0;
            if (target.armor != null)
                armor = target.armor.protection;

            damage -= armor;
            target.hp -= damage;

            Console.WriteLine(this.name + " hit " + target.name + " and dealt " + damage + " damage.");

            if (target.hp > 0)
                return;

            switch (target)
            {
                case Player p:
                    _game.player.TriggerDeath();
                    break;

                case NPC n:
                    NPC npcTarget = (NPC)target;
                    npcTarget.status = CharacterStatus.Dead;
                    npcTarget.DropAllItems();

                    Console.WriteLine(npcTarget.name + " died.");
                    break;
            }
        }

        public bool Flee()
        {
            int mod = this.attributes[Attribute.Smarts];
            int result = SkillCheck(mod);

            if (result < _game.lowerDifficulty)
            {
                Console.WriteLine(this.name + " fails to flee.");
                return false;
            }
            else if (result < _game.higherDifficulty && result > _game.lowerDifficulty)
            {
                NPC attacker = null;

                foreach (var entry in _game.initiative)
                {
                    if (entry.Key is NPC)
                    {
                        NPC npc = (NPC)entry.Key;

                        if (npc.status == CharacterStatus.Hostile)
                        {
                            attacker = npc;
                            break;
                        }
                    }
                }

                Console.WriteLine(this.name + " attempts to flee, but " + attacker.name + " manages to hit them first.");
                attacker.Attack(this);
            }

            Random random = new Random();
            Direction direction = (Direction)random.Next(0, this.currentRoom.exits.Count);

            Console.WriteLine(this.name + " manages to flee to the " + direction);

            this.Move(direction);

            return true;
        }

        public void ShowInventory()
        {
            Console.WriteLine("You have the following items in your inventory: ");
            foreach (Item item in this.inventory)
            {
                item.Show();
            }
        }

        public void DropItem(String name)
        {
            Item dropItem = null;

            foreach (Item item in this.inventory)
            {
                if (item.name.Equals(name))
                    dropItem = item;
            }

            if (dropItem == null)
            {
                Console.WriteLine("The item you tried to drop is not in your inventory.");
                return;
            }

            this.inventory.Remove(dropItem);
            this.currentRoom.items.Add(dropItem);
            Console.WriteLine("You dropped " + name);

        }

        public void TakeItem(String name)
        {
            Item pickItem = null;

            foreach (Item item in this.currentRoom.items)
            {
                if (item.name.Equals(name))
                    pickItem = item;
            }

            if (pickItem == null)
            {
                Console.WriteLine("The item you tried to take is not in this room.");
                return;
            }

            this.currentRoom.items.Remove(pickItem);
            this.inventory.Add(pickItem);
            Console.WriteLine("You took " + name);
        }

        public void UseItem(String name)
        {
            Item useItem = null;

            foreach (Item item in this.inventory)
            {
                if (item.name.Equals(name))
                    useItem = item;
            }

            Type itemType = useItem.GetType();

            switch (useItem)
            {
                case Armor a:
                    this.armor = (Armor)useItem;
                    break;

                case Weapon w:
                    this.weapon = (Weapon)useItem;
                    break;

                default:
                    Console.WriteLine("You cannot use this item right now.");
                    break;

                case null:
                    Console.WriteLine("The item you tried to use is not in your inventory.");
                    break;
            }
        }

        public void DropAllItems()
        {
            foreach (Item item in inventory)
            {
                this.DropItem(item.name);
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