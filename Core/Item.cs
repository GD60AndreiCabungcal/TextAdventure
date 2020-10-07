// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public Position Pos { get; set; }
        public string Description { get; protected set; }

        public Item(string name, int x = 0, int y = 0, string description = "")
        {
            Name = name;
            Pos = new Position(x, y);
            Description = description;
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual string Stats()
        {
            return $"{Name} {Description}";
        }
    }
}