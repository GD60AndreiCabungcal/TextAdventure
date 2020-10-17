// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Enemy : Entity
    {
        public Enemy(string name, float money, int x, int y, params Item[] inventory) : base(name, x, y, inventory) 
        { 
            Money = money;
        }

        public override void Interact(World world, Entity entity)
        {
            if(entity.GetType() != typeof(Player)) return;
            Player player = (Player)entity;

            Console.WriteLine($"{Name}: Hey you, let's fight!");
            world.Fight(this, player);
        }

        public override void Die(Entity killedBy)
        {
            base.Die(killedBy);
            killedBy.GainMoney(Money);
        }
    }
}