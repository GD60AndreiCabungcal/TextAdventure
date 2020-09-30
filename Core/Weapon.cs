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

        public Weapon(string name, int damage, float critChance, int x = 0, int y = 0, string description = "") : base(name, x, y, description)
        {
            Damage = damage;
            CritChance = critChance;
        }
    }
}