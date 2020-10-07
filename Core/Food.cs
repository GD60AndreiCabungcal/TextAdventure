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
        int armor;
        public int ArmorPoints { get { return armor; } private set { Math.Clamp(value, 0, 3); } }

        public Food(string name, int healPoints, int armorPoints = 0, int x = 0, int y = 0, string description = "") : base(name, x, y, description)
        {
            health = healPoints;
            armor = armorPoints;
        }

        public override string Stats()
        {
            return $"{Name}({HealPoints}HP)";
        }
    }
}