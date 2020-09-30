using System;

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
    }
}
