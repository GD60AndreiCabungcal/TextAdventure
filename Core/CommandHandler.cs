// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class CommandHandler
    {
                
        //directions for player movement
        static (int, int, string)[] directions = new (int, int, string)[] { (0,1, "south"), (0,-1, "north"), (1,0, "east"), (-1,0, "west") }; //(dirX, dirY, moveName)

        //list of all allowed moves to the player
        static List<(int, int, string)> allowedMoves = new List<(int, int, string)>();
        /*
        list of all the actions available to the player
        PlayerAction<turnPersist>
            turnPersist: if the turn should continue
        */
        public static List<PlayerAction<bool>> defaultCommands = new PlayerAction<bool>[]
        {
            //when player wants to check their stats
            new PlayerAction<bool>("stats", (args) => "stats", (args) => 
                {   
                    Player player = (Player)args[1];

                    string name = player.Name;
                    int health = player.Health;
                    int armor = player.Armor();
                    float money = player.Money;

                    //display stats
                    Console.WriteLine($"\n{name}'s Stats:\nHealth: {health}\nArmor: {armor}\nMoney: {money.ToString("C")}\n");
                    return false;
                }),
            //when player wants to check their inventory
            new PlayerAction<bool>("inventory", (args) => "inventory", (args) => {   
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
            new PlayerAction<bool>("eat", (args) => "eat $itemName", (args) => {
                Player player = (Player)args[1];
                string[] input = (string[])args[2];

                if(input.Length < 2) {
                    Console.WriteLine("Invalid Argument Length.");
                    return false;
                } else if (player.Inventory.Count(x => x.GetType() == typeof(Food)) <= 0) {
                    Console.WriteLine("You have no food!");
                    return false;
                }
                Food food = (Food)player.Inventory.Find(x => x.GetType() == typeof(Food) && x.Name.Replace(" ", "").ToLower() == string.Join("", input.Skip(1)).ToLower());
                player.Heal(food);
                return false;
            }),
            //move player to a new location
            new PlayerAction<bool>("move", (args) =>
            {
                World world = (World)args[0];
                Player player = (Player)args[1];

                //repopulate allowed moves
                allowedMoves.Clear();
                foreach(var direction in directions) 
                {
                    int dirX = player.Pos.x + direction.Item1;
                    int dirY = player.Pos.y + direction.Item2;

                    Location location = world.GetLocation(dirX, dirY);

                    //skip over null locations (used as walls)
                    if(location == null) continue;
                    //if the location is open, then add it to the allowdMoves
                    if(location.IsOpen) allowedMoves.Add(direction);
                }

                //setup move command for player
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

        /*
        commands for a player in battle
        PlayerAction<(turnPersist, battlePersist)>
            turnPersist: if the turn should continue
            battlePersist: if the battle should continue
        */
        public static List<PlayerAction<(bool, bool)>> battleCommands = new PlayerAction<(bool, bool)>[] {
            //when player wants to attack an enemy
            new PlayerAction<(bool, bool)>("attack", (args) => {
                
                Player player = (Player)args[0];
                List<Entity> defenders = (List<Entity>)args[1];

                string result = "attack $";
                foreach(Entity defender in defenders)
                {
                    result += defender.Name + '|';
                }
                result = result.TrimEnd('|');
                return result + " with fists|$itemName";
            }, (args) => {

                World world = (World)args[0];
                Player player = (Player)args[1];
                Weapon weapon = (Weapon)args[2];
                Entity defender = (Entity)args[3];

                if(weapon == null || defender == null)
                {
                    Console.WriteLine("Invalid weapon or opponent.");
                    //continue turn, continue battle
                    return (true, true);
                }

                //damage defender
                defender.TakeDamage(world, player, weapon);
                //end turn, continue battle
                return (false, true);
            }),
            //when player wants to eat food; eating food takes up a turn
            new PlayerAction<(bool, bool)>("eat", (args) => "eat $itemName", (args) => {
                World world = (World)args[0];
                Player player = (Player)args[1];
                Weapon weapon = (Weapon)args[2];
                Entity defender = (Entity)args[3];
                string[] input = (string[])args[4];

                if(input.Length < 2) {
                    Console.WriteLine("Invalid Argument Length.");
                    //continue turn, continue battle
                    return (true, true);
                } else if (player.Inventory.Count(x => x.GetType() == typeof(Food)) <= 0) {
                    Console.WriteLine("You have no food!");
                    //continue turn, continue battle
                    return (true, true);
                }
                Food food = (Food)player.Inventory.Find(x => x.GetType() == typeof(Food) && x.Name.Replace(" ", "").ToLower() == string.Join("", input.Skip(1)).ToLower());
                player.Heal(food);
                //end turn, continue battle
                return (false, true);
            }),
            //when the player wants to check their inventory; it doesn't take up a turn
            new PlayerAction<(bool, bool)>("inventory", (args) => "inventory", (args) => {   
                World world = (World)args[0];
                Player player = (Player)args[1];
                Weapon weapon = (Weapon)args[2];
                Entity defender = (Entity)args[3];
                string[] input = (string[])args[4];

                string inventory = "";
                foreach(Item item in player.Inventory)
                {
                    inventory += $"\n    {item.Stats()}";
                }
                Console.WriteLine($"\nInventory: {inventory}\n");
                //continue turn, continue battle
                return (true, true);
            }),
            new PlayerAction<(bool, bool)>("run", (args) => "run", (args) => {
                Console.WriteLine("You left the battle.");
                //end turn, end battle
                return (false, false);
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
    }
}