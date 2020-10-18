// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//package for json serialization / deserialization
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace TextAdventure.Core
{
    public class World
    {
        public string Name { get; private set; }
        public Location[,] Locations { get; private set; }
        public List<Entity> Entities { get; set; }
        public List<Item> Items { get; set; }

        //settings for serialization / deserialization
        static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
             TypeNameHandling = TypeNameHandling.Objects
        };

        public World(string name, Location[,] locations, List<Entity> entities, List<Item> items)
        {
            Name = name;
            Locations = locations;
            Entities = entities;
            Items = items;
        }

        public bool IsLocation(int x, int y) {
            return (x >= 0 && x < Locations.GetLength(0)) && ((y >= 0 && y < Locations.GetLength(1)));
        }
        
        //returns a given location based on the coordinates
        public Location GetLocation(int x, int y)
        {
            return IsLocation(x, y) ? Locations[x, y] : null;
        }

        public Location GetLocation(Entity entity)
        {
            return GetLocation(entity.Pos.x, entity.Pos.y);
        }

        public Location GetLocation(Item item)
        {
            return GetLocation(item.Pos.x, item.Pos.y);
        }

        //moves an entity to an absloute position
        public void WorldMoveEntity(Entity entity, int x, int y)
        {
            entity.Pos = new Position(x, y);
        }

        //moves an entity relative to its position
        public void LocalMoveEntity(Entity entity, int xDir, int yDir)
        {
            entity.Pos = new Position (entity.Pos.x + xDir, entity.Pos.y + yDir);
        }

        //adds item to entity's inventory
        public void EntityGetItem(Entity entity, Item item)
        {
            if(entity.Pos == item.Pos) {
                item.Pos = new Position(-1, -1);
                entity.Inventory.Add(item);
                Items.Remove(item);
            }
        }

        public void EntityDropItem(Entity entity, Item item)
        {
            if(entity.Inventory.Contains(item)) {
                item.Pos = entity.Pos;
                Items.Add(item);
                entity.Inventory.Remove(item);
            }
        }

        public void EntityInteract(Player player)
        {
            Entity entity = Entities.Find(e => e != player && e.Pos == player.Pos);
            if(entity == null) return;
            else entity.Interact(this, player);
        }

        //displays the current map
        public void DisplayMap(Player player)
        {
            Console.WriteLine($"- - -({Name})- - -\n     north     \n       ^       \nwest <   > east\n       v       \n     south     ");

            //initalizing the legend of the map
            List<string> legend = new List<string>();
            legend.Add("X - You are here");
            for(int row = 0; row < Locations.GetLength(0); row++)
            {
                for(int col = 0; col < Locations.GetLength(1); col++)
                {   
                    Location location = Locations[row, col];
                    if(location == null) continue;
                    string entry = $"{location.MapIcon()} - {location.GetType().Name}";
                    if(!legend.Contains(entry)) {
                        legend.Add(entry);
                    }
                }
            }

            //generate map
            for(int row = 0; row < Locations.GetLength(0); row++)
            {
                for(int col = 0; col < Locations.GetLength(1); col++)
                {
                    char mapIcon = ' ';
                    Location location = Locations[col, row];
                    if (location == null) {

                    } else if(location.Equals(GetLocation(player))) {
                        mapIcon = 'X';
                    } else {
                        mapIcon = location.MapIcon();
                    }
                    Console.Write(mapIcon);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"- - -({Name})- - -");
            foreach(string entry in legend) Console.WriteLine(entry);
        }
        
        public static World Load(string path)
        {
            //get file from path and read all text from it
            string json = File.ReadAllText(path);

            //deserialize json into world object
            World world = JsonConvert.DeserializeObject<World>(json, settings);
            return world;
        }

        public void Save(string path)
        {
            //open or create file, then dispose the filestream
            FileStream fs = File.Open(path, FileMode.OpenOrCreate);
            fs.Dispose();

            //get json from world object
            string json = JsonConvert.SerializeObject(this, Formatting.Indented, settings);
            
            //write world json to file
            File.WriteAllText(path, json);
        }
    }
}