/*
M4-Controls CE - Tool di completamento delle descrizioni Json a partire dai source C++ 
Copyright (C) 2017 Microarea s.p.a.

This program is free software: you can redistribute it and/or modify it under the 
terms of the GNU General Public License as published by the Free Software Foundation, 
either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, 
but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. 

See the GNU General Public License for more details.
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace M4ControlsParser
{
    /// <summary>
    /// Summary description for ModulesReader.
    /// </summary>
    public class ModulesReader
    {
        private List<string> mModules = new List<string>();

        public ModulesReader()
        {
        }

        public void Load(string aERP)
        {
            mModules.Clear();
            if (aERP != string.Empty && Directory.Exists(aERP))
                SearchModule(aERP);
        }

        public void SearchModule(string aFolder)
        {
            foreach (string m in Directory.GetDirectories(aFolder))
            {
                if (File.Exists(Path.Combine(m, "Module.Config")))
                    mModules.Add(Path.GetFileName(m));
                else
                    SearchModule(m);
            }
        }

        public int GetModulesCount()
        {
            return mModules.Count;
        }

        public List<string> GetModules()
        {
            return mModules;
        }

        public string GetModule(int aIndex)
        {
            return mModules[aIndex];
        }
    }
}
