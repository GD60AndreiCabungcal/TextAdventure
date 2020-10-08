// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public class Player : Entity
    {
        public float Money { get; private set; }

        public Player(string name = "Unknown", int x = 0, int y = 0) : base(name, x, y) 
        {
            Money = 15;
        }

        public override void Die()
        {
            Console.WriteLine("A hero has fallen.");
            System.Environment.Exit(0);
        }

        public void PurchaceItem(Item item, float price)
        {
            if(item.ForceSingle && Inventory.Exists(i => i.Tag == item.Tag)) {
                Console.WriteLine($"You can't have more than 1 of {item.Name}!");
                return;
            }
            Inventory.Add(item);
            Money -= price;
        }

        public void PurchaceItem(ShopItem shopItem) => PurchaceItem(shopItem.item, shopItem.price);

        public void GainMoney(int amount)
        {
            Money += amount;
            Console.WriteLine($"{Name} got paid ${amount}!");
        }
    }
}