// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class CommandList
    {
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
                    int fullness = player.Fullness;
                    float money = player.Money;
                    
                    string allies = "";
                    foreach(Entity ally in player.Allies)
                    {
                        allies += $"\n    {ally}";
                    }

                    //display stats
                    Console.WriteLine($"\n{name}'s Stats:\nHealth: {health}\nArmor: {armor}\nFullness: {fullness}\nMoney: {money}\nAllies: {allies}");
                    return true;
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
                    Console.WriteLine($"\nInventory: {inventory}");
                    return true;
                }),
            //when player wants to eat
            new PlayerAction<bool>("eat", (args) => "eat", (args) => 
                {
                    Player player = (Player)args[1];
                    string[] input = (string[])args[2];

                    if(input.Length != 2) {
                        Console.WriteLine("Invalid Argument Length.");
                        return false;
                    } else if (player.Inventory.Count(x => x.GetType() == typeof(Food)) <= 0) {
                        Console.WriteLine("You have no food!");
                        return false;
                    }
                    Food food = (Food)player.Inventory.Find(x => x.GetType() == typeof(Food) && x.Name.ToLower() == input[1].ToLower());
                    player.Heal(food);
                    return true;
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
                world.DisplayMap();
                return false;
            }),
            //exit from the game
            new PlayerAction<bool>("exit", (args) => "exit", (args) => 
            {
                System.Environment.Exit(0);
                return true;
            })
        }.ToList();
    }
}