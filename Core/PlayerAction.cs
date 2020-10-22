// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class PlayerAction
    {
        //the name of the action
        public string Name { get; private set; }
        //how the command to call the action is formatted
        public Func<object[], string> Command { get; private set; }
        //the code that runs
        Action<object[]> Trigger { get; set; }

        public PlayerAction(string name, Func<object[], string> command, Action<object[]> trigger)
        {
            Name = name;
            Command = command;
            Trigger = trigger;
        }

        //displays how to type the command to perform the action
        public string Display(params object[] args)
        {
            return Command.Invoke(args);
        }

        //perform the action
        public void Do(params object[] args)
        {
            Trigger.Invoke(args);
            return;
        }
    }

    //same as the class above, but Do() returns a value of type T
    public class PlayerAction<T>
    {
        public string Name { get; private set; }
        public Func<object[], string> Command { get; private set; }
        public Func<object[], T> Trigger { get; private set; }

        public PlayerAction(string name, Func<object[], string> command, Func<object[], T> trigger)
        {
            Name = name;
            Command = command;
            Trigger = trigger;
        }

        public string Display(params object[] args)
        {
            return Command.Invoke(args);
        }

        public T Do(params object[] args)
        {
            return Trigger.Invoke(args);
        }
    }
}