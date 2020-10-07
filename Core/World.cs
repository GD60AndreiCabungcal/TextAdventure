// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class World
    {
        public string Name { get; private set; }
        public Location[,] Locations { get; private set; }
        public List<Entity> Entities { get; set; }
        public List<Item> Items { get; set; }

        public World(string name, Location[,] locations, List<Entity> entities, List<Item> items)
        {
            Name = name;
            Locations = locations;
            Entities = entities;
            Items = items;
        }
        
        //returns a given location based on the coordinates
        public Location GetLocation(int x, int y)
        {
            return (Location)Locations.GetValue(x,y);
        }

        public Location GetLocation(Entity entity)
        {
            return (Location)Locations.GetValue(entity.Pos.x, entity.Pos.y);
        }

        public Location GetLocation(Item item)
        {
            return (Location)Locations.GetValue(item.Pos.x, item.Pos.y);
        }

        //moves an entity to an absloute position
        public void WorldMoveEntity(Entity entity, int x, int y)
        {
            entity.Pos = new Position(x, y);
        }

        //moves an entity relative to its position
        public void LocalMoveEntity(Entity entity, int xDir, int yDir)
        {
            entity.Pos = new Position (entity.Pos.x + xDir, entity.Pos.y + yDir);
        }

        //adds item to entity's inventory
        public void EntityGetItem(Entity entity, Item item)
        {
            if(entity.Pos == item.Pos) {
                entity.Inventory.Add(item);
                Items.Remove(item);
            }
        }

        public void EntityInteract()
        {
            Player player = (Player)Entities.Find(e => e.GetType() == typeof(Player));
            Entity entity = Entities.Find(e => e != player && e.Pos == player.Pos);
            if(entity == null) return;
            else entity.Interact(this, player);
        }

        public void Fight(params Entity[] entities)
        {
            //rng is used to generate random numbers
            Random rng = new Random();

            Console.WriteLine("---/FIGHT/---");
            bool fightActive = true;
            while(fightActive)
            {
                foreach(Entity entity in entities)
                {
                    Entity offender;  //the entity that's attacking
                    Entity defender;  //the entity that's being attacked
                    Weapon weapon;    //the weapon the offender uses

                    //get what the entity can use
                    //check if current entity is player
                    //  -> prompt dialogue for player only

                    List<Weapon> weapons = entity.Inventory.OfType<Weapon>().ToList();
                    weapons.Add(new Weapon("Fist", 1, 0));
                    
                    offender = entity;

                    bool isPlayer = entity.GetType() == typeof(Player);
                    if(isPlayer) {
                        //setup combat text
                        string combatants = "";
                        for(int i = 0; i < entities.Length; i++)
                        {
                            combatants += $"{entities[i]} ";
                            if(i < entities.Length - 1) combatants += "vs. ";
                        }

                        Console.WriteLine(combatants);
                        Console.WriteLine("What do you want to do?");
                        int answer = DecisionHandler.MakeDecision(new string[] { "Attack", "Eat", "Run Away" }, "You chose to");
                        switch(answer)
                        {
                            case 1:
                            {   
                                Food[] foods = entity.Inventory.OfType<Food>().ToArray();
                                int foodCount = foods.Length;
                                //check if player has food
                                if(foodCount == 0) {
                                    Console.WriteLine($"{entity.Name} has no food!");
                                    continue;
                                }
                                //prompt user what to eat
                                Console.WriteLine("What do you want to eat?");

                                string[] foodOptions = new string[foods.Length + 1];
                                for(int i = 0; i < foodOptions.Length; i++)
                                {
                                    foodOptions[i] = i != foods.Length ? foods[i].Stats() : "Back";
                                }
                                int option = DecisionHandler.MakeDecision(foodOptions);
                                //if user chooses the last option, then they go back to battle
                                if(option != foodCount) {
                                    entity.Heal(foods[option]);
                                }
                                continue;
                                //player takes a whole turn to eat
                            }
                            case 2:
                            {
                                //exit fight if user chooses to run away
                                return;
                            }
                            default: break;
                        }
                        Console.WriteLine("What do you want to use?");
                    }                       
                    //setup decisions for which weapon to use
                    List<(string, int)> entries; //option, index
                    List<string> decisions;

                    entries = new List<(string, int)>();
                    decisions = new List<string>();
                    for(int i = 0; i < weapons.Count; i++)
                    {
                        entries.Add((weapons[i].Name, i));
                        decisions.Add(weapons[i].Name);
                    }
                    int weaponIndex = isPlayer ? DecisionHandler.MakeDecision(decisions, "") : rng.Next(entries.Count);
                    weapon = weapons[entries[weaponIndex].Item2];

                    if(isPlayer) Console.WriteLine("Who do you want to attack?");
                    //setup decisons for which entity to attack
                    entries = new List<(string, int)>();
                    decisions = new List<string>();

                    for(int i = 0; i < entities.Length; i++)
                    {
                        if(entities[i] == entity) continue;
                        entries.Add((entities[i].Name, i));
                        decisions.Add(entities[i].Name);
                    }
                    int entityIndex = isPlayer ? DecisionHandler.MakeDecision(decisions, "") : rng.Next(entries.Count);
                    defender = entities[entries[entityIndex].Item2];

                    //offender attacks defender with weapon
                    Console.WriteLine($"{offender.Name} attacked {defender.Name} with {weapon.Name}!");
                    defender.TakeDamage(weapon);

                    //check if defender is dead
                    if(defender.Health <= 0)
                    {
                        Console.WriteLine($"{offender.Name} killed {defender.Name}!");
                        fightActive = false;
                        Entities.Remove(defender);
                    }
                }
            }
        }

        //displays the current map
        public void DisplayMap()
        {
            Console.WriteLine($"- - -({Name})- - -");
            for(int row = 0; row < Locations.GetLength(0); row++)
            {
                for(int col = 0; col < Locations.GetLength(1); col++)
                {
                    Location location = Locations[col, row];
                    Console.Write(location != null ? location.MapIcon() : " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"- - -({Name})- - -");
        }
    }
}