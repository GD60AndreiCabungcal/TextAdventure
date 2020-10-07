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
        public List<Item> Items { get; set; }

        public Location(string name, string description, params Item[] items)
        {
            Name = name;
            Description = description;
            Items = items.ToList();
        }

        public virtual void LocationEvent(Player player)
        {
            Console.WriteLine($"\n{Name}:\n{Description}\n");
        }
    }
}