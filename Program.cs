// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

using TextAdventure.Core;

class Program
{
    static void Main(string[] args)
    {
        //default player
        Player player = new Player("", 0, 4);

        //entity Information
        List<Entity> entities = new Entity[] 
        {
            player,
            new Enemy("Bababouy", 0, 3),
            new Enemy("Gogy", 3, 1, new Armor("Shiny Chestplate", 4, "Chestplate"))
        }.ToList();

        //item information
        List<Item> items = new Item[]
        {
            new Armor("Wooly Boots", 2, "Boots", x: 4, y: 2),
            new Armor("Wooly Cloak", 3, "Chestplate", x: 1, y: 3)
        }.ToList();

        //map information
        Location[,] map = GameDictionary.MAP;

        //World information
        World world = new World("Overworld", map, entities, items);

        /* --- START OF GAME --- */
        Console.WriteLine("Hello traveler! What is your name?");
        player.Name = Console.ReadLine();
        Console.WriteLine($"Nice to meet you, {player.Name}. There's a few shops to the east if you want to check them out.");
        //game loop
        while(true) 
        {
            //play the location event
            world.GetLocation(player).LocationEvent(world, player);
            //interact with entities
            world.EntityInteract();
            //promp the player where to go next
            DecisionHandler.GetInput(world, player);
        }
    }
}
