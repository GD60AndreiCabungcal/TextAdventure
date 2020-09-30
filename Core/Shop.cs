using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Shop : Location
    {
        public Entity Owner { get; private set; }
        public List<ShopItem> ShopItems { get; set; }

        public Shop(string name, Entity owner, List<ShopItem> shopItems, string description = "") : base(name, description, null)
        {
            Owner = owner;
            ShopItems = shopItems;
        }
    }
    
    public struct ShopItem
    {
        public Item item;
        public float price;
    }
}