using System;

namespace LegendsOfAntir
{
    class Weapon : Item
    {
        public int damage;
        public Attribute attribute;

        public Weapon(){}

        override public void Show()
        {
            Console.WriteLine(name + ": " + "+" + damage + " damage.");
        }
    }
}