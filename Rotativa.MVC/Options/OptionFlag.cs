﻿using System;

namespace Rotativa.MVC.Options
{
    class OptionFlag : Attribute
    {
        public string Name { get; private set; }

        public OptionFlag(string name)
        {
            Name = name;
        }
    }
}
