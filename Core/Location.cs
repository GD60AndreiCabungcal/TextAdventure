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

        public Location(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual void LocationEvent(World world, Player player)
        {
            //display location
            Console.WriteLine($"\n{Name}:\n{Description}\n");

            //find any items
            List<Item> items = world.Items.FindAll(item => world.GetLocation(item) == this);
            if(items.Count <= 0) return;

            //prompt user if they want to get an item
            Console.WriteLine($"{player.Name}, there are items here! what will you pick up?");

            string[] decisions = new string[items.Count + 1];
            for(int i = 0; i < decisions.Length; i++)
            {
                decisions[i] = i < items.Count ? items[i].Name : "Nothing";
            }
            int itemIndex = DecisionHandler.MakeDecision(decisions, "You picked up");
            //if player chooses the last option, then they don't pick up anything
            if(itemIndex != items.Count) world.EntityGetItem(player, items[itemIndex]);
        }

        public virtual string MapIcon()
        {
            return "*";
        }
    }
}