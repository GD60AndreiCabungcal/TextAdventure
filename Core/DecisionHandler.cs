// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class DecisionHandler
    {
        public static int MakeDecision(string[] decisions, string[] answers) 
        {
            //user's answer
            int selection = 0;

            //display answers
            for(int i = 0; i < decisions.Length; i++) {
                Console.WriteLine($"{((char)(i+65)).ToString()}) {decisions[i]}");
            }

            //get valid user input
            while(true) {
                //player input
                string input = Console.ReadLine();
                char inputLetter = Char.ToUpper(input[0]);

                //convert input into possible index
                int index = inputLetter - '0' - 17;
                //if value is a valid index for the decisions array
                if(index < 0 || index >= decisions.Length) continue;

                //set selection to index and exit input validation
                selection = index;
                break;
            }

            //display the reaction and return the index of the reaction
            Console.WriteLine(answers[selection]);
            return selection;
        }

        public static int MakeDecision(string[] decisions, string answer = "You chose") {
            string[] answers = new string[decisions.Length];
            for(int i = 0; i < answers.Length; i++) {
                answers[i] = $"{answer} {decisions[i]}.";
            }
            return MakeDecision(decisions, answers);
        }

        public static int MakeDecision(List<string> decisions, List<string> answers) { return MakeDecision(decisions.ToArray(), answers.ToArray()); }
        public static int MakeDecision(List<string> decisions, string answer) { return MakeDecision(decisions.ToArray(), answer); }

        public static void GetInput(World world, Player player)
        {
            //prompt user what to do
            Console.WriteLine($"What would you like to do{(string.IsNullOrEmpty(player.Name) ? "?" : $", {player.Name}?")}");

            //setup for move command
            (int, int, string)[] directions = new (int, int, string)[] { (0,1, "south"), (0,-1, "north"), (1,0, "east"), (-1,0, "west") };
            List<(int, int, string)> allowedMoves = new List<(int, int, string)>();

            foreach(var direction in directions) {
                int dirX = player.Pos.x + direction.Item1;
                int dirY = player.Pos.y + direction.Item2;

                //if the direction is out of bounds, then skip it
                if((dirX < 0 || dirX >= world.Locations.GetLength(0)) || ((dirY < 0 || dirY >= world.Locations.GetLength(1)))) continue;

                if(world.Locations[dirX, dirY] != null) {
                    allowedMoves.Add(direction);
                }
            }

            //display each action available to the player
            foreach(PlayerAction<bool> action in CommandList.defaultCommands)
            {
                Console.WriteLine(action.Display(world, player, allowedMoves));
            }

            //get player input and perform action
            string[] input;
            while(true)
            {
                input = Console.ReadLine().Split(' ');

                PlayerAction<bool> action = CommandList.defaultCommands.Find((x) => x.Name == input[0].ToLower());
                if(action != null) 
                {
                    bool result = action.Do(world, player, input, allowedMoves);
                    if(result) break; //if the result is true, then the command was executed sucessfully
                }
            }
        }
    }
}
