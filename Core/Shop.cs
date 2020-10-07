// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Shop : Location
    {
        public Merchant Owner { get; private set; }
        public ShopItem[] ShopItems { get; set; }

        public Shop(string name, Merchant owner, ShopItem[] shopItems, string description = "") : base(name, description)
        {
            Owner = owner;
            ShopItems = shopItems;
        }

        public override void LocationEvent(Player player)
        {
            base.LocationEvent(player);

            string[] decisions = new string[ShopItems.Length + 1];
            for(int i = 0; i < ShopItems.Length; i++) {
                decisions[i] = $"{ShopItems[i].item} for ${ShopItems[i].price}";
            }
            decisions[ShopItems.Length] = "Nothing";

            while(true)
            {
                Console.WriteLine($"What do you wnat to buy?\nMoney: ${player.Money}");
                int answer = DecisionHandler.MakeDecision(decisions);
                if(answer == ShopItems.Length) {
                    break;
                } else if(ShopItems[answer].price > player.Money) {
                    Console.WriteLine("You can't buy that!");
                } else {
                    player.PurchaceItem(ShopItems[answer]);
                }
            }
        }

        public override string MapIcon()
        {
            return "S";
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