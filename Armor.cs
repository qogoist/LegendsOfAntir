using System;

namespace LegendsOfAntir
{
    class Armor : Item
    {
        public int protection;

        public Armor(){}

        override public void Show()
        {
            Console.Write("a ");
            Console.WriteLine(name + ": " + protection + " protection.");
        }
    }
}