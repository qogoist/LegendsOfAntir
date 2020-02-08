using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class DialogueNode
    {
        [JsonProperty]
        private String text;
        public List<Answer> answers;

        public DialogueNode(){}

        public void Show()
        {
           Console.WriteLine(text);

           int i = 1;
           foreach(Answer answer in answers)
           {
               Console.Write(i + ": ");
               answer.Show();
           } 
        }
    }
}