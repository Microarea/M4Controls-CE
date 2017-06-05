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
            {
                foreach (string m in Directory.GetDirectories(aERP))
                {
                    if (File.Exists(Path.Combine(m, "Module.Config")))
                        mModules.Add(Path.GetFileName(m));
                }
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
