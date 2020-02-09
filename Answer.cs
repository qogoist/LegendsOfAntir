using System;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class Answer
    {
        [JsonProperty]
        private String _text;
        public DialogueNode Destination;

        public Answer(){}

        public void Show()
        {
            Console.WriteLine(_text);
        }
    }
}