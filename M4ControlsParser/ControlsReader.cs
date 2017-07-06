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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M4ControlsDBMaker;

namespace M4ControlsParser
{
   public  class ControlsReader
    {
        private string mERP = string.Empty;
        private CommonFunctions cf = new CommonFunctions();
        private Hashtable mControls = new Hashtable();
        private DBManager mDB = null;

        private static string BEGIN_REGISTER_CONTROLS = "BEGIN_REGISTER_CONTROLS";
        private static string END_REGISTER_CONTROLS = "END_REGISTER_CONTROLS";
        private static string REGISTER = "REGISTER";

        public ControlsReader(DBManager aDB, string aERP)
        {
            mDB = aDB;
            mERP = aERP;
        }

        public void Search()
        {
            if (string.IsNullOrEmpty(mERP))
                return;

            mControls.Clear();
            mDB.ControlsClassesDelete();

            foreach (string m in Directory.GetDirectories(mERP))
            {
                foreach (string d in Directory.GetDirectories(m))
                {
                    foreach (string f in Directory.GetFiles(d, "*.cpp"))
                        LoadControls(File.ReadAllText(f, Encoding.Default));
                }
            }
        }

        private void LoadControls(string aText)
        {
            if (!aText.Contains(BEGIN_REGISTER_CONTROLS))
                return;

            string t = cf.Clean(aText);

            int pBeginRegisterControls = t.IndexOf(BEGIN_REGISTER_CONTROLS);
            if (pBeginRegisterControls == -1)
                return;
            int pEndRegisterControls = t.IndexOf(END_REGISTER_CONTROLS, pBeginRegisterControls);
            if (pEndRegisterControls == -1)
                return;

            int pRegister = t.IndexOf(REGISTER, pBeginRegisterControls + 23);
            int pBracketOpened = -1;

            while (pRegister != -1 && pRegister < pEndRegisterControls)
            {
                pBracketOpened = t.IndexOf("(", pRegister);
                if (pBracketOpened != -1)
                    LoadControls(t, pBracketOpened);
                pRegister = t.IndexOf(REGISTER, pRegister + 1);
            }
        }

        private void LoadControls(string aText, int pBegin)
        {
            int pComma1 = aText.IndexOf(",", pBegin);
            if (pComma1 == -1)
                return;

            string aJsonName = aText.Substring(pBegin + 1, pComma1 - pBegin - 1).Trim();

            pComma1 = aText.IndexOf(",", pComma1 + 1);
            if (pComma1 == -1)
                return;
            int pComma2 = aText.IndexOf(",", pComma1 + 1);
            if (pComma2 == -1)
                return;

            string aClass = aText.Substring(pComma1 + 1, pComma2 - pComma1 - 1).Trim();

            AddControl(aClass, aJsonName);
        }

        private void AddControl(string aClass, string aJsonName)
        {
            if (mDB == null)
                return;

            System.Diagnostics.Debug.WriteLine("\tCONTROL:\t" + aClass  + "\t" + aJsonName);

            if (!mControls.Contains(aClass))
            {
                mControls.Add(aClass, aJsonName);
                mDB.ControlsClassesInsert(aClass, aJsonName);
            }
        }
    }
}
