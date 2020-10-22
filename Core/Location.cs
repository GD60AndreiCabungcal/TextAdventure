// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Location
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Tag { get; protected set; }
        public bool IsOpen { get; protected set; } = true;

        public Location(string name, string description)
        {
            Name = name;
            Description = description;
        }

        //when the player is at this location
        public virtual void LocationEvent(World world, Player player)
        {
            //display location
            Console.WriteLine($"\n{Name}:\n{Description}\n");

            //for adjacent locations
            int[,] directions = new int[,] { {0, 1}, {0, -1}, {1, 0}, {-1, 0} };

            //find any items
            List<Item> items = world.Items.FindAll(item => world.GetLocation(item) == this);
            //if there are any items, prompt user if they want to get an item
            if(items.Count > 0) {
                Console.WriteLine($"{player.Name}, there are items here! what will you pick up?");

                //item pickup loop
                while(true)
                {
                    //setup decisions for the player
                    string[] decisions = new string[items.Count + 1];
                    for(int i = 0; i < decisions.Length; i++)
                    {
                        decisions[i] = i < items.Count ? items[i].Name : "Nothing";
                    }
                    //prompt player
                    int itemIndex = DecisionHandler.MakeDecision(decisions, "You picked up");
                    //if player chooses the last option, then they don't pick up anything
                    if(itemIndex != items.Count) {
                        //drop any items of a similar type that are forced single
                        Item similarItem = player.Inventory.Find(x => x.ForceSingle == true && x.Tag == items[itemIndex].Tag);
                        if(similarItem != null) {
                            Console.WriteLine($"You dropped {similarItem.Name} in it's place.");
                            world.EntityDropItem(player, similarItem);
                        }
                        //pick up item
                        Item item = items[itemIndex];
                        world.EntityGetItem(player, item);
                        items.Remove(item);
                    } else break;
                    if(items.Count == 0) break;
                }
            }

            //run the adjacent location events
            for(int i = 0; i < directions.GetLength(0); i++)
            {
                int dirX = player.Pos.x + directions[i,0];
                int dirY = player.Pos.y + directions[i,1];

                Location adjacent = world.GetLocation(dirX, dirY);
                if(adjacent == null) continue;
                adjacent.AdjacentEvent(world, player);
            }
        }

        //when the player is beside this location
        protected virtual void AdjacentEvent(World world, Player player)
        {

        }

        public virtual char MapIcon()
        {
            return '*';
        }
    }
}