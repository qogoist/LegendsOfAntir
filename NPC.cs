using System;

namespace LegendsOfAntir
{
    class Npc : Character
    {
        public Npc(){}
        public DialogueNode Dialogue;
        public CharacterStatus Status;
        public Room Home;
    }
}