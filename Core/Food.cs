// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public class Food : Item
    {
        public int HealPoints { get; private set; }

        public Food(string name, int healPoints, int x = 0, int y = 0, string description = "") : base(name, x, y, description)
        {
            HealPoints = healPoints;
        }
    }
}