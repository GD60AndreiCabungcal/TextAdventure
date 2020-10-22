// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Armor : Item
    {
        int armor; //current armor value; gets smaller during battle
        int maxArmor; //max armor value; the value armor will be when repaired
        public int ArmorPoints { get { return armor; } set { armor = Math.Max(0, value); } } //how many points this armor will protect the entity
        int speed;
        public int SpeedPoints { get { return speed; } set { speed = value; } } //how many points this this armor will prioritize the enitiy's attacks
        
        public Armor(string name, int armorPoints, int speedPoints, string tag, bool forceSingle = true, int x = 0, int y = 0, string description = "") : base(name, tag, forceSingle, x, y, description)
        {
            maxArmor = armorPoints;
            ArmorPoints = armorPoints;
            SpeedPoints = speedPoints;
        }

        public void Repair()
        {
            armor = maxArmor;
        }

        public override string Stats()
        {
            return $"{Name}({armor}/{maxArmor}Amr)({SpeedPoints}Spd)";
        }
    }
}