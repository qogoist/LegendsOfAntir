using System;
using System.Linq;
using static LegendsOfAntir.Program;

namespace LegendsOfAntir
{
    class Player : Character
    {
        public delegate void Death();
        public event Death deathEvent;

        public Player() { }

        public override void Move(Direction direction)
        {
            Console.WriteLine("You are going to the " + direction + ".");
            base.Move(direction);

            foreach (Npc npc in Program.Game.Characters)
            {
                Direction npcDirection = Direction.North;
                if (npc.Home == null)
                {
                    npcDirection = npc.GetRandomDirection();

                    npc.Move(npcDirection);
                }
                else if (!npc.Home.Equals(npc.CurrentRoom))
                {
                    foreach (var exit in npc.CurrentRoom.Exits)
                    {
                        if (exit.Value.Equals(npc.Home))
                            npcDirection = exit.Key;
                    }
                    npc.Move(npcDirection);
                }
            }
        }

        public void Dialogue(string npc)
        {
            Npc character = null;

            foreach (Character chara in this.CurrentRoom.Characters)
            {
                if (chara.Name.ToLower().Equals(npc))
                    character = (Npc)chara;
            }

            if (character == null)
            {
                Console.WriteLine("This character does not exist.");
                return;
            }

            if (character.Status == CharacterStatus.Hostile)
            {
                Console.WriteLine(character.Name + " doesn't want to talk. They want to fight.");
                Program.Game.Fight();
                return;
            }

            DialogueNode dialogue = character.Dialogue;
            while (dialogue != null)
            {
                dialogue.Show();

                Answer answer = null;

                try
                {
                    Console.Write("> ");
                    int input = Int32.Parse(Console.ReadLine());

                    for (int i = 0; i < dialogue.Answers.Count; i++)
                    {
                        if (i + 1 == input)
                        {
                            answer = dialogue.Answers[i];
                            break;
                        }
                    }

                    dialogue = answer.Destination;
                }
                catch (System.Exception)
                {
                    Console.WriteLine("The choice you picked wasn't valid, please try again.");
                }
            }

        }

        public void TriggerDeath()
        {
            deathEvent();
        }
    }
}