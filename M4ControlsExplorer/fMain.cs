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
using M4ControlsParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace M4ControlsExplorer
{
    public partial class fMain : Form
    {
        private enum return_code { OK, ERROR, WARNING };

        const int ICON_APPLICATION = 0;
        const int ICON_MODULE = 1;
        const int ICON_FILENAME = 2;
        const int ICON_CLASS = 3;
        const int ICON_IDC = 4;
        const int ICON_PROPERTY = 5;
        const int ICON_NO_JSON = 6;

        private List<_CONTROL> mControls = new List<_CONTROL>();
        

        private DBManager mDBManager = new DBManager("M4ControlsCE");
        private ModulesReader mModuleReader = new ModulesReader();
        private ObjectsReader mObjectReader = new ObjectsReader();
        private TreeNode mCurrentNode = null;
        private Hashtable mFiles = new Hashtable();
        private string mSaveApp = string.Empty;

        public fMain()
        {
            InitializeComponent();

            ObjectsReader.CurrentClass += ObjectsReader_CurrentClass;
            ObjectsReader.CurrentIDC += ObjectsReader_CurrentIDC;
            if (!mDBManager.Create())
                    MessageBox.Show("Error creating DB");
            else
                mDBManager.Open();
            tbApp.Text = Properties.Settings.Default.Application;
            if (!Directory.Exists(tbApp.Text))
                tbApp.Text = string.Empty;
            mSaveApp = tbApp.Text;
            LoadModules();
            cbModules.Text = Properties.Settings.Default.Module;
            cbAllModules.Checked = Properties.Settings.Default.AllModules;
            LoadFiles();
            cbFiles.Text = Properties.Settings.Default.File;
            cbAllFiles.Checked = Properties.Settings.Default.AllFiles;
        }

        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.AllModules = cbAllModules.Checked;
            Properties.Settings.Default.Module = cbModules.Text;
            Properties.Settings.Default.Application = tbApp.Text;
            Properties.Settings.Default.AllFiles = cbAllFiles.Checked;
            Properties.Settings.Default.File = cbFiles.Text;
            Properties.Settings.Default.Save();
        }

        #region loading
        private void LoadModules()
        {
            cbModules.Items.Clear();
            bool bOK = !string.IsNullOrEmpty(tbApp.Text) && Directory.Exists(tbApp.Text);
            if (bOK)
            {
                mModuleReader.Load(tbApp.Text);
                cbModules.Items.AddRange(mModuleReader.GetModules().ToArray());
            }
        }

        private void LoadFiles()
        {
            cbFiles.Items.Clear();
            mFiles.Clear();
            if (string.IsNullOrEmpty(tbApp.Text) || string.IsNullOrEmpty(cbModules.Text) || cbAllModules.Checked)
                return;

            string r = Path.Combine(tbApp.Text, cbModules.Text);
            string f = string.Empty;

            foreach (string d in Directory.GetDirectories(r))
            {
                string[] filenames = Directory.GetFiles(d, "*.cpp");

                var filecontents = from file in filenames.AsParallel()
                                   from line in File.ReadLines(file, Encoding.Default)
                                   where line.Contains("AddLink") || line.Contains("AddColumn")
                                   select new { File = file, line = line };

                foreach (var item in filecontents)
                {
                    f = Path.GetFileNameWithoutExtension(item.File);
                    if (!mFiles.Contains(f))
                    {
                        mFiles.Add(f, item.File);
                        cbFiles.Items.Add(f);
                    }
                }
            }
        }

        private void LoadControls()
        {
            mControls.Clear();
            //if (!CheckDatabase())
                //return;

            Cursor = Cursors.WaitCursor;
            tsslStatusModule.Visible = true;
            tsslStatus.Text = "Loading...";

            mControls = mDBManager.GetControls();

            tvTree.Nodes.Clear();
            lvControls.Items.Clear();
            Application.DoEvents();

            tvTree.SuspendLayout();
            lvControls.SuspendLayout();
            tvTree.BeginUpdate();
            lvControls.BeginUpdate();

            TreeNode eNode = tvTree.Nodes.Add("App");
            eNode.ImageIndex = eNode.SelectedImageIndex = ICON_APPLICATION;

            LoadControls(eNode);

            foreach (TreeNode aNode in eNode.Nodes)
            {
                //                tvTree.SelectedNode = mNode;
                aNode.EnsureVisible();
                aNode.Expand();
                //foreach (TreeNode cNode in mNode.Nodes)
                //    cNode.Expand();
            }

            tvTree.EndUpdate();
            lvControls.EndUpdate();
            tvTree.ResumeLayout();
            tvTree.SelectedNode = tvTree.Nodes[0];
            tvTree.SelectedNode.EnsureVisible();
            tvTree.Select();
            lvControls.ResumeLayout();
            tsslStatus.Text = "Ready";
            tsslStatusModule.Visible = false;
            Application.DoEvents();

            Cursor = Cursors.Default;
        }

        private void LoadControls(TreeNode eNode)
        {
            TreeNode mNode = null;
            TreeNode fNode = null;
            TreeNode cNode = null;
            TreeNode iNode = null;
            TreeNode _Node = null;

            string _m = string.Empty;
            string _lm = string.Empty;
            string _f = string.Empty;
            string _lf = string.Empty;
            string _c = string.Empty;
            string _lc = string.Empty;
            string _i = string.Empty;
            string _li = string.Empty;

            string s = string.Empty;
            foreach (_CONTROL cl in mControls)
            {
                tsslStatusModule.Text = cl._module;
                Application.DoEvents();

                _m = cl._module;
                if (_m != _lm)
                {
                    mNode = eNode.Nodes.Add(_m);
                    mNode.ImageIndex = mNode.SelectedImageIndex = ICON_MODULE;
                    _lm = _m;
                }
                s = cl._filename;
                _f = Path.GetFileNameWithoutExtension(s);
                if (_f != _lf)
                {
                    fNode = mNode.Nodes.Add(_f);
                    fNode.ImageIndex = fNode.SelectedImageIndex = ICON_FILENAME;
                    fNode.Tag = s;
                    _lf = _f;
                }
                _c = cl._class;
                if (_c != _lc)
                {
                    //if(string.IsNullOrWhiteSpace(_c))
                        cNode = fNode.Nodes.Add(_c);
                    //else
                    //    cNode = fNode.Nodes.Add("*** Class NOT FOUND ***");
                    cNode.ImageIndex = cNode.SelectedImageIndex = ICON_CLASS;
                    _lc = _c;
                }
                _i = cl._idc;
                if (_i != _li)
                {
                    iNode = cNode.Nodes.Add(_i);
                    if (cl._generatejson)
                        iNode.ImageIndex = iNode.SelectedImageIndex = ICON_IDC;
                    else
                        iNode.ImageIndex = iNode.SelectedImageIndex = ICON_NO_JSON;
                    _li = _i;
                }
                s = cl._fieldnamespace;
                if (!string.IsNullOrEmpty(s))
                {
                    _Node = iNode.Nodes.Add(s);
                    _Node.ImageIndex = _Node.SelectedImageIndex = ICON_PROPERTY;
                }
                s = cl._dbtnamespace;
                if (!string.IsNullOrEmpty(s))
                {
                    _Node = iNode.Nodes.Add(s);
                    _Node.ImageIndex = _Node.SelectedImageIndex = ICON_PROPERTY;
                }
                s = cl._runtimeclass;
                if (!string.IsNullOrEmpty(s))
                {
                    _Node = iNode.Nodes.Add(s);
                    _Node.ImageIndex = _Node.SelectedImageIndex = ICON_PROPERTY;
                }
                s = cl._hkl;
                if (!string.IsNullOrEmpty(s))
                {
                    _Node = iNode.Nodes.Add(s);
                    _Node.ImageIndex = _Node.SelectedImageIndex = ICON_PROPERTY;
                }
            }
        }

        private void LoadGrid()
        {
            lvControls.Items.Clear();
            tsslStatusModule.Visible = false;
            tsslStatusClass.Visible = false;
            tsslStatusIDC.Visible = false;

            if (tvTree.SelectedNode == null)
                return;

            Cursor = Cursors.WaitCursor;

            switch (tvTree.SelectedNode.ImageIndex)
            {
                case ICON_APPLICATION:
                    LoadGridControls();
                    break;
                case ICON_MODULE:
                    tsslStatusModule.Text = tvTree.SelectedNode.Text;
                    tsslStatusModule.Visible = true;
                    LoadGridControls(tvTree.SelectedNode.Text);
                    break;
                case ICON_FILENAME:
                    tsslStatusModule.Text = tvTree.SelectedNode.Text;
                    tsslStatusModule.Visible = true;
                    LoadGridControls(tvTree.SelectedNode.Parent.Text, tvTree.SelectedNode.Text);
                    break;
                case ICON_CLASS:
                    tsslStatusModule.Text = tvTree.SelectedNode.Text;
                    tsslStatusClass.Text = tvTree.SelectedNode.Parent.Parent.Text;
                    tsslStatusModule.Visible = true;
                    tsslStatusClass.Visible = true;
                    LoadGridControls(tvTree.SelectedNode.Parent.Parent.Text, tvTree.SelectedNode.Parent.Text, tvTree.SelectedNode.Text);
                    break;
                case ICON_IDC:
                    tsslStatusModule.Text = tvTree.SelectedNode.Text;
                    tsslStatusClass.Text = tvTree.SelectedNode.Parent.Parent.Text;
                    tsslStatusIDC.Text = tvTree.SelectedNode.Parent.Parent.Parent.Text;
                    tsslStatusModule.Visible = true;
                    tsslStatusClass.Visible = true;
                    tsslStatusIDC.Visible = true;
                    LoadGridControls(tvTree.SelectedNode.Parent.Parent.Parent.Text, tvTree.SelectedNode.Parent.Parent.Text, tvTree.SelectedNode.Parent.Text, tvTree.SelectedNode.Text);
                    break;
                case ICON_PROPERTY:
                    tsslStatusModule.Text = tvTree.SelectedNode.Text;
                    tsslStatusClass.Text = tvTree.SelectedNode.Parent.Parent.Text;
                    tsslStatusIDC.Text = tvTree.SelectedNode.Parent.Parent.Parent.Text;
                    tsslStatusModule.Visible = true;
                    tsslStatusClass.Visible = true;
                    tsslStatusIDC.Visible = true;
                    LoadGridControls(tvTree.SelectedNode.Parent.Parent.Parent.Parent.Text, tvTree.SelectedNode.Parent.Parent.Parent.Text, tvTree.SelectedNode.Parent.Parent.Text, tvTree.SelectedNode.Parent.Text);
                    break;
            }
            Cursor = Cursors.Default;
        }

        private void LoadGridControls(string aMdodule = "", string aFilename = "", string aClass = "", string aIDC = "")
        {
            ListViewItem lvi = null;
            return_code r = return_code.OK;

            foreach (_CONTROL cl in mControls)
            {
                if ((string.IsNullOrEmpty(aMdodule) || aMdodule == cl._module) &&
                    (string.IsNullOrEmpty(aFilename) || aFilename == Path.GetFileNameWithoutExtension(cl._filename)) &&
                    (string.IsNullOrEmpty(aClass) || aClass == cl._class) &&
                    (string.IsNullOrEmpty(aIDC) || aIDC == cl._idc))
                {
                    lvi = lvControls.Items.Add(cl._module);
                    lvi.SubItems.Add(cl._filename);
                    lvi.SubItems.Add(cl._classidd);
                    lvi.SubItems.Add(cl._class);
                    lvi.SubItems.Add(cl._namespace);
                    lvi.SubItems.Add(cl._tilestyle.ToString());
                    lvi.SubItems.Add(cl._tiletext);
                    lvi.SubItems.Add(cl._tilesize);
                    lvi.SubItems.Add(cl._idc);
                    lvi.SubItems.Add(cl._bodyeditidc);
                    lvi.SubItems.Add(cl._bodyedittext);
                    lvi.SubItems.Add(cl._text);
                    lvi.SubItems.Add(cl._dbtpointer);
                    lvi.SubItems.Add(cl._dbtnamespace);
                    lvi.SubItems.Add(cl._recordpointer);
                    lvi.SubItems.Add(cl._recordclass);
                    lvi.SubItems.Add(cl._field);
                    lvi.SubItems.Add(cl._fieldnamespace);
                    lvi.SubItems.Add(cl._runtimeclass);
                    lvi.SubItems.Add(cl._combotype);
                    lvi.SubItems.Add(cl._hkl);
                    lvi.SubItems.Add(cl._button);
                    lvi.SubItems.Add(cl._hidden ?"true":"false");
                    lvi.SubItems.Add(cl._grayed ? "true" : "false");
                    lvi.SubItems.Add(cl._noChange_Grayed ? "true" : "false");
                    lvi.SubItems.Add(cl._minValue);
                    lvi.SubItems.Add(cl._maxValue);
                    lvi.SubItems.Add(cl._chars);
                    lvi.SubItems.Add(cl._rows);

                    r = return_code.OK;

                    if (string.IsNullOrEmpty(cl._classidd) || string.IsNullOrEmpty(cl._idc))
                        r = return_code.ERROR;

                    if (string.IsNullOrEmpty(cl._namespace) && string.IsNullOrEmpty(cl._fieldnamespace))
                    {
                        if (cl._field.ToUpper().Contains("BODY"))
                            r = return_code.WARNING; // Probabilmente e' giusto
                        else
                            r = return_code.ERROR;
                    }

                    if (!string.IsNullOrEmpty(cl._recordpointer) && string.IsNullOrEmpty(cl._recordclass))
                        r = return_code.ERROR;

                    if (!string.IsNullOrEmpty(cl._dbtpointer) && string.IsNullOrEmpty(cl._dbtnamespace))
                        r = return_code.ERROR;

                    if (!(cl._generatejson))
                        lvi.ImageIndex = 3;
                    else if (r == return_code.WARNING)
                        lvi.ImageIndex = 1;
                    else if (r == return_code.ERROR)
                        lvi.ImageIndex = 2;
                }
            }
        }
        #endregion

        #region search
        private void SearchControls()
        {
            tsslStatus.Text = "Initializing";
            Application.DoEvents();
            mObjectReader.Initialize(mDBManager, tbApp.Text);

            tsslStatus.Text = "Searching...";
            Application.DoEvents();

            tsslStatusModule.Visible = true;
            tsslStatusClass.Visible = true;
            tsslStatusIDC.Visible = true;
            Application.DoEvents();

            if (cbAllModules.Checked)
            {
                for (int i = 0; i < cbModules.Items.Count; i++)
                {
                    tsslStatusModule.Text = cbModules.Items[i].ToString();
                    Application.DoEvents();
                    mDBManager.ControlsDelete(cbModules.Items[i].ToString());
                    mObjectReader.LoadModule(cbModules.Items[i].ToString());
                    mObjectReader.UpdateDB();
                }
            }
            else
            {
                tsslStatusModule.Text = cbModules.Text;
                Application.DoEvents();
                if (!cbAllFiles.Checked && !string.IsNullOrEmpty(cbFiles.Text) && mFiles.Contains(cbFiles.Text))
                {
                    mDBManager.ControlsDelete(cbModules.Text, mFiles[cbFiles.Text].ToString());
                    mObjectReader.LoadFile(cbModules.Text, mFiles[cbFiles.Text].ToString());
                }
                else
                {
                    mDBManager.ControlsDelete(cbModules.Text);
                    mObjectReader.LoadModule(cbModules.Text);
                }
                mObjectReader.UpdateDB();
            }
            tsslStatus.Text = "Ready";
            tsslStatusModule.Visible = false;
            tsslStatusClass.Visible = false;
            tsslStatusIDC.Visible = false;
            tsslStatusModule.Text = string.Empty;
            tsslStatusClass.Text = string.Empty;
            tsslStatusIDC.Text = string.Empty;
            Application.DoEvents();
        }

        private void ObjectsReader_CurrentClass(object sender, string e)
        {
            tsslStatusClass.Text = e;
            Application.DoEvents();
        }

        private void ObjectsReader_CurrentIDC(object sender, string e)
        {
            tsslStatusIDC.Text = e;
            Application.DoEvents();
        }
        #endregion

        private void bBrowseERP_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.SelectedPath = tbApp.Text;
            fbd.Description = "Select the ERP folder";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbApp.Text = fbd.SelectedPath;
                if (tbApp.Text != mSaveApp)
                    LoadModules();
            }
            mSaveApp = tbApp.Text;
        }

        private void tsmiControlsCopy_Click(object sender, EventArgs e)
        {
            if (lvControls.SelectedIndices.Count <= 0)
                return;

            string s = string.Empty;
            foreach (ListViewItem.ListViewSubItem lvsi in lvControls.SelectedItems[0].SubItems)
                s += lvsi.Text + "\r\n";
            Clipboard.SetData(DataFormats.Text, s);
        }

        private void tsmiControlsFind_Click(object sender, EventArgs e)
        {
            if (lvControls.SelectedIndices.Count <= 0)
                return;

            fFind f = new fFind();

            if (f.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(f.GetText()))
            {
                foreach (ListViewItem lvi in lvControls.Items)
                {
                    foreach (ListViewItem.ListViewSubItem lvsi in lvi.SubItems)
                    {
                        if (lvsi.Text == f.GetText())
                        {
                            lvi.Selected = true;
                            lvi.EnsureVisible();
                            lvControls.Select();
                            lvControls.Focus();
                            return;
                        }
                    }
                }
            }
        }

        private void cmsControlsMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lvControls.SelectedIndices.Count <= 0)
                e.Cancel = true;
        }

        private void tbERP_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tbApp.Text != mSaveApp)
                LoadModules();
            mSaveApp = tbApp.Text;
        }

        private void tvTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadGrid();
            lvControls.Select();
            lvControls.Focus();
        }

        private void tsmiTreeCopy_Click(object sender, EventArgs e)
        {
            if (mCurrentNode == null)
                return;
            Clipboard.SetData(DataFormats.Text, mCurrentNode.Text);
        }

        private void cmsTreeMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tsmiGenerateJson.Visible = false;

            if (mCurrentNode == null)
                return;

            if (mCurrentNode.ImageIndex == ICON_PROPERTY)
                e.Cancel = true;

            if (mCurrentNode.ImageIndex == ICON_CLASS)
            {
                foreach (TreeNode n in mCurrentNode.Nodes)
                {
                    if (n.ImageIndex == ICON_IDC)
                    {
                        tsmiGenerateJson.Visible = true;
                        return;
                    }
                }
            }
        }

        private void tvTree_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                mCurrentNode = tvTree.GetNodeAt(e.X, e.Y);
        }

        private void cbModulesSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (cbModules.Items.Count > 0)
            {
                cbModules.Text = cbModules.Items[0].ToString();
                cbModules_SelectedIndexChanged(sender, e);
            }
        }

        private void cbModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFiles();
            if (cbFiles.Items.Count > 0)
                cbFiles.Text = cbFiles.Items[0].ToString();
        }

        private void bSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbApp.Text))
                return;

            if (!cbAllModules.Checked && string.IsNullOrEmpty(cbModules.Text))
                return;

            if (!cbAllFiles.Checked && string.IsNullOrEmpty(cbFiles.Text))
                return;
            mDBManager.Delete(false);
            Cursor = Cursors.WaitCursor;
            SearchControls();
            SetDBTFildNamespace();
            LoadControls();
            Cursor = Cursors.Default;
        }
        

        #region DBT_Fild
        public void SetDBTFildNamespace()
        {
            foreach (_CONTROL c in mControls)
                c._fieldnamespace = mDBManager.GetFieldNamespace(c._recordclass, c._field);

        }

        #endregion

        #region json

        private void tsmiGenerateJson_Click(object sender, EventArgs e)
        {
            var r = CreateJson(mCurrentNode.Text);
            if (r.Item1)
                MessageBox.Show(r.Item2 + "\r\n" + r.Item3);
            else
            {
                fJson f = new fJson(mCurrentNode.Text, r.Item2, r.Item3, r.Item4);
                f.ShowDialog(this);
            }
        }

        public Tuple<bool, string, string, string> CreateJson(string aClass)
        {
            bool bFound = false;
            bool bFirst = true;
            string errors = string.Empty;
            string jsonFilename = string.Empty;

            JsonManager jm = new JsonManager(mDBManager);

            foreach (_CONTROL cl in mControls)
            {
                if (cl._class == aClass)
                {
                    bFound = true;

                    if (!cl._generatejson)
                        continue;

                    if (bFirst)
                    {
                        if (string.IsNullOrEmpty(cl._classidd))
                            errors += "Class IDD not found\r\n";
                        if (string.IsNullOrEmpty(cl._tilesize))
                            errors += "Tyle size not found\r\n";
                        if (string.IsNullOrEmpty(cl._tiletext))
                            errors += "Tyle text not found\r\n";
                        if (string.IsNullOrEmpty(cl._filename))
                            errors += "Filename not found\r\n";

                        if (!jm.LoadJson(cl._classidd, cl._namespace, cl._tilesize, cl._tilestyle, cl._tiletext, cl._filename, out jsonFilename))
                        {
                            errors += "Error loading json";
                            return new Tuple<bool, string, string, string>(true, cl._classidd, errors, jsonFilename);
                        }

                        if (!cl._isaddlink)
                        {
                            if (string.IsNullOrEmpty(cl._bodyeditidc))
                                errors += "Bodyedit IDC not found\r\n";
                            if (string.IsNullOrEmpty(cl._dbtnamespace))
                                errors += "DBT namespace not found\r\n";

                            if (!jm.CreateBodyEditBinding(cl._bodyeditidc, cl._dbtnamespace, cl._bodyedittext))
                            {
                                errors += "Error creating BodyEdit binding\r\n";
                                return new Tuple<bool, string, string, string>(true, aClass + " " + cl._bodyeditidc, errors, jsonFilename);
                            }
                        }

                        bFirst = false;
                    }
                    if (cl._isaddlink && !jm.CreateAddLinkBinding(cl._idc, cl._namespace, cl._fieldnamespace, cl._runtimeclass, cl._dbtnamespace, cl._minValue, cl._maxValue, cl._chars, cl._rows, cl._hkl, cl._button))
                    {
                        errors += "Error creating AddLink binding\r\n";
                        return new Tuple<bool, string, string, string>(true, aClass + " " + cl._idc, errors, jsonFilename);
                    }
                    else if (!cl._isaddlink && !jm.CreateAddColumn(cl._idc, cl._namespace, cl._fieldnamespace, cl._runtimeclass, cl._combotype == "0" ? string.Empty : cl._combotype, cl._text, cl._hidden, cl._grayed, cl._noChange_Grayed, cl._minValue, cl._maxValue, cl._chars, cl._rows, cl._hkl, cl._button))
                    {
                        errors += "Error creating AddColumn\r\n";
                        return new Tuple<bool, string, string, string>(true, aClass + " " + cl._idc, errors, jsonFilename);
                    }
                }
            }

            if (!bFound)
            {
                errors += "Class not found\r\n";
                return new Tuple<bool, string, string, string>(true, aClass, errors, jsonFilename);
            }

            errors += "OK!";
            if (jm.GetVariables().Count > 0)
                errors += "\r\n\r\nDOCUMENT VARIABLES:\r\n";
            foreach (string s in jm.GetVariables())
                errors += s + "\r\n";

            if (jm.GetErrors().Count > 0)
                errors += "\r\nERRORS:\r\n\r\n";
            foreach (string s in jm.GetErrors())
                errors += s + "\r\n";
            return new Tuple<bool, string, string, string>(false, jm.GetJson(), errors, jsonFilename);
        }
        #endregion

        private void fMain_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LoadControls();
            Cursor = Cursors.Default;
        }

        #region Edit

        private void lvControls_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit();
        }

        private void tsmiControlsEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            _CONTROL c = new _CONTROL();
            ListViewItem lvp = null;

            foreach (ListViewItem lvi in lvControls.SelectedItems)
            {
                if (lvp == null)
                {
                    c._module = lvi.SubItems[0].Text;
                    c._filename = lvi.SubItems[1].Text;
                    c._classidd = lvi.SubItems[2].Text;
                    c._class = lvi.SubItems[3].Text;
                    c._namespace = lvi.SubItems[4].Text;
                    if (string.IsNullOrEmpty(lvi.SubItems[5].Text))
                        c._tilestyle = -1;
                    else
                    {
                        int ts = Convert.ToInt32(lvi.SubItems[5].Text);
                        if (ts < -1 || ts > 2 || lvi.SubItems[5].Text.Length > 1)
                            c._tilestyle = -1;
                        else
                            c._tilestyle = ts;
                    }
                    c._tiletext = lvi.SubItems[6].Text;
                    c._tilesize = lvi.SubItems[7].Text;
                    c._idc = lvi.SubItems[8].Text;
                    c._bodyeditidc = lvi.SubItems[9].Text;
                    c._bodyedittext = lvi.SubItems[10].Text;
                    c._text = lvi.SubItems[11].Text;
                    c._dbtpointer = lvi.SubItems[12].Text;
                    c._dbtnamespace = lvi.SubItems[13].Text;
                    c._recordpointer = lvi.SubItems[14].Text;
                    c._recordclass = lvi.SubItems[15].Text;
                    c._field = lvi.SubItems[16].Text;
                    c._fieldnamespace = lvi.SubItems[17].Text;
                    c._runtimeclass = lvi.SubItems[18].Text;
                    c._combotype = lvi.SubItems[19].Text;
                    c._hkl = lvi.SubItems[20].Text;
                    c._button = lvi.SubItems[21].Text;
                    c._hidden = lvi.SubItems[22].Text == "true" ? true : false;
                    c._grayed = lvi.SubItems[23].Text == "true" ? true : false;
                    c._noChange_Grayed = lvi.SubItems[24].Text == "true" ? true : false;
                    c._minValue = lvi.SubItems[25].Text;
                    c._maxValue= lvi.SubItems[26].Text;
                    c._chars = lvi.SubItems[27].Text;
                    c._rows = lvi.SubItems[28].Text;

                }
                else
                {
                    if (!string.IsNullOrEmpty(lvi.SubItems[0].Text) && lvi.SubItems[0].Text != lvp.SubItems[0].Text)
                        c._module = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[1].Text) && lvi.SubItems[1].Text != lvp.SubItems[1].Text)
                        c._filename = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[2].Text) && lvi.SubItems[2].Text != lvp.SubItems[2].Text)
                        c._classidd = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[3].Text) && lvi.SubItems[3].Text != lvp.SubItems[3].Text)
                        c._class = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[4].Text) && lvi.SubItems[4].Text != lvp.SubItems[4].Text)
                        c._namespace = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[5].Text) && lvi.SubItems[5].Text != lvp.SubItems[5].Text)
                        c._tilestyle = -1;
                    if (!string.IsNullOrEmpty(lvi.SubItems[6].Text) && lvi.SubItems[6].Text != lvp.SubItems[6].Text)
                        c._tiletext = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[7].Text) && lvi.SubItems[7].Text != lvp.SubItems[7].Text)
                        c._tilesize = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[8].Text) && lvi.SubItems[8].Text != lvp.SubItems[8].Text)
                        c._idc = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[9].Text) && lvi.SubItems[9].Text != lvp.SubItems[9].Text)
                        c._bodyeditidc = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[10].Text) && lvi.SubItems[10].Text != lvp.SubItems[10].Text)
                        c._bodyedittext = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[11].Text) && lvi.SubItems[11].Text != lvp.SubItems[11].Text)
                        c._text = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[12].Text) && lvi.SubItems[12].Text != lvp.SubItems[12].Text)
                        c._dbtpointer = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[13].Text) && lvi.SubItems[13].Text != lvp.SubItems[13].Text)
                        c._dbtnamespace = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[14].Text) && lvi.SubItems[14].Text != lvp.SubItems[14].Text)
                        c._recordpointer = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[15].Text) && lvi.SubItems[15].Text != lvp.SubItems[15].Text)
                        c._recordclass = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[16].Text) && lvi.SubItems[16].Text != lvp.SubItems[16].Text)
                        c._field = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[17].Text) && lvi.SubItems[17].Text != lvp.SubItems[17].Text)
                        c._fieldnamespace = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[18].Text) && lvi.SubItems[18].Text != lvp.SubItems[18].Text)
                        c._runtimeclass = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[19].Text) && lvi.SubItems[19].Text != lvp.SubItems[19].Text)
                        c._combotype = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[20].Text) && lvi.SubItems[20].Text != lvp.SubItems[20].Text)
                        c._hkl = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[21].Text) && lvi.SubItems[21].Text != lvp.SubItems[21].Text)
                        c._button = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[22].Text) && lvi.SubItems[22].Text != lvp.SubItems[22].Text)
                        c._hidden = false;
                    if (!string.IsNullOrEmpty(lvi.SubItems[23].Text) && lvi.SubItems[23].Text != lvp.SubItems[23].Text)
                        c._grayed = false;
                    if (!string.IsNullOrEmpty(lvi.SubItems[24].Text) && lvi.SubItems[24].Text != lvp.SubItems[24].Text)
                        c._noChange_Grayed = false;
                    if (!string.IsNullOrEmpty(lvi.SubItems[25].Text) && lvi.SubItems[25].Text != lvp.SubItems[25].Text)
                        c._minValue = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[26].Text) && lvi.SubItems[26].Text != lvp.SubItems[26].Text)
                        c._maxValue = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[27].Text) && lvi.SubItems[27].Text != lvp.SubItems[27].Text)
                        c._chars = string.Empty;
                    if (!string.IsNullOrEmpty(lvi.SubItems[28].Text) && lvi.SubItems[28].Text != lvp.SubItems[28].Text)
                        c._rows = string.Empty;

                }

                lvp = lvi;
            }

            fEdit f = new fEdit(c);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                string aModule = string.Empty;
                string aFilename = string.Empty;
                string aClass = string.Empty;
                string aIDC = string.Empty;
                c = f.GetNewValues();

                foreach (ListViewItem lvi in lvControls.SelectedItems)
                {
                    aModule = lvi.SubItems[0].Text;
                    aFilename = lvi.SubItems[1].Text;
                    aClass = lvi.SubItems[3].Text;
                    aIDC = lvi.SubItems[8].Text;

                    if (!string.IsNullOrEmpty(c._module))
                        lvi.SubItems[0].Text = c._module;
                    if (!string.IsNullOrEmpty(c._filename))
                        lvi.SubItems[1].Text = c._filename;
                    if (!string.IsNullOrEmpty(c._classidd))
                        lvi.SubItems[2].Text = c._classidd;
                    if (!string.IsNullOrEmpty(c._class))
                        lvi.SubItems[3].Text = c._class;
                    if (!string.IsNullOrEmpty(c._namespace))
                        lvi.SubItems[4].Text = c._namespace;
                    if (c._tilestyle >= 0)
                        lvi.SubItems[5].Text = c._tilestyle.ToString();
                    if (!string.IsNullOrEmpty(c._tiletext))
                        lvi.SubItems[6].Text = c._tiletext;
                    if (!string.IsNullOrEmpty(c._tilesize))
                        lvi.SubItems[7].Text = c._tilesize;
                    if (!string.IsNullOrEmpty(c._idc))
                        lvi.SubItems[8].Text = c._idc;
                    if (!string.IsNullOrEmpty(c._bodyeditidc))
                        lvi.SubItems[9].Text = c._bodyeditidc;
                    if (!string.IsNullOrEmpty(c._bodyedittext))
                        lvi.SubItems[10].Text = c._bodyedittext;
                    if (!string.IsNullOrEmpty(c._text))
                        lvi.SubItems[11].Text = c._text;
                    if (!string.IsNullOrEmpty(c._dbtpointer))
                        lvi.SubItems[12].Text = c._dbtpointer;
                    if (!string.IsNullOrEmpty(c._dbtnamespace))
                        lvi.SubItems[13].Text = c._dbtnamespace;
                    if (!string.IsNullOrEmpty(c._recordpointer))
                        lvi.SubItems[14].Text = c._recordpointer;
                    if (!string.IsNullOrEmpty(c._recordclass))
                        lvi.SubItems[15].Text = c._recordclass;
                    if (!string.IsNullOrEmpty(c._field))
                        lvi.SubItems[16].Text = c._field;
                    if (!string.IsNullOrEmpty(c._fieldnamespace))
                        lvi.SubItems[17].Text = c._fieldnamespace;
                    if (!string.IsNullOrEmpty(c._runtimeclass))
                        lvi.SubItems[18].Text = c._runtimeclass;
                    if (!string.IsNullOrEmpty(c._combotype))
                        lvi.SubItems[19].Text = c._combotype;
                    if (!string.IsNullOrEmpty(c._hkl))
                        lvi.SubItems[20].Text = c._hkl;
                    if (!string.IsNullOrEmpty(c._button))
                        lvi.SubItems[21].Text = c._button;
                    if (!string.IsNullOrEmpty(c._button))
                        lvi.SubItems[22].Text = c._hidden == true ? "true" : "false"; ;
                    if (!string.IsNullOrEmpty(c._button))
                        lvi.SubItems[23].Text = c._grayed == true ? "true" : "false";
                    if (!string.IsNullOrEmpty(c._button))
                        lvi.SubItems[24].Text = c._noChange_Grayed == true ? "true" : "false";
                    if (!string.IsNullOrEmpty(c._minValue))
                        lvi.SubItems[25].Text = c._minValue;
                    if (!string.IsNullOrEmpty(c._maxValue))
                        lvi.SubItems[26].Text = c._maxValue;
                    if (!string.IsNullOrEmpty(c._chars))
                        lvi.SubItems[27].Text = c._chars;
                    if (!string.IsNullOrEmpty(c._rows))
                        lvi.SubItems[28].Text = c._rows;

                    foreach (_CONTROL cl in mControls)
                    {
                        if (aModule == cl._module && aFilename == cl._filename && aClass == cl._class && aIDC == cl._idc)
                        {
                            if (!string.IsNullOrEmpty(c._module))
                                cl._module = c._module;
                            if (!string.IsNullOrEmpty(c._filename))
                                cl._filename = c._filename;
                            if (!string.IsNullOrEmpty(c._classidd))
                                cl._classidd = c._classidd;
                            if (!string.IsNullOrEmpty(c._class))
                                cl._class = c._class;
                            if (!string.IsNullOrEmpty(c._namespace))
                                cl._namespace = c._namespace;
                            if (c._tilestyle >= 0)
                                cl._tilestyle = c._tilestyle;
                            if (!string.IsNullOrEmpty(c._tiletext))
                                cl._tiletext = c._tiletext;
                            if (!string.IsNullOrEmpty(c._tilesize))
                                cl._tilesize = c._tilesize;
                            if (!string.IsNullOrEmpty(c._idc))
                                cl._idc = c._idc;
                            if (!string.IsNullOrEmpty(c._bodyeditidc))
                                cl._bodyeditidc = c._bodyeditidc;
                            if (!string.IsNullOrEmpty(c._bodyedittext))
                                cl._bodyedittext = c._bodyedittext;
                            if (!string.IsNullOrEmpty(c._text))
                                cl._text = c._text;
                            if (!string.IsNullOrEmpty(c._dbtpointer))
                                cl._dbtpointer = c._dbtpointer;
                            if (!string.IsNullOrEmpty(c._dbtnamespace))
                                cl._dbtnamespace = c._dbtnamespace;
                            if (!string.IsNullOrEmpty(c._recordpointer))
                                cl._recordpointer = c._recordpointer;
                            if (!string.IsNullOrEmpty(c._recordclass))
                                cl._recordclass = c._recordclass;
                            if (!string.IsNullOrEmpty(c._field))
                                cl._field = c._field;
                            if (!string.IsNullOrEmpty(c._fieldnamespace))
                                cl._fieldnamespace = c._fieldnamespace;
                            if (!string.IsNullOrEmpty(c._runtimeclass))
                                cl._runtimeclass = c._runtimeclass;
                            if (!string.IsNullOrEmpty(c._combotype))
                                cl._combotype = c._combotype;
                            if (!string.IsNullOrEmpty(c._hkl))
                                cl._hkl = c._hkl;
                            if (!string.IsNullOrEmpty(c._button))
                                cl._button = c._button;
                            if (c._hidden)
                                cl._hidden = c._hidden;
                            if (c._grayed)
                                cl._grayed = c._grayed;
                            if (c._noChange_Grayed)
                                cl._noChange_Grayed = c._noChange_Grayed;
                            if (!string.IsNullOrEmpty(c._minValue))
                                cl._minValue = c._minValue;
                            if (!string.IsNullOrEmpty(c._maxValue))
                                cl._maxValue = c._maxValue;
                            if (!string.IsNullOrEmpty(c._chars))
                                cl._chars = c._chars;
                            if (!string.IsNullOrEmpty(c._rows))
                                cl._rows = c._rows;
                        }
                    }
                }
            }
        }
        #endregion

        public List<_CONTROL> ListViewToListControl(ListView lv)
        {
            List<_CONTROL> control = new List<_CONTROL>();
            string aModule = string.Empty;
            string aFilename = string.Empty;
            string aClass = string.Empty;
            string aIDC = string.Empty;

            foreach (ListViewItem lvi in lv.SelectedItems)
            {
                _CONTROL c = new _CONTROL();
                c._module = lvi.SubItems[0].Text;
                c._filename =lvi.SubItems[1].Text;
                c._classidd=lvi.SubItems[2].Text  ;
                c._class = lvi.SubItems[3].Text;
                c._namespace =lvi.SubItems[4].Text  ;
                c._tilestyle = Int32.Parse(lvi.SubItems[5].Text);
                c._tiletext= lvi.SubItems[6].Text  ;
                c._tilesize=lvi.SubItems[7].Text ;
                c._idc=lvi.SubItems[8].Text;
                c._bodyeditidc=lvi.SubItems[9].Text  ;
                c._bodyedittext=lvi.SubItems[10].Text  ;
                c._text=lvi.SubItems[11].Text  ;
                c._dbtpointer=lvi.SubItems[12].Text  ;
                c._dbtnamespace=lvi.SubItems[13].Text  ;
                c._recordpointer=lvi.SubItems[14].Text;
                c._recordclass=lvi.SubItems[15].Text  ;
                c._field=lvi.SubItems[16].Text;
                c._fieldnamespace=lvi.SubItems[17].Text  ;
                c._runtimeclass=lvi.SubItems[18].Text  ;
                c._combotype = lvi.SubItems[19].Text;
                c._hkl = lvi.SubItems[20].Text;
                c._button = lvi.SubItems[21].Text;
                c._hidden = lvi.SubItems[22].Text == "true" ? true : false;
                c._grayed = lvi.SubItems[23].Text == "true" ? true : false;
                c._noChange_Grayed = lvi.SubItems[24].Text == "true" ? true : false;
                control.Add(c);
            }
            return control;
        }
        
        private void dBTRefactorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvControls.SelectedIndices.Count <= 0)
                return;
            DBTFild refactor =new DBTFild(lvControls, mControls);
            refactor.ShowDialog(this);
        }

        private void commitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int f=0;
            List<_CONTROL> controls = ListViewToListControl(lvControls);
            foreach (_CONTROL c in controls)
            {
                if (!mDBManager.UpdateControl(c))
                    f++; 
            }
            LoadGrid();
            lvControls.Select();
            lvControls.Focus();
        }

        private void btnRefreshFields_Click(object sender, EventArgs e)
        {
            tsslStatus.Text = "Creating DB...";
            Application.DoEvents();

            mDBManager.Delete(true);

            if (!mDBManager.Open())
            {
                MessageBox.Show("Error opening DB");
                return;
            }

            tsslStatus.Text = "Creating Fields && Table tables...";
            Application.DoEvents();

            FieldsReader fr = new FieldsReader(mDBManager, tbApp.Text);
            fr.Search();

            tsslStatus.Text = "Creating Controls Classes table...";
            Application.DoEvents();

            ControlsReader cr = new ControlsReader(mDBManager, tbApp.Text);
            cr.Search();

            tsslStatus.Text = "Ready";
            Application.DoEvents();
        }

    }
}
