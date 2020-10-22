// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Shop : Location
    {
        public ShopOwner Owner { get; private set; }
        public ShopItem[] ShopItems { get; set; }

        public Shop(string name, ShopOwner owner, ShopItem[] shopItems, string description = "") : base(name, description)
        {
            Owner = owner;
            ShopItems = shopItems;
            Tag = "Shop";
        }

        public override void LocationEvent(World world, Player player)
        {
            base.LocationEvent(world, player);

            string[] decisions = new string[ShopItems.Length + 1];
            for(int i = 0; i < decisions.Length; i++) {
                decisions[i] = i < ShopItems.Length ? $"{ShopItems[i].item} for {ShopItems[i].price.ToString("C")}" : "Nothing";
            }
            
            while(true)
            {
                Console.WriteLine($"What do you want to buy?\nMoney: {player.Money.ToString("C")}");
                int answer = DecisionHandler.MakeDecision(decisions);
                if(answer == ShopItems.Length) {
                    break;
                } else if(ShopItems[answer].price > player.Money) {
                    Console.WriteLine("You don't have enough money!");
                } else {
                    player.PurchaceItem(ShopItems[answer]);
                }
            }
        }

        public override char MapIcon()
        {
            return 'S';
        }
    }
    
    public struct ShopItem
    {
        public Item item;
        public float price;

        public ShopItem(Item item, float price)
        {
            this.item = item;
            this.price = price;
        }
    }
}