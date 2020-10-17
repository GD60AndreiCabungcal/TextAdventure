// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class GameDictionary
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
            new PlayerAction<bool>("eat", (args) => "eat $itemName", (args) => 
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
                List<(int, int, string)> allowedMoves = GetAllowableMoves(world, player);

                string directionOutput = "move $";
                foreach(var move in allowedMoves)
                {
                    directionOutput += move.Item3 + '|';
                }
                directionOutput = directionOutput.TrimEnd('|');

                return directionOutput;
            }, (args) => {
                World world = (World)args[0];
                Player player = (Player)args[1];
                string[] input = (string[])args[2];
                List<(int, int, string)> allowedMoves = GetAllowableMoves(world, player);

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
            //player wants to drop an item
            new PlayerAction<bool>("drop", (args) => "drop <all> $itemName", (args) => {
                World world = (World)args[0];
                Player player = (Player)args[1];
                string[] input = (string[])args[2];

                string itemName;

                if(input.Length < 2) {
                    Console.WriteLine("Invalid Argument Length.");
                } else if(input[1] == "all") {
                    itemName = string.Join(" ", input.Skip(2));
                    List<Item> items = player.Inventory.FindAll(x => x.Name == itemName);
                    if(items.Count == 0) {
                        Console.WriteLine("You don't have those items!");
                    } else {
                        items.ForEach(item => world.EntityDropItem(player, item));
                        Console.WriteLine($"You dropped {items.Count} {itemName}s.");
                    }
                } else {
                    itemName = string.Join(" ", input.Skip(1));
                    Item item = player.Inventory.Find(x => x.Name == itemName);
                    if(item == null) { 
                        Console.WriteLine("You don't have that item!");
                    } else {
                        world.EntityDropItem(player, item);
                        Console.WriteLine($"You dropped {(item.ForceSingle ? "the" : IsVowel(itemName[0]) ? "an" : "a")} {itemName}.");
                    }
                }
                return false;
            }),
            //exit from the game
            new PlayerAction<bool>("exit", (args) => "exit", (args) => 
            {
                System.Environment.Exit(0);
                return true;
            })
        }.ToList();

        static bool IsVowel(char c)
        {
            string vowels = "aeiouAEIOU";
            foreach(char vowel in vowels)
            {
                if(c == vowel) return true;
            }
            return false;
        }

        static List<(int, int, string)> GetAllowableMoves(World world, Player player)
        {
            //setup allowable move directions
            (int, int, string)[] directions = new (int, int, string)[] { (0,1, "south"), (0,-1, "north"), (1,0, "east"), (-1,0, "west") }; //(dirX, dirY, moveName)
            List<(int, int, string)> allowedMoves = new List<(int, int, string)>();

            foreach(var direction in directions) {
                int dirX = player.Pos.x + direction.Item1;
                int dirY = player.Pos.y + direction.Item2;

                //if the direction is out of bounds, then skip it
                if((dirX < 0 || dirX >= world.Locations.GetLength(0)) || ((dirY < 0 || dirY >= world.Locations.GetLength(1)))) continue;
                Location location = world.Locations[dirX, dirY];

                //check the tag of the current location
                if(location == null) continue;
                //if the location is open, then add it to the allowdMoves
                if(location.IsOpen(player)) allowedMoves.Add(direction);
            }

            return allowedMoves;
        }
    }
}