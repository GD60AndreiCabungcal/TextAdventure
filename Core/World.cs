// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class World
    {
        public string Name { get; private set; }
        public Location[,] Locations { get; private set; }
        public List<Entity> Entities { get; set; }
        public List<Item> Items { get; set; }

        public World(string name, Location[,] locations, List<Entity> entities, List<Item> items)
        {
            Name = name;
            Locations = locations;
            Entities = entities;
            Items = items;
        }

        public Location GetLocation(int x, int y)
        {
            return (Location)Locations.GetValue(x,y);
        }

        public void MoveEntity(Entity entity, int x, int y)
        {
            entity.Position = new int[] { x, y };
        }

        public void EntityGetItem(Entity entity, Item item)
        {
            if(entity.Position == item.Position) {
                entity.Inventory.Add(item);
                Items.Remove(item);
            }
        }
    }
}