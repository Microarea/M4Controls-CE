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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M4ControlsExplorer
{
    public partial class fJson : Form
    {
        public fJson(string idd, string json, string errors, string aJsonFilename)
        {
            InitializeComponent();
            this.Text = idd;
            tbErrors.Text = errors;
            tbJson.Text = json;
            tbJsonFilename.Text = aJsonFilename;
            tbJson.SelectionStart = tbJson.SelectionLength;
            Clipboard.SetData(DataFormats.Text, json);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(tbJson.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(tbJsonFilename.Text, tbJson.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
