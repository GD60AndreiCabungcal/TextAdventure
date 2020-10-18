// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Enemy : Entity
    {
        public Enemy(string name, string team, float money, int x, int y, params Item[] inventory) : base(name, team, x, y, inventory) 
        { 
            Money = money;
        }

        public override void Interact(World world, Entity entity)
        {
            //do nothing if the entity isn't a player
            if(entity.GetType() != typeof(Player)) return;

            //get all entities with the same position as this enemy
            Entity[] entities = world.Entities.FindAll(e => e.Pos == this.Pos).ToArray();
            Enemy[] enemies = entities.OfType<Enemy>().ToArray();

            //print all the enemies the player is going to battle
            for(int i = 0; i < enemies.Count(); i++)
            {
                if(enemies[i] == this) {
                    Console.WriteLine($"{Name}: Hey you! Let's fight!");
                } else {
                    Console.WriteLine($"{enemies[i].Name}: Me too!");
                }
            }

            //start battle
            Simulator.StartBattle(world, entities);
        }

        public override void Die(Entity killedBy)
        {
            base.Die(killedBy);
            killedBy.GainMoney(Money);
        }
    }
}