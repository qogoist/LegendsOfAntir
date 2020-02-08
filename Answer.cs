using System;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Answer
    {
        [JsonProperty]
        private String text;
        public DialogueNode destination;

        public Answer(){}

        public void Show()
        {
            Console.WriteLine(text);
        }
    }
}