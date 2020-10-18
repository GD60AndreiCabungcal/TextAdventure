// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class DecisionHandler
    {
        //player makes a decision
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

        public static void GetInput(World world, Player player)
        {
            //prompt user what to do
            Console.WriteLine($"What would you like to do{(string.IsNullOrEmpty(player.Name) ? "?" : $", {player.Name}?")}");

            //display all actions to the player
            foreach(PlayerAction<bool> action in CommandHandler.defaultCommands)
            {
                Console.WriteLine(action.Display(world, player));
            }

            //action loop
            string[] input;
            while(true)
            {
                input = Console.ReadLine().Split(' ');

                PlayerAction<bool> action = CommandHandler.defaultCommands.Find((x) => x.Name == input[0].ToLower());
                if(action != null) 
                {
                    bool result = action.Do(world, player, input);
                    if(result) break; //if the result is true, the action loop will terminate
                }
            }
        }
    }
}
