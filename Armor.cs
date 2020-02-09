using System;

namespace LegendsOfAntir
{
    class Armor : Item
    {
        public int Protection;

        public Armor(){}

        override public void Show()
        {
            Console.Write("a ");
            Console.WriteLine(Name + ": " + Protection + " protection.");
        }
    }
}