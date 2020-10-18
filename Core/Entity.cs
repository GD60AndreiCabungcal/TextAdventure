// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;


namespace TextAdventure.Core
{
    public abstract class Entity
    {
        public string Name { get; set; }
        public string Team { get; set; }
        public Position Pos { get; set; }
        int health;
        public int Health { get { return health; } protected set { health = Math.Max(0, value); } }

        float money;
        public float Money { get { return MathF.Round(money, 2); } protected set { money = value; } }

        public List<Item> Inventory { get; protected set; }

        public Entity(string name, string team, int x, int y, params Item[] inventory)
        {
            Name = name;
            Team = team;
            Pos = new Position(x, y);
            Health = 10;
            Inventory = inventory.ToList();
        }

        //how much damage the entity can deflect
        public int Armor()
        {
            int armorPoints = 0;
            foreach(Item item in Inventory)
            {
                if(item.GetType() != typeof(Armor)) continue;
                Armor armor = (Armor)item;
                armorPoints += armor.ArmorPoints;
            }
            return Math.Max(0, armorPoints);
        }

        //how fast the entity will attack
        public int Speed()
        {
            int speedPoints = 0;
            foreach(Item item in Inventory)
            {
                if(item.GetType() != typeof(Armor)) continue;
                Armor armor = (Armor)item;
                speedPoints += armor.SpeedPoints;
            }
            return Math.Max(0, speedPoints);
        }

        //when entity takes damage
        public virtual bool TakeDamage(World world, Entity damagedBy, Weapon weapon) //returns true if entity took damage
        {
            //rng is an instance of the Random class
            Random rng = new Random();
            //check if other entity critical hits player
            bool didCrit = weapon.CritChance > rng.NextDouble();

            //damage all armor items if the entity defended the attack
            if(Armor() > 0 && !didCrit) {
                foreach(Item item in Inventory)
                {
                    if(item.GetType() != typeof(Armor)) continue;
                    Armor armor = (Armor)item;
                    armor.ArmorPoints -= weapon.Damage;
                }
                Console.WriteLine($"{Name} defended {damagedBy.Name}'s attack!");
                return false;
            //damage entity by base damage and crit damage
            } else {
                Health -= weapon.Damage;
                Console.Write($"{Name} took {weapon.Damage} damage");
                if(didCrit)
                {
                    int critDamage = (weapon.Damage / 2) + 1;
                    Health -= critDamage;
                    Console.Write($" + {critDamage} critical damage");
                }
                Console.WriteLine($" from {damagedBy.Name}'s {weapon.Name}!");
                if(health <= 0) Die(world, damagedBy);
                return true;
            }
        }

        //when entity dies
        public virtual void Die(World world, Entity killedBy)
        {
            Console.WriteLine($"{killedBy.Name} has killed {Name}!");
            //entity drops all their items
            for(int i = 0; i < Inventory.Count; i++) world.EntityDropItem(this, Inventory[0]);
            //killedBy gains money
            killedBy.GainMoney(Money);
            //entity gets remove from the world
            world.Entities.Remove(this);
        }

        //when entity heals
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

        public override string ToString()
        {
            return $"{Name}({health}HP){(Armor() > 0 ? $"({Armor()}Amr)" : "")}";
        }

        //when entity gains money
        public void GainMoney(float amount)
        {
            Money += amount;
            Console.WriteLine($"{Name} got paid {amount.ToString("C")}!");
        }

        public virtual void Interact(World world, Entity entity) { }
    }
}