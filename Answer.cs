using System;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Answer
    {
        [JsonProperty]
        private String text;
        public DialogueNode destination;
        public bool give;
        public string[] items;
        public string character;

        public Answer(){}

        public void Show()
        {
            Console.WriteLine(text);
        }
    }
}