// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public class Key : Item 
    {
        public string Password { get; private set; }

        public Key(string name, string password, string tag = "Key", bool forceSingle = true) : base(name, tag, forceSingle)
        {
            Password = password;
        }
    }
}