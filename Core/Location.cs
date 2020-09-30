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

        public Location(string name, string description, List<Item> items)
        {
            Name = name;
            Description = description;
            Items = items;
        }
    }
}