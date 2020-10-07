// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public class Weapon : Item
    {
        public int Damage { get; private set; }
        public float CritChance { get; private set; }

        public Weapon(string name, int damage, float critChance, bool forceSingle = true, int x = 0, int y = 0, string description = "") : base(name, forceSingle, x, y, description)
        {
            Damage = damage;
            CritChance = critChance;
        }

        public override string Stats()
        {
            return $"{Name}({Damage}Atk {CritChance}Crt)";
        }
    }
}