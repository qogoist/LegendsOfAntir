using System;
using System.Linq;
using static LegendsOfAntir.Program;

namespace LegendsOfAntir
{
    class Player : Character
    {
        public Player(){}

        public override void Move(Direction direction)
        {
            Console.WriteLine("You are going to the " + direction + ".");
            base.Move(direction);

            foreach (NPC npc in _game.characters)
            {
                Direction npcDirection = Direction.north;
                if (npc.home == null)
                {
                    Random random = new Random();
                    npcDirection = (Direction)random.Next(0, npc.currentRoom.exits.Count);
                    npc.Move(npcDirection);
                }
                else if (!npc.home.Equals(npc.currentRoom))
                {
                    foreach (var exit in npc.currentRoom.exits)
                    {
                        if (exit.Value.Equals(npc.home))
                            npcDirection = exit.Key;
                    }
                    npc.Move(npcDirection);
                }
            }
        }

        public void Dialogue(string npc)
        {
            NPC character = null;

            foreach (NPC chara in this.currentRoom.characters)
            {
                if (chara.name.Equals(npc))
                    character = chara;
            }

            if (character == null)
            {
                Console.WriteLine("This character does not exist.");
                return;
            }

            if (character.status == CharacterStatus.Hostile)
            {
                Console.WriteLine(character.name + " doesn't want to talk. They want to fight.");
                _game.Fight();
                return;
            }

            DialogueNode dialogue = character.dialogue;
            while (dialogue != null)
            {
                dialogue.Show();

                Answer answer = null;

                try
                {
                    int input = Int32.Parse(Console.ReadKey().ToString());

                    for (int i = 1; i <= dialogue.answers.Count; i++)
                    {
                        if (i == input)
                        {
                            answer = dialogue.answers[i];
                            break;
                        }
                    }

                    dialogue = answer.destination;

                }
                catch(System.Exception)
                {
                    Console.WriteLine("The choice you picked wasn't valid, please try again.");
                }
            }

        }

        public void Flee()
        {
            int mod = this.attributes[Attribute.Smarts];
            int result = SkillCheck(mod);

            if (result < _game.lowerDifficulty)
            {
                Console.WriteLine("You fail to flee.");
                return;
            }
            else if (result < _game.higherDifficulty && result > _game.lowerDifficulty)
            {
                NPC attacker = null;

                foreach (NPC character in _game.initiative)
                {
                    if (character.status == CharacterStatus.Hostile)
                    {
                        attacker = character;
                        break;
                    }
                }

                attacker.Attack(this);
            }

            Random random = new Random();
            Direction direction = (Direction)random.Next(0, this.currentRoom.exits.Count);

            Console.WriteLine("You manage to flee to the " + direction);

            this.Move(direction);
        }
    }
}