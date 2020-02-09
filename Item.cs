using System;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Item
    {
        public String name;

        public Item(){}

        virtual public void Show()
        {
            Console.Write("a ");
            Console.WriteLine(name);
        }
    }
}