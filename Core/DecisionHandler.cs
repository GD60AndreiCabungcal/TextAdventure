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

                //user can exit application when they type 'exit'
                if(input.ToUpper() == "EXIT") {
                    System.Environment.Exit(0);
                }

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

        public static void MovePlayer(World world, Player player)
        {
            //prompt user where to go
            Console.WriteLine($"Where would you like to move{(string.IsNullOrEmpty(player.Name) ? "?" : $", {player.Name}?")}");
            
            //setup dirrection prompt
            (int, int, string)[] directions = new (int, int, string)[] { (0,1, "south"), (0,-1, "north"), (1,0, "east"), (-1,0, "west") };
            List<(int, int, string)> allowedMoves = new List<(int, int, string)>();
            (int, int, string) move = (0, 0, "");
            string directionOutput = "move <";

            foreach(var direction in directions) {
                int dirX = player.Position[0] + direction.Item1;
                int dirY = player.Position[1] + direction.Item2;

                //if the direction is out of bounds, then skip it
                if((dirX < 0 || dirX >= world.Locations.GetLength(0)) || ((dirY < 0 || dirY >= world.Locations.GetLength(1)))) continue;

                if(world.Locations[dirX, dirY] != null) {
                    directionOutput += direction.Item3 + '|';
                    allowedMoves.Add(direction);
                }
            }
            directionOutput = directionOutput.TrimEnd('|');
            directionOutput += ">";

            Console.WriteLine(directionOutput);

            //get player input
            string[] input;
            while(true)
            {
                input = Console.ReadLine().ToLower().Split(' ');
                if(input.Length != 2) {
                    Console.WriteLine("Invalid input length.");
                } else if (input[0] != "move") {
                    Console.WriteLine("Invalid 1st argument.");
                } else if (!allowedMoves.Exists(dir => dir.Item3 == input[1])) {
                    Console.WriteLine("Invalid 2nd argument.");
                } else {
                    move = allowedMoves.Find(dir => dir.Item3 == input[1]);
                    break;
                }         
            }

            //apply player input
            world.LocalMoveEntity(player, move.Item1, move.Item2);
        }
    }
}
