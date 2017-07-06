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

using M4ControlsDBMaker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M4ControlsExplorer
{
    public partial class fEdit : Form
    {
        public fEdit(_CONTROL c)
        {
            InitializeComponent();

            tbModule.Text = c._module;
            tbFilename.Text = c._filename;
            tbClassIDD.Text = c._classidd;
            tbClass.Text = c._class;
            tbNamespace.Text = c._namespace;
            nudTileStyle.Value = c._tilestyle;
            tbTileText.Text = c._tiletext;
            tbTileSize.Text = c._tilesize;
            tbIDC.Text = c._idc;
            tbBodyEditIDC.Text = c._bodyeditidc;
            tbBodyEditText.Text = c._bodyedittext;
            tbText.Text = c._text;
            tbDBT.Text = c._dbtpointer;
            tbDBTNamespace.Text = c._dbtnamespace;
            tbRecordPointer.Text = c._recordpointer;
            tbRecordClass.Text = c._recordclass;
            tbField.Text = c._field;
            tbFieldNamespace.Text = c._fieldnamespace;
            tbRuntimeClass.Text = c._runtimeclass;
            tbCombo.Text = c._combotype;
            tbHotKeyLink.Text = c._hkl;
            tbButton.Text = c._button;
            tbHidden.Text = c._hidden ?"true":"false";
            tbGrayed.Text = c._grayed ? "true" : "false";
            tbNoChangeGrayed.Text = c._noChange_Grayed ? "true" : "false";
            tbMinValue.Text = c._minValue ;
            tbMaxValue.Text = c._maxValue ;
            tbChar.Text = c._chars ;
            tbRows.Text = c._rows ;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public _CONTROL GetNewValues()
        {
            _CONTROL c = new _CONTROL();

            c._module = tbModule.Text;
            c._filename = tbFilename.Text;
            c._classidd = tbClassIDD.Text;
            c._class = tbClass.Text;
            c._namespace = tbNamespace.Text;
            c._tilestyle = Convert.ToInt32(nudTileStyle.Value);
            c._tiletext = tbTileText.Text;
            c._tilesize = tbTileSize.Text;
            c._idc = tbIDC.Text;
            c._bodyeditidc = tbBodyEditIDC.Text;
            c._bodyedittext = tbBodyEditText.Text;
            c._text = tbText.Text;
            c._dbtpointer = tbDBT.Text;
            c._dbtnamespace = tbDBTNamespace.Text;
            c._recordpointer = tbRecordPointer.Text;
            c._recordclass = tbRecordClass.Text;
            c._field = tbField.Text;
            c._fieldnamespace = tbFieldNamespace.Text;
            c._runtimeclass = tbRuntimeClass.Text;
            c._combotype = tbCombo.Text;
            c._hkl = tbHotKeyLink.Text;
            c._button = tbButton.Text;
            c._hidden = tbHidden.Text == "true" ? true : false;
            c._grayed = tbGrayed.Text == "true" ? true : false;
            c._noChange_Grayed = tbNoChangeGrayed.Text == "true" ? true : false;
            c._minValue = tbMinValue.Text;
            c._maxValue = tbMaxValue.Text;
            c._chars = tbChar.Text;
            c._rows = tbRows.Text; 

            return c;
        }
        
    }
}
