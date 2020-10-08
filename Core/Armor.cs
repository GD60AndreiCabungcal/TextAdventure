// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Armor : Item
    {
        int armor;
        public int ArmorPoints { get { return armor; } set { Math.Max(0, value); } } //how many points this armor will protect the entity

        public Armor(string name, int armorPoints, string tag, bool forceSingle = true, int x = 0, int y = 0, string description = "") : base(name, tag, forceSingle, x, y, description)
        {
            armor = armorPoints;
        }

        public override string Stats()
        {
            return $"{Name}({ArmorPoints}Amr)";
        }
    }
}