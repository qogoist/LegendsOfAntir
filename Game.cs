using System;
using System.Collections.Generic;

namespace LegendsOfAntir
{
    class Game
    {
        public List<Room> rooms;
        public List<Character> characters;
        public List<Item> items;
        public List<DialogueNode> dialogueNodes;
        public List<Answer> answers;
        public List<String> commands;
        public int lowerDifficulty;
        public int higherDifficulty;

        public Game(){}

        public void GameLoop()
        {
            Console.WriteLine("Not implemented yet!");
        }

        public void ShowCommands()
        {
            Console.WriteLine("Not implemented yet!");
        }

        public bool CheckForHostiles()
        {
            Console.WriteLine("Not implemented yet!");
            return true;
        }

        public void Fight()
        {
            Console.WriteLine("Not implemented yet!");
        }


    }
}