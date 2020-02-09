using System;
using System.Collections.Generic;
using static LegendsOfAntir.Program;
using Newtonsoft.Json;

namespace LegendsOfAntir
{
    class DialogueNode
    {
        [JsonProperty]
        private String text;
        public List<Answer> answers;
        [JsonProperty]
        private bool give;
        [JsonProperty]
        private string[] items;
        [JsonProperty]
        private string character;

        public DialogueNode(){}

        public void Show()
        {
           Console.WriteLine(text);

           if (give)
           {
               NPC giver = null;
               foreach (Character npc in _game.player.currentRoom.characters)
               {
                    if (npc.name.Equals(this.character))
                        giver = (NPC)npc;
               }

               foreach(string item in items)
               {
                   giver.GiveItem(_game.player, item);
               }
           }

           int i = 1;
           foreach(Answer answer in answers)
           {
               Console.Write(i + ": ");
               answer.Show();
               i++;
           } 
        }
    }
}