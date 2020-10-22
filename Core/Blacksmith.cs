// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Blacksmith : Location
    {
        public ShopOwner Owner { get; private set; }
        public float Price { get; private set; }

        public Blacksmith(string name, ShopOwner owner, float price, string description = "") : base(name, description)
        {
            Owner = owner;
            Price = price;
            Tag = "Blacksmith";
        }

        public override void LocationEvent(World world, Player player)
        {
            base.LocationEvent(world, player);

            //get all the armor from the player's inventory
            Armor[] armors = player.Inventory.OfType<Armor>().ToArray();
            string[] decisions = new string[armors.Length + 1];
            for(int i = 0; i < decisions.Length; i++) {
                decisions[i] = i < armors.Length ? $"{armors[i].Name} for {Price.ToString("C")}" : "Nothing";
            }
            
            //input validation loop
            while(true)
            {
                Console.WriteLine($"What armor do you want to repair?\nMoney: {player.Money.ToString("C")}");
                int answer = DecisionHandler.MakeDecision(decisions);
                if(answer == armors.Length) {
                    break;
                } else if(Price > player.Money) {
                    Console.WriteLine("You don't have enough money!");
                } else {
                    player.RepairArmor(armors[answer], Price);
                }
            }
        }

        public override char MapIcon()
        {
            return 'B';
        }
    }
}