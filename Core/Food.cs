// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public class Food : Item
    {
        int health;
        public int HealPoints { get { return health; } private set { Math.Max(0, value); } } //how many points this food will heal the entity

        public Food(string name, int healPoints, string tag = "Food", bool forceSingle = false, int x = 0, int y = 0, string description = "") : base(name, tag, forceSingle, x, y, description)
        {
            health = healPoints;
        }

        public override string Stats()
        {
            return $"{Name}({HealPoints}HP)";
        }
    }
}