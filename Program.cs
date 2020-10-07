// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

using TextAdventure.Core;

class Program
{
    static void Main(string[] args)
    {
        //object used for generating random numbers
        Random rng = new Random();

        /*
         01234
        0   *
        1*****
        2  * *
        3***S*
        4*
        */

        //user information
        Player player = new Player("", 0, 4);
        Merchant merchant1 = new Merchant("Merchant", 3, 3);

        //Entity Information
        List<Entity> entities = new Entity[] 
        {
            player,
            merchant1
        }.ToList();

        Location[,] map = new Location[/*col*/,/*row*/]
        {
            {
                null,
                new Location("Cliff", "It's very high up here."),
                null,
                new Location("Grass", "wow"),
                new Location("Starting area", "wow")
            },
            {
                null,
                new Location("Grass", "1,1"),
                null,
                new Location("Grass", "1,3"),
                null
            },
            {
                null,
                new Location("Grass", "2,1"),
                new Location("Grass", "2,2"),
                new Location("Grass", "2,3"),
                null
            },
            {
                new Location("Grass", "3,0"),
                new Location("Grass", "3,1"),
                null,
                new Shop("Merchant", merchant1, new ShopItem[] {
                    new ShopItem(new Food("Apple", 2), 2),
                    new ShopItem(new Weapon("Stone Sword", 1, 0.3f), 5)
                }),
                null
            },
            {
                null,
                new Location("Grass", "4,1"),
                new Location("Grass", "4,2"),
                new Location("Grass", "4,3"),
                null
            },
        };

        //World information
        World world = new World("Overworld", map, entities, null);

        /* --- START OF GAME --- */
        Console.WriteLine("Hello traveler! What is your name?");
        player.Name = Console.ReadLine();
        Console.WriteLine($"Nice to meet you, {player.Name}. Welcome to the village! Make yourself feel at home here.");
        while(true) 
        {
            world.GetLocation(player).LocationEvent(player);
            DecisionHandler.GetInput(world, player);
        }
    }
}
