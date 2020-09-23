// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

/*
TODO:
 - Add Math (+, -, *, /)
*/

class Program
{
    static void Main(string[] args)
    {
        //object used for generating random numbers
        Random rng = new Random();

        //user information
        string name;
        List<string> inventory = new List<string>();
        List<string> friends = new List<string>();
        float money = 50;

        //items
        string[] items = new string[] { "Apple", "Sword", "Fire Starting Kit", "Sheild" };

        //location information
        int curLocationIndex;
        string curLocation;
        string[] locations = { "Village", "Forest", "Park", "Hill", };

        //enemy inofrmation
        string enemy;
        string[] enemies = { "Bork", "Zoar", "Magger" };

        /* --- START OF GAME --- */

        //user stats their name
        Console.WriteLine("Hello Traveler! May you tell me what your name is?");
        name = Console.ReadLine();

        //user picks up an item
        Console.WriteLine($"Why hello there, {name}. Which item do you want to take?");
        inventory.Add(items[MakeDecision(items, new string[] { "Ooh, I like Apples too.", "You're ready to attack, now!", "Now you can feel warm at night.", "You're ready to defend!" })]);

        //user goes to a location
        Console.WriteLine("Where would you like to go?");
        curLocationIndex = MakeDecision(locations, "You went to the");
        curLocation = locations[curLocationIndex];

        //user has encountered an enemy
        enemy = enemies[rng.Next(enemies.Length)];
        Console.WriteLine($"As {name} walked to the {curLocation}, they came across a wild {enemy}!");
        switch(MakeDecision(new string[] { "Use an item", "Run", "Ignore it", "Become Friends"}, new string[] { $"You used {inventory.First()} on {enemy}!" , $"You ran away from the {enemy}.", $"{name} ignored it.", $"{name} is now best friends with a {enemy}!" })) {
            case 0: {
                Console.WriteLine("What item do you want to use?");
                string item = items[MakeDecision(items, $"{name} used the")];
                break;
                }
            case 3: {
                //user befriends the enemy
                friends.Add(enemy);
                break;
                }
            default: break;

        }

        //user ends up at curLocation
        Console.Write($"You arrived at the {curLocation}. ");
        switch(curLocationIndex) {
            //if the user went to the village
            case 0: {
                //itemName, price
                (string, int)[] shop = new (string, int)[] {("bucket", 5), ("Scarf", 1), ("Zoar Meat", 7) };
                string[] options = new string[shop.Length + 1];
                for(int i = 0; i < options.Length; i++) {
                    options[i] = $"{shop[i].Item1} for {shop[i].Item2}";
                }
                options[options.Length - 1] = "Nothing";

                //user attempts to buy item from shop
                Console.WriteLine($"A villager offers {name} to shop at his shop. What will you buy?");
                int itemIndex = MakeDecision(options, $"{name} bought");
                (string, int) item = shop[itemIndex];
                if(itemIndex != options.Length - 1) {
                    inventory.Add(item.Item1);
                    money -= item.Item2;
                }
                Console.WriteLine($"You have ${money} left.");
                break;
            }
            //if user went to the forest
            case 1: {
                Console.WriteLine("The forest seems peaceful and quiet.");
                break;
            }
            //if the user went to the park
            case 2: {
                Console.WriteLine($"In the distance, {name} comes across a Dog.");
                MakeDecision(new string[] { "Pet the Dog.", "Ignore It." }, new string[] { $"while {name} pet the dog, it pooped on your foot.", $"{name} ignored the dog. It was quite smelly."});
                break;
            }
            //if the user went to the hill
            case 3: {
                if(friends.Count > 0) {
                    //interact with friend
                    string friend = friends[rng.Next(friends.Count)];
                    Console.WriteLine($"{friend} spots you in the distance. {name} responds: ");
                    MakeDecision(new string[] { "\"Hello, how are you doing?\"", "\"Hi!\"", "\"I'm busy right now.\"" }, new string[] { "I'm good! good luck on your adventure!", "Hello! Good luck on your adventure!", "Oh."});
                } 
                else {
                    Console.WriteLine("The Hill is the tallest thing around here.");
                }
                break;
            }
        }
    }

    static int MakeDecision(string[] decisions, string[] answers) 
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

    static int MakeDecision(string[] decisions, string answer = "You chose") {
        string[] answers = new string[decisions.Length];
        for(int i = 0; i < answers.Length; i++) {
            answers[i] = $"{answer} {decisions[i]}.";
        }
        return MakeDecision(decisions, answers);
    }
}
