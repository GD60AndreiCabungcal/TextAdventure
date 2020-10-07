// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public abstract class Entity
    {
        public string Name { get; set; }
        public int[] Position { get; set; }
        public int Health { get; protected set; }
        public int Armor { get; set; } //3 is full, 0 is empty; used for defending attacks
        public List<Item> Inventory { get; protected set; }
        public List<Entity> Allies { get; protected set; }

        public Entity(string name, int x, int y)
        {
            Name = name;
            Position = new int[] { x, y };
            Health = 10;
            Inventory = new List<Item>();
            Allies = new List<Entity>();
        }

        public virtual bool TakeDamage(int damage, float critChance = 1) //returns true if entity took damage
        {
            //rng is an instance of the Random class
            Random rng = new Random();
            //check if other entity critical hits player
            bool didCrit = critChance > rng.NextDouble();

            if(Armor > 0 && !didCrit) {
                Armor--;
                Console.WriteLine($"{Name} defended the attack!");
                return false;
            } else {
                Health -= damage;
                Console.WriteLine($"{Name} took {damage} damage!");
                return true;
            }
        }

        public virtual bool TakeDamage(Weapon weapon)
        {
            return TakeDamage(weapon.Damage, weapon.CritChance);
        }

        public virtual void Heal(int heal) 
        {
            Health += heal;
            Console.WriteLine($"{Name} healed by {heal} points!");
        }
    }
}