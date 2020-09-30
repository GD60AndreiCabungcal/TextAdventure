// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

using TextAdventure.Core;

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
        Console.WriteLine($"Well hello there, {name}. Which item do you want to take?");
        inventory.Add(items[DecisionHandler.MakeDecision(items, new string[] { "Ooh, I like Apples too.", "You're ready to attack, now!", "Now you can feel warm at night.", "You're ready to defend!" })]);

        //user goes to a location
        Console.WriteLine("Where would you like to go?");
        curLocationIndex = DecisionHandler.MakeDecision(locations, $"{name} went to the");
        curLocation = locations[curLocationIndex];

        //user has encountered an enemy
        enemy = enemies[rng.Next(enemies.Length)];
        Console.WriteLine($"As {name} walked to the {curLocation}, they came across a wild {enemy}!");
        switch(DecisionHandler.MakeDecision(new string[] { "Use an item", "Run", "Ignore it", "Become Friends"}, new string[] { $"{name} used {inventory.First()} on {enemy}!" , $"{name} ran away from the {enemy}.", $"{name} ignored it.", $"{name} is now best friends with a {enemy}!" })) {
            case 0: {
                Console.WriteLine("What item do you want to use?");
                string item = items[DecisionHandler.MakeDecision(items, $"{name} used the")];
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
        Console.Write($"{name} arrived at the {curLocation}. ");
        switch(curLocationIndex) {
            //if the user went to the village
            case 0: {
                //itemName, price
                (string, int)[] shop = new (string, int)[] { ("bucket", 5), ("Scarf", 1), ("Zoar Meat", 7) };
                string[] options = new string[shop.Length + 1];
                for(int i = 0; i < shop.Length; i++) {
                    options[i] = $"{shop[i].Item1} for {shop[i].Item2}";
                }
                options[options.Length - 1] = "Nothing";

                //user attempts to buy item from shop
                Console.WriteLine($"A villager offers {name} to shop at his shop. The villager offered the following items:");
                int itemIndex = DecisionHandler.MakeDecision(options, $"{name} bought");
                if(itemIndex != options.Length - 1) {
                    (string, int) item = shop[itemIndex];
                    inventory.Add(item.Item1);
                    money -= item.Item2;
                }
                Console.WriteLine($"{name} have ${money} left.");
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
                DecisionHandler.MakeDecision(new string[] { "Pet the Dog.", "Ignore It." }, new string[] { $"while {name} pet the dog, it pooped on your foot.", $"{name} ignored the dog. It was quite smelly."});
                break;
            }
            //if the user went to the hill
            case 3: {
                if(friends.Count > 0) {
                    //interact with friend
                    string friend = friends[rng.Next(friends.Count)];
                    Console.WriteLine($"{friend} spots you in the distance. {name} responds: ");
                    DecisionHandler.MakeDecision(new string[] { "\"Hello, how are you doing?\"", "\"Hi!\"", "\"I'm busy right now.\"" }, new string[] { "I'm good! good luck on your adventure!", "Hello! Good luck on your adventure!", "Oh."});
                } 
                else {
                    Console.WriteLine("The Hill is the tallest thing around here.");
                }
                break;
            }
        }
    }
}
