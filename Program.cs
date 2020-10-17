// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


using TextAdventure.Core;

public class Program
{
    //path to the world file
    static string worldPath = Directory.GetCurrentDirectory() + "/world.json";

    static void Main(string[] args)
    {
        /*Game Map
         01234 <- col/x value  Map layout
        0   *      X - You Are Here
        1S*E*E     * - Location
        2  D       S - Shop
        3EI*SK     D - Door
        4X         K - Key
                   E - Enemy
        ^- row/y value

        get location: world.Locations[x,y]

        */
        World world = World.Load(worldPath);
        Player player = (Player)world.Entities[0];

        /* --- START OF GAME --- */
        Console.WriteLine("Hello traveler! What is your name?");
        player.Name = Console.ReadLine();
        Console.WriteLine($"Nice to meet you, {player.Name}. There's a few shops to the east if you want to check them out.");
        //game loop
        while(true) 
        {
            //interact with entities
            world.EntityInteract();
            //play the location event
            world.GetLocation(player).LocationEvent(world, player);
            //promp the player where to go next
            DecisionHandler.GetInput(world, player);
        }
    }
}
