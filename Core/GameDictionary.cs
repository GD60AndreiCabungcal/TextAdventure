// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class GameDictionary
    {
        /*Game Map
         01234 <- col/x value  Map layout
        0   *
        1S**E*
        2  * I
        3EI*S*
        4X
        ^- row/y value
        */

        public static readonly Location[,] MAP = new Location[/*col*/,/*row*/]
        {
            {
                null,
                new Shop("Cliffside Shop", new Merchant("Billy"), new ShopItem[] {
                    new ShopItem(new Armor("Gold Pants", 2, "Pants", description: "Very shiny."), 5),
                    new ShopItem(new Food("Shiny Apple", 10), 7),
                    new ShopItem(new Weapon("Beat Stick", 6, 0.8f), 10)
                }, "It's very high up here."),
                null,
                new Location("Grass", "Just plain grass."),
                new Location("Starting area", "Just plain grass.")
            },
            {
                null,
                new Location("Grass", "Just plain grass."),
                null,
                new Location("Grass", "Just plain grass."),
                null
            },
            {
                null,
                new Location("Grass", "Just plain grass."),
                new Location("Grass", "Just plain grass."),
                new Location("Grass", "Just plain grass."),
                null
            },
            {
                new Location("Grass", "Just plain grass."),
                new Location("Grass", "Just plain grass."),
                null,
                new Shop("Merchant Shop", new Merchant("Merchant"), new ShopItem[] {
                    new ShopItem(new Food("Apple", 2), 2),
                    new ShopItem(new Food("Shiny Carrot", 3), 3),
                    new ShopItem(new Weapon("Stone Sword", 2, 0.3f), 5),
                    new ShopItem(new Armor("Tin Helmet", 2, "Helmet"), 4)
                }),
                null
            },
            {
                null,
                new Location("Grass", "Just plain grass."),
                new Location("Grass", "Just plain grass."),
                new Location("Grass", "Just plain grass."),
                null
            },
        };

        //list of all the actions available to the player
        public static List<PlayerAction<bool>> defaultCommands = new PlayerAction<bool>[]
        {
            //when player wnats to check their stats
            new PlayerAction<bool>("stats", (args) => "stats", (args) => 
                {   
                    Player player = (Player)args[1];

                    string name = player.Name;
                    int health = player.Health;
                    int armor = player.Armor;
                    float money = player.Money;

                    //display stats
                    Console.WriteLine($"\n{name}'s Stats:\nHealth: {health}\nArmor: {armor}\nMoney: ${money}\n");
                    return false;
                }),
            //when player wants to check their inventory
            new PlayerAction<bool>("inventory", (args) => "inventory", (args) =>
                {   
                    Player player = (Player)args[1];

                    string inventory = "";
                    foreach(Item item in player.Inventory)
                    {
                        inventory += $"\n    {item.Stats()}";
                    }
                    Console.WriteLine($"\nInventory: {inventory}\n");
                    return false;
                }),
            //when player wants to eat
            new PlayerAction<bool>("eat", (args) => "eat", (args) => 
                {
                    Player player = (Player)args[1];
                    string[] input = (string[])args[2];

                    if(input.Length < 2) {
                        Console.WriteLine("Invalid Argument Length.");
                        return false;
                    } else if (player.Inventory.Count(x => x.GetType() == typeof(Food)) <= 0) {
                        Console.WriteLine("You have no food!");
                        return false;
                    }
                    Food food = (Food)player.Inventory.Find(x => x.GetType() == typeof(Food) && x.Name.Replace(" ", "") == string.Join("", input.Skip(1)));
                    player.Heal(food);
                    return false;
                }),
            //move player to a new location
            new PlayerAction<bool>("move", (args) =>
            {
                World world = (World)args[0];
                Player player = (Player)args[1];
                List<(int, int, string)> allowedMoves = (List<(int, int, string)>)args[2];

                string directionOutput = "move <";
                foreach(var move in allowedMoves)
                {
                    directionOutput += move.Item3 + '|';
                }
                directionOutput = directionOutput.TrimEnd('|');
                directionOutput += ">";

                return directionOutput;
            }, (args) => {
                World world = (World)args[0];
                Player player = (Player)args[1];
                string[] input = (string[])args[2];
                List<(int, int, string)> allowedMoves = (List<(int, int, string)>)args[3];

                if(input.Length != 2)
                {
                    Console.WriteLine("Invalid Argument Length.");
                    return false;
                }
                (int, int, string) move = allowedMoves.Find(x => x.Item3 == input[1].ToLower());
                if(move == (0, 0, null)) {
                    Console.WriteLine("Invalid 2nd argument.");
                    return false;
                } else {
                    world.LocalMoveEntity(player, move.Item1, move.Item2);
                    return true;
                }
            }),
            //display the world map
            new PlayerAction<bool>("map", (args) => "map", (args) => 
            {
                World world = (World)args[0];
                Player player = (Player)args[1];
                
                world.DisplayMap(player);
                return false;
            }),
            //exit from the game
            new PlayerAction<bool>("exit", (args) => "exit", (args) => 
            {
                System.Environment.Exit(0);
                return true;
            }),
            new PlayerAction<bool>("payday", (args) => "payday", (args) =>
            {   
                Player player = (Player)args[1];
                string[] input = (string[])args[2];

                //check for valid input
                if(input.Length != 2) {
                    Console.WriteLine("Invalid Argument Length.");
                } else if(int.TryParse(input[1], out int result)) {
                    //pay player
                    player.GainMoney(result);
                }
                return false;
            })
        }.ToList();
    }
}