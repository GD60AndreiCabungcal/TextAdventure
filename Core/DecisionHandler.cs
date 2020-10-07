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
            Console.WriteLine("stats");
            Console.WriteLine("inventory");
            Console.WriteLine("eat");

            //setup dirrection command
            (int, int, string)[] directions = new (int, int, string)[] { (0,1, "south"), (0,-1, "north"), (1,0, "east"), (-1,0, "west") };
            List<(int, int, string)> allowedMoves = new List<(int, int, string)>();
            string directionOutput = "move <";

            foreach(var direction in directions) {
                int dirX = player.Pos.x + direction.Item1;
                int dirY = player.Pos.y + direction.Item2;

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
            bool inputActive = true;
            while(inputActive)
            {
                input = Console.ReadLine().Split(' ');
                switch(input[0].ToLower())
                {
                    //when player wants to check their stats
                    case "stats":
                    {   
                        string name = player.Name;
                        int health = player.Health;
                        int armor = player.Armor;
                        int fullness = player.Fullness;
                        float money = player.Money;
                        
                        string allies = "";
                        foreach(Entity ally in player.Allies)
                        {
                            allies += $"\n    {ally}";
                        }

                        //display stats
                        Console.WriteLine($"\n{name}'s Stats:\nHealth: {health}\nArmor: {armor}\nFullness: {fullness}\nMoney: {money}\nAllies: {allies}");
                        inputActive = false;
                        break;
                    }
                    //when player wants to check their inventory
                    case "inventory":
                    {   
                        string inventory = "";
                        foreach(Item item in player.Inventory)
                        {
                            inventory += $"\n    {item.Stats()}";
                        }
                        Console.WriteLine($"\nInventory: {inventory}");
                        inputActive = false;
                        break;
                    }
                    case "eat":
                    {   
                        if(input.Length != 2)
                        {
                            Console.WriteLine("Invalid Argument Length.");
                            break;
                        }
                        Food food = (Food)player.Inventory.Find(x => x.GetType() == typeof(Food) && x.Name.ToLower() == input[1].ToLower());
                        player.Heal(food);
                        break;
                    }
                    //when player wants to move around
                    case "move":
                    {
                        if(input.Length != 2)
                        {
                            Console.WriteLine("Invalid Argument Length.");
                            break;
                        }
                        (int, int, string) move = allowedMoves.Find(x => x.Item3 == input[1].ToLower());
                        if(move == (0, 0, null)) {
                            Console.WriteLine("Invalid 2nd argument.");
                        } else {
                            world.LocalMoveEntity(player, move.Item1, move.Item2);
                            inputActive = false;
                        }
                        break;
                    }
                    default: 
                    {
                        Console.WriteLine("Invalid Arguments.");
                        break;
                    }
                }
            }
        }
    }
}
