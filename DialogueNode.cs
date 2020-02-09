using System;
using System.Collections.Generic;
using static LegendsOfAntir.Program;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class DialogueNode
    {
        [JsonProperty]
        private String _text;
        public List<Answer> Answers;
        [JsonProperty]
        private bool _give;
        [JsonProperty]
        private string[] _items;
        [JsonProperty]
        private string _character;

        public DialogueNode(){}

        public void Show()
        {
           Console.WriteLine(_text);

           if (_give)
           {
               Npc giver = null;
               foreach (Character npc in Program.Game.Player.CurrentRoom.Characters)
               {
                    if (npc.Name.Equals(this._character))
                        giver = (Npc)npc;
               }

               foreach(string item in _items)
               {
                   giver.GiveItem(Program.Game.Player, item);
               }
           }

           int i = 1;
           foreach(Answer answer in Answers)
           {
               Console.Write(i + ": ");
               answer.Show();
               i++;
           } 
        }
    }
}