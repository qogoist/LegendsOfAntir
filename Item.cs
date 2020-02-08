using System;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Item
    {
        [JsonProperty]
        protected String name;

        public Item(){}

        virtual public void Show()
        {
            Console.WriteLine(name);
        }
    }
}