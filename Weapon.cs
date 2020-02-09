using System;

namespace LegendsOfAntir
{
    class Weapon : Item
    {
        public int Damage;
        public Attribute UsedAttribute;

        public Weapon(){}

        override public void Show()
        {
            Console.Write("a ");
            Console.WriteLine(Name + ": " + "+" + Damage + " damage.");
        }
    }
}