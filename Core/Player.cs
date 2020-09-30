// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public class Player : Entity
    {
        public int Fullness { get; private set; } //10 is full, 0 is hungry
        public float Money { get; private set; }

        public Player(string name, int x = 0, int y = 0) : base(name, x, y) 
        {
            Fullness = 10;
            Money = 50;
        }

        public void PurchaceItem(Item item, float price)
        {
            Inventory.Add(item);
            Money -= price;
        }
    }
}