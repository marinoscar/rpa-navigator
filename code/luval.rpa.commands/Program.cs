﻿using luval.rpa.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.commands
{
    class Program
    {
        static void Main(string[] args)
        {
            var xml = File.ReadAllText(@"C:\Users\oscar.marin\Desktop\TMP\Tool-Release-v5.bprelease");
            var release = new ReleaseExtractor(xml);
            release.Load();
        }
    }
}
