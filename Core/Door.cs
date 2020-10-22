// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Door : Location 
    {
        public List<string> Passwords { get; private set; }

        public Door(string name, string description = "", params string[] passwords) : base(name, description)
        {
            Passwords = passwords.ToList();
            Tag = "Door";
            IsOpen = false;
        }

        protected override void AdjacentEvent(World world, Player player)
        {
            base.AdjacentEvent(world, player);
            
            if(IsOpen) return;
            //prompt user to open door 
            Console.WriteLine($"You found {Name}. Do you want to unlock it?");
            int answer = DecisionHandler.MakeDecision(new string[] { "Yes", "No" }, "");
            if(answer == 0) {
                //get all keys in their inventory
                List<Key> keys = player.Inventory.OfType<Key>().ToList();

                //try unlocking door
                if(TryOpen(keys)) {
                    Console.WriteLine($"You unlocked {Name}!\n");
                    IsOpen = true;
                } else {
                    Console.WriteLine($"You don't have the right keys.\n");
                }
            }
        }

        public bool TryOpen(List<Key> entityKeys)
        {
            if(entityKeys.Count <= 0) return false;
            //if the door is lock, then try to unlock the door
            if(!IsOpen) {
                //if the entity has all the EXACT keys that the door requires, then unlock the door
                foreach(Key entityKey in entityKeys)
                {
                    if(!Passwords.Exists(doorKey => doorKey == entityKey.Password)) {
                        return false;
                    }
                }
                IsOpen = true;
            }
            return IsOpen;
        }

        public override char MapIcon()
        {
            return 'D';
        }
    }
}