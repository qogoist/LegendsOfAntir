using System;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Item
    {
        public String Name;

        public Item(){}

        virtual public void Show()
        {
            Console.Write("a ");
            Console.WriteLine(Name);
        }
    }
}