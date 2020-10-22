// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Simulator
    {
        public static void StartBattle(World world, params Entity[] entities)
        {
            //for other entity's decisions
            Random rng = new Random();

            //copy of all the entities in battle
            List<Entity> combatants = entities.ToList();

            //copy of all the player's battle commands
            List<PlayerAction<(bool, bool)>> battleCommands = new List<PlayerAction<(bool, bool)>>();
            battleCommands.AddRange(CommandHandler.battleCommands.Select(x => new PlayerAction<(bool, bool)>(x.Name, x.Command, x.Trigger)));

            Console.WriteLine("---/FIGHT/---");
            bool battlePersist = true;
            //fight loop
            while(battlePersist)
            {
                //sort combatants by the fastest speed
                combatants.Sort((x, y) => {
                    return (x.Speed() < y.Speed() ? 1 : x.Speed() > y.Speed() ? -1 : 0);
                });

                for(int i = 0; i < combatants.Count; i++) //the entity that's attacking
                {
                    Entity offender = combatants[i];
                    //all the defenders
                    List<Entity> allDefenders = combatants.FindAll(e => e.Team != offender.Team);
                    //the entity that the offender is going to attack
                    Entity selectedDefender = null;
                    //all weapons in the entity's inventory, including their fists
                    List<Weapon> allWeapons = offender.Inventory.OfType<Weapon>().ToList();
                    allWeapons.Add(new Weapon("Fists", 1, 0));
                    //the weapon that the offender chooses
                    Weapon selectedWeapon = null;

                    //the player attacks
                    if(offender is Player) {

                        Console.WriteLine();
                        //display all combatants
                        foreach(Entity entity in entities)
                        {
                            Console.WriteLine($"{entity}{entity.Team}");
                        }
                        Console.WriteLine($"\nWhat would you like to do{(string.IsNullOrEmpty(offender.Name) ? "?" : $", {offender.Name}?")}");

                        //player can't run if at least 1 enemy won't let them escape
                        Enemy enemy = combatants.OfType<Enemy>().ToList().Find(x => x.CantEscape);
                        if(enemy != null) {
                            Console.WriteLine($"You can't run because {enemy.Name} won't let you escape!");
                            battleCommands.Remove(battleCommands.Find(x => x.Name == "run"));
                        //add run command back if there are no more enemies don't allow the player to escape
                        } else {
                            var runCommand = CommandHandler.battleCommands.Find(x => x.Name == "run");
                            if(!battleCommands.Exists(x => x.Name == "run")) battleCommands.Add(runCommand);
                        }

                        //display all actions to the player
                        foreach(var command in battleCommands) {
                            Console.WriteLine(command.Display(offender, allDefenders));
                        }

                        //action loop
                        List<string> input;
                        while(true)
                        {
                            input = Console.ReadLine().Split(' ').ToList();;

                            //parse input
                            string defenderName = string.Join(" ", input.Skip(1).SkipLast(input.Count - input.IndexOf("with")));
                            string weaponName = string.Join(" ", input.Skip(input.IndexOf("with")+1));

                            //get weapon from offender, and defender
                            selectedDefender = allDefenders.ToList().Find(defender => defender.Name.ToLower() == defenderName.ToLower());
                            selectedWeapon = allWeapons.ToList().Find(weapon => weapon.Name.ToLower() == weaponName.ToLower());

                            var action = battleCommands.Find((x) => x.Name == input[0].ToLower());
                            //run the command
                            if(action != null) 
                            {
                               //(turnPersist, gamePersist)
                                (bool, bool) result = action.Do(world, offender, selectedWeapon, selectedDefender, input.ToArray());
                                if(!result.Item2) return; //end battle
                                else if(!result.Item1) break; //end turn
                                 
                            }
                        }
                    //other entity attacks
                    } else {
                        //entity selects a defender and a weapon
                        selectedDefender = allDefenders[rng.Next(allDefenders.Count)];
                        selectedWeapon = allWeapons[rng.Next(allWeapons.Count)];

                        //defender takes damage
                        selectedDefender.TakeDamage(world, offender, selectedWeapon);
                    }

                    //check if defender is dead
                    if(selectedDefender == null) {
                        continue;
                    }
                    if(selectedDefender.Health <= 0) {
                        //remove the defender from combatants and allDefenders
                        combatants.Remove(selectedDefender);
                        allDefenders.Remove(selectedDefender);
                    }
                    //end the fight if the offender is the last one surviving
                    if(combatants.Count == 1) {
                        battlePersist = false;
                        return;
                    }
                }
            }
        }
    }
}