using System;

namespace LegendsOfAntir
{
    class NPC : Character
    {
        public NPC(){}
        public DialogueNode dialogue;
        public CharacterStatus status;
        public Room home;
    }
}