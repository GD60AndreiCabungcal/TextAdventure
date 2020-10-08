// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public abstract class Entity
    {
        public string Name { get; set; }
        public Position Pos { get; set; }
        protected int health;
        public int Health { get { return health; } protected set { Math.Max(0, value); } }
        protected int armor;
        public int Armor
        {
            get
            {
                int armorPoints = 0;
                foreach(Item item in Inventory)
                {
                    if(item.GetType() != typeof(Armor)) continue;
                    Armor armor = (Armor)item;
                    armorPoints += armor.ArmorPoints;
                }
                return armorPoints;
            }
        }
            //used for defending attacks
        public List<Item> Inventory { get; protected set; }

        public Entity(string name, int x, int y, params Item[] inventory)
        {
            Name = name;
            Pos = new Position(x, y);
            health = 10;
            Inventory = inventory.ToList();
        }

        public virtual bool TakeDamage(int damage, float critChance = 1) //returns true if entity took damage
        {
            //rng is an instance of the Random class
            Random rng = new Random();
            //check if other entity critical hits player
            bool didCrit = critChance > rng.NextDouble();

            if(Armor > 0 && !didCrit) {
                foreach(Item item in Inventory)
                {
                    if(item.GetType() != typeof(Armor)) continue;
                    Armor armor = (Armor)item;
                    armor.ArmorPoints--;
                }
                Console.WriteLine($"{Name} defended the attack!");
                return false;
            } else {
                health -= damage;
                Console.WriteLine($"{Name} took {damage} damage!");
                if(health <= 0) Die();
                return true;
            }
        }

        public virtual bool TakeDamage(Weapon weapon)
        {
            return TakeDamage(weapon.Damage, weapon.CritChance);
        }

        public virtual void Die()
        {
            Console.WriteLine($"{Name} has died!");
        }

        public virtual void Heal(int heal) 
        {
            Health += heal;
            Console.WriteLine($"{Name} healed by {heal} points!");
        }

        public virtual void Heal(Food food)
        {
            if(!Inventory.Exists(x => x == food)) return;
            Heal(food.HealPoints);
            Inventory.Remove(food);
        }

        public virtual void Interact(World world, Entity entity)
        {

        }

        public override string ToString()
        {
            return $"{Name}({health}HP)";
        }
    }
}