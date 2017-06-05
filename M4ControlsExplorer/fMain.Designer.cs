namespace M4ControlsExplorer
{
    partial class fMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.bSearch = new System.Windows.Forms.Button();
            this.tbApp = new System.Windows.Forms.TextBox();
            this.bBrowseERP = new System.Windows.Forms.Button();
            this.cmsControlsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiControlsEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiControlsCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiControlsFind = new System.Windows.Forms.ToolStripMenuItem();
            this.dBTRefactorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilLVImages = new System.Windows.Forms.ImageList(this.components);
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslStatusModule = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslStatusClass = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslStatusIDC = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.chControlsModule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsDBT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsDBTNamespace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsRecordPointer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsRecordClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsIDC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsRuntimeClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsHkl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvControls = new System.Windows.Forms.ListView();
            this.chControlsFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsClassIDD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsNamespace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTileStyle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTileText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsBodyEditIDC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsBodyEditText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsFieldNamespace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chControlsCombo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cControlsButton = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHidden = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chGrayed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chNoChange_Grayed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tvTree = new System.Windows.Forms.TreeView();
            this.cmsTreeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiTreeCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGenerateJson = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTreeImages = new System.Windows.Forms.ImageList(this.components);
            this.lApplication = new System.Windows.Forms.Label();
            this.cbModules = new System.Windows.Forms.ComboBox();
            this.cbAllModules = new System.Windows.Forms.CheckBox();
            this.cbAllFiles = new System.Windows.Forms.CheckBox();
            this.cbFiles = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnRefreshFields = new System.Windows.Forms.Button();
            this.chMinValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMaxValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChars = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRows = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsControlsMenu.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.cmsTreeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // bSearch
            // 
            this.bSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSearch.Location = new System.Drawing.Point(1297, 25);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(75, 23);
            this.bSearch.TabIndex = 6;
            this.bSearch.Text = "Search";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // tbERP
            // 
            this.tbApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApp.Location = new System.Drawing.Point(15, 27);
            this.tbApp.Name = "tbERP";
            this.tbApp.Size = new System.Drawing.Size(940, 20);
            this.tbApp.TabIndex = 0;
            this.tbApp.Validating += new System.ComponentModel.CancelEventHandler(this.tbERP_Validating);
            // 
            // bBrowseERP
            // 
            this.bBrowseERP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBrowseERP.Location = new System.Drawing.Point(955, 26);
            this.bBrowseERP.Name = "bBrowseERP";
            this.bBrowseERP.Size = new System.Drawing.Size(24, 23);
            this.bBrowseERP.TabIndex = 1;
            this.bBrowseERP.Text = "...";
            this.bBrowseERP.UseVisualStyleBackColor = true;
            this.bBrowseERP.Click += new System.EventHandler(this.bBrowseERP_Click);
            // 
            // cmsControlsMenu
            // 
            this.cmsControlsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiControlsEdit,
            this.tsmiControlsCopy,
            this.tsmiControlsFind,
            this.dBTRefactorToolStripMenuItem,
            this.commitToolStripMenuItem});
            this.cmsControlsMenu.Name = "cmsControlsMenu";
            this.cmsControlsMenu.Size = new System.Drawing.Size(144, 114);
            this.cmsControlsMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsControlsMenu_Opening);
            // 
            // tsmiControlsEdit
            // 
            this.tsmiControlsEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsmiControlsEdit.Image")));
            this.tsmiControlsEdit.Name = "tsmiControlsEdit";
            this.tsmiControlsEdit.Size = new System.Drawing.Size(143, 22);
            this.tsmiControlsEdit.Text = "Edit";
            this.tsmiControlsEdit.Click += new System.EventHandler(this.tsmiControlsEdit_Click);
            // 
            // tsmiControlsCopy
            // 
            this.tsmiControlsCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsmiControlsCopy.Image")));
            this.tsmiControlsCopy.Name = "tsmiControlsCopy";
            this.tsmiControlsCopy.Size = new System.Drawing.Size(143, 22);
            this.tsmiControlsCopy.Text = "Copy";
            this.tsmiControlsCopy.Click += new System.EventHandler(this.tsmiControlsCopy_Click);
            // 
            // tsmiControlsFind
            // 
            this.tsmiControlsFind.Image = global::M4ControlsExplorer.Properties.Resources.find;
            this.tsmiControlsFind.Name = "tsmiControlsFind";
            this.tsmiControlsFind.Size = new System.Drawing.Size(143, 22);
            this.tsmiControlsFind.Text = "Find";
            this.tsmiControlsFind.Click += new System.EventHandler(this.tsmiControlsFind_Click);
            // 
            // dBTRefactorToolStripMenuItem
            // 
            this.dBTRefactorToolStripMenuItem.Image = global::M4ControlsExplorer.Properties.Resources.clipboard_copy;
            this.dBTRefactorToolStripMenuItem.Name = "dBTRefactorToolStripMenuItem";
            this.dBTRefactorToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.dBTRefactorToolStripMenuItem.Text = "DBT Refactor";
            this.dBTRefactorToolStripMenuItem.Click += new System.EventHandler(this.dBTRefactorToolStripMenuItem_Click);
            // 
            // commitToolStripMenuItem
            // 
            this.commitToolStripMenuItem.Name = "commitToolStripMenuItem";
            this.commitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.commitToolStripMenuItem.Text = "Commit";
            this.commitToolStripMenuItem.Click += new System.EventHandler(this.commitToolStripMenuItem_Click);
            // 
            // ilLVImages
            // 
            this.ilLVImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilLVImages.ImageStream")));
            this.ilLVImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ilLVImages.Images.SetKeyName(0, "ok_button.png");
            this.ilLVImages.Images.SetKeyName(1, "warning.png");
            this.ilLVImages.Images.SetKeyName(2, "ov_error.png");
            this.ilLVImages.Images.SetKeyName(3, "cancel.png");
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus,
            this.tsslStatusModule,
            this.tsslStatusClass,
            this.tsslStatusIDC});
            this.ssStatus.Location = new System.Drawing.Point(0, 639);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(1384, 24);
            this.ssStatus.TabIndex = 11;
            // 
            // tsslStatus
            // 
            this.tsslStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(43, 19);
            this.tsslStatus.Text = "Ready";
            // 
            // tsslStatusModule
            // 
            this.tsslStatusModule.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslStatusModule.Name = "tsslStatusModule";
            this.tsslStatusModule.Size = new System.Drawing.Size(4, 19);
            this.tsslStatusModule.Visible = false;
            // 
            // tsslStatusClass
            // 
            this.tsslStatusClass.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslStatusClass.Name = "tsslStatusClass";
            this.tsslStatusClass.Size = new System.Drawing.Size(4, 19);
            this.tsslStatusClass.Visible = false;
            // 
            // tsslStatusIDC
            // 
            this.tsslStatusIDC.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslStatusIDC.Name = "tsslStatusIDC";
            this.tsslStatusIDC.Size = new System.Drawing.Size(4, 19);
            this.tsslStatusIDC.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 639);
            this.splitter1.TabIndex = 12;
            this.splitter1.TabStop = false;
            // 
            // chControlsModule
            // 
            this.chControlsModule.Text = "Module";
            this.chControlsModule.Width = 100;
            // 
            // chControlsClass
            // 
            this.chControlsClass.Text = "Class";
            this.chControlsClass.Width = 200;
            // 
            // chControlsDBT
            // 
            this.chControlsDBT.Text = "DBT";
            this.chControlsDBT.Width = 200;
            // 
            // chControlsDBTNamespace
            // 
            this.chControlsDBTNamespace.Text = "DBT Namespace";
            this.chControlsDBTNamespace.Width = 200;
            // 
            // chControlsRecordPointer
            // 
            this.chControlsRecordPointer.Text = "Record Pointer";
            this.chControlsRecordPointer.Width = 200;
            // 
            // chControlsRecordClass
            // 
            this.chControlsRecordClass.Text = "Record Class";
            this.chControlsRecordClass.Width = 200;
            // 
            // chControlsField
            // 
            this.chControlsField.Text = "Field";
            this.chControlsField.Width = 200;
            // 
            // chControlsIDC
            // 
            this.chControlsIDC.Text = "IDC";
            this.chControlsIDC.Width = 200;
            // 
            // chControlsRuntimeClass
            // 
            this.chControlsRuntimeClass.Text = "Runtime Class";
            this.chControlsRuntimeClass.Width = 200;
            // 
            // chControlsHkl
            // 
            this.chControlsHkl.Text = "HotKeyLink";
            this.chControlsHkl.Width = 200;
            // 
            // lvControls
            // 
            this.lvControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvControls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chControlsModule,
            this.chControlsFilename,
            this.chControlsClassIDD,
            this.chControlsClass,
            this.chControlsNamespace,
            this.chTileStyle,
            this.chTileText,
            this.chTileSize,
            this.chControlsIDC,
            this.chControlsBodyEditIDC,
            this.chControlsBodyEditText,
            this.chControlsText,
            this.chControlsDBT,
            this.chControlsDBTNamespace,
            this.chControlsRecordPointer,
            this.chControlsRecordClass,
            this.chControlsField,
            this.chControlsFieldNamespace,
            this.chControlsRuntimeClass,
            this.chControlsCombo,
            this.chControlsHkl,
            this.cControlsButton,
            this.chHidden,
            this.chGrayed,
            this.chNoChange_Grayed,
            this.chMinValue,
            this.chMaxValue,
            this.chChars,
            this.chRows});
            this.lvControls.ContextMenuStrip = this.cmsControlsMenu;
            this.lvControls.FullRowSelect = true;
            this.lvControls.GridLines = true;
            this.lvControls.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvControls.LargeImageList = this.ilLVImages;
            this.lvControls.Location = new System.Drawing.Point(410, 54);
            this.lvControls.Name = "lvControls";
            this.lvControls.Size = new System.Drawing.Size(962, 584);
            this.lvControls.SmallImageList = this.ilLVImages;
            this.lvControls.TabIndex = 8;
            this.lvControls.UseCompatibleStateImageBehavior = false;
            this.lvControls.View = System.Windows.Forms.View.Details;
            this.lvControls.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvControls_MouseDoubleClick);
            // 
            // chControlsFilename
            // 
            this.chControlsFilename.Text = "Filename";
            this.chControlsFilename.Width = 200;
            // 
            // chControlsClassIDD
            // 
            this.chControlsClassIDD.Text = "Class IDD";
            this.chControlsClassIDD.Width = 200;
            // 
            // chControlsNamespace
            // 
            this.chControlsNamespace.Text = "Namespace";
            this.chControlsNamespace.Width = 200;
            // 
            // chTileStyle
            // 
            this.chTileStyle.Text = "Tile Style";
            this.chTileStyle.Width = 200;
            // 
            // chTileText
            // 
            this.chTileText.Text = "Tile Text";
            this.chTileText.Width = 200;
            // 
            // chTileSize
            // 
            this.chTileSize.Text = "Tile Size";
            this.chTileSize.Width = 200;
            // 
            // chControlsBodyEditIDC
            // 
            this.chControlsBodyEditIDC.Text = "BodyEdit IDC";
            this.chControlsBodyEditIDC.Width = 200;
            // 
            // chControlsBodyEditText
            // 
            this.chControlsBodyEditText.Text = "BodyEdit Text";
            this.chControlsBodyEditText.Width = 200;
            // 
            // chControlsText
            // 
            this.chControlsText.Text = "Text";
            this.chControlsText.Width = 200;
            // 
            // chControlsFieldNamespace
            // 
            this.chControlsFieldNamespace.Text = "Field Namespace";
            this.chControlsFieldNamespace.Width = 200;
            // 
            // chControlsCombo
            // 
            this.chControlsCombo.Text = "Combo Type";
            this.chControlsCombo.Width = 200;
            // 
            // cControlsButton
            // 
            this.cControlsButton.Text = "Button";
            this.cControlsButton.Width = 200;
            // 
            // chHidden
            // 
            this.chHidden.Text = "Hidden";
            // 
            // chGrayed
            // 
            this.chGrayed.Text = "Grayed";
            // 
            // chNoChange_Grayed
            // 
            this.chNoChange_Grayed.Text = "No Change Grayed";
            // 
            // tvTree
            // 
            this.tvTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvTree.ContextMenuStrip = this.cmsTreeMenu;
            this.tvTree.ImageIndex = 0;
            this.tvTree.ImageList = this.ilTreeImages;
            this.tvTree.Location = new System.Drawing.Point(15, 53);
            this.tvTree.Name = "tvTree";
            this.tvTree.SelectedImageIndex = 0;
            this.tvTree.Size = new System.Drawing.Size(389, 585);
            this.tvTree.TabIndex = 7;
            this.tvTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTree_AfterSelect);
            this.tvTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvTree_MouseClick);
            // 
            // cmsTreeMenu
            // 
            this.cmsTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTreeCopy,
            this.tsmiGenerateJson});
            this.cmsTreeMenu.Name = "cmsControlsMenu";
            this.cmsTreeMenu.Size = new System.Drawing.Size(148, 48);
            this.cmsTreeMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTreeMenu_Opening);
            // 
            // tsmiTreeCopy
            // 
            this.tsmiTreeCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsmiTreeCopy.Image")));
            this.tsmiTreeCopy.Name = "tsmiTreeCopy";
            this.tsmiTreeCopy.Size = new System.Drawing.Size(147, 22);
            this.tsmiTreeCopy.Text = "Copy";
            this.tsmiTreeCopy.Click += new System.EventHandler(this.tsmiTreeCopy_Click);
            // 
            // tsmiGenerateJson
            // 
            this.tsmiGenerateJson.Image = ((System.Drawing.Image)(resources.GetObject("tsmiGenerateJson.Image")));
            this.tsmiGenerateJson.Name = "tsmiGenerateJson";
            this.tsmiGenerateJson.Size = new System.Drawing.Size(147, 22);
            this.tsmiGenerateJson.Text = "Generate Json";
            this.tsmiGenerateJson.Visible = false;
            this.tsmiGenerateJson.Click += new System.EventHandler(this.tsmiGenerateJson_Click);
            // 
            // ilTreeImages
            // 
            this.ilTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTreeImages.ImageStream")));
            this.ilTreeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTreeImages.Images.SetKeyName(0, "application.png");
            this.ilTreeImages.Images.SetKeyName(1, "module.png");
            this.ilTreeImages.Images.SetKeyName(2, "source_code-c.png");
            this.ilTreeImages.Images.SetKeyName(3, "class.png");
            this.ilTreeImages.Images.SetKeyName(4, "control.png");
            this.ilTreeImages.Images.SetKeyName(5, "variables.png");
            this.ilTreeImages.Images.SetKeyName(6, "cancel.png");
            // 
            // lApplication
            // 
            this.lApplication.AutoSize = true;
            this.lApplication.Location = new System.Drawing.Point(12, 10);
            this.lApplication.Name = "lApplication";
            this.lApplication.Size = new System.Drawing.Size(59, 13);
            this.lApplication.TabIndex = 14;
            this.lApplication.Text = "Application";
            // 
            // cbModules
            // 
            this.cbModules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbModules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModules.FormattingEnabled = true;
            this.cbModules.Location = new System.Drawing.Point(985, 27);
            this.cbModules.Name = "cbModules";
            this.cbModules.Size = new System.Drawing.Size(150, 21);
            this.cbModules.Sorted = true;
            this.cbModules.TabIndex = 3;
            this.cbModules.SelectedIndexChanged += new System.EventHandler(this.cbModules_SelectedIndexChanged);
            // 
            // cbAllModules
            // 
            this.cbAllModules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAllModules.AutoSize = true;
            this.cbAllModules.Location = new System.Drawing.Point(985, 10);
            this.cbAllModules.Name = "cbAllModules";
            this.cbAllModules.Size = new System.Drawing.Size(80, 17);
            this.cbAllModules.TabIndex = 2;
            this.cbAllModules.Text = "All Modules";
            this.cbAllModules.UseVisualStyleBackColor = true;
            this.cbAllModules.CheckedChanged += new System.EventHandler(this.cbModulesSelection_CheckedChanged);
            // 
            // cbAllFiles
            // 
            this.cbAllFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAllFiles.AutoSize = true;
            this.cbAllFiles.Location = new System.Drawing.Point(1141, 10);
            this.cbAllFiles.Name = "cbAllFiles";
            this.cbAllFiles.Size = new System.Drawing.Size(61, 17);
            this.cbAllFiles.TabIndex = 4;
            this.cbAllFiles.Text = "All Files";
            this.cbAllFiles.UseVisualStyleBackColor = true;
            // 
            // cbFiles
            // 
            this.cbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiles.FormattingEnabled = true;
            this.cbFiles.Location = new System.Drawing.Point(1141, 27);
            this.cbFiles.Name = "cbFiles";
            this.cbFiles.Size = new System.Drawing.Size(150, 21);
            this.cbFiles.Sorted = true;
            this.cbFiles.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // btnRefreshFields
            // 
            this.btnRefreshFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshFields.Location = new System.Drawing.Point(799, 0);
            this.btnRefreshFields.Name = "btnRefreshFields";
            this.btnRefreshFields.Size = new System.Drawing.Size(156, 23);
            this.btnRefreshFields.TabIndex = 16;
            this.btnRefreshFields.Text = "Refresh Fields";
            this.btnRefreshFields.UseVisualStyleBackColor = true;
            this.btnRefreshFields.Click += new System.EventHandler(this.btnRefreshFields_Click);
            // 
            // chMinValue
            // 
            this.chMinValue.Text = "Min Value";
            // 
            // chMaxValue
            // 
            this.chMaxValue.Text = "Max Value";
            // 
            // chChars
            // 
            this.chChars.Text = "Chars";
            // 
            // chRows
            // 
            this.chRows.Text = "Rows";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 663);
            this.Controls.Add(this.btnRefreshFields);
            this.Controls.Add(this.cbAllFiles);
            this.Controls.Add(this.cbFiles);
            this.Controls.Add(this.cbAllModules);
            this.Controls.Add(this.cbModules);
            this.Controls.Add(this.lApplication);
            this.Controls.Add(this.tvTree);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.lvControls);
            this.Controls.Add(this.bBrowseERP);
            this.Controls.Add(this.tbApp);
            this.Controls.Add(this.bSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fMain";
            this.Text = "Mago4 Controls Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fMain_FormClosing);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            this.cmsControlsMenu.ResumeLayout(false);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.cmsTreeMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bSearch;
        private System.Windows.Forms.TextBox tbApp;
        private System.Windows.Forms.Button bBrowseERP;
        private System.Windows.Forms.ContextMenuStrip cmsControlsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiControlsCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiControlsFind;
        private System.Windows.Forms.ImageList ilLVImages;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.ColumnHeader chControlsModule;
        private System.Windows.Forms.ColumnHeader chControlsClass;
        private System.Windows.Forms.ColumnHeader chControlsDBT;
        private System.Windows.Forms.ColumnHeader chControlsDBTNamespace;
        private System.Windows.Forms.ColumnHeader chControlsRecordPointer;
        private System.Windows.Forms.ColumnHeader chControlsRecordClass;
        private System.Windows.Forms.ColumnHeader chControlsField;
        private System.Windows.Forms.ColumnHeader chControlsIDC;
        private System.Windows.Forms.ColumnHeader chControlsRuntimeClass;
        private System.Windows.Forms.ColumnHeader chControlsHkl;
        private System.Windows.Forms.ListView lvControls;
        private System.Windows.Forms.TreeView tvTree;
        private System.Windows.Forms.Label lApplication;
        private System.Windows.Forms.ComboBox cbModules;
        private System.Windows.Forms.CheckBox cbAllModules;
        private System.Windows.Forms.ImageList ilTreeImages;
        private System.Windows.Forms.ColumnHeader chControlsFilename;
        private System.Windows.Forms.ColumnHeader cControlsButton;
        private System.Windows.Forms.ColumnHeader chControlsText;
        private System.Windows.Forms.ColumnHeader chControlsClassIDD;
        private System.Windows.Forms.ColumnHeader chControlsFieldNamespace;
        private System.Windows.Forms.ColumnHeader chControlsNamespace;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatusClass;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatusIDC;
        private System.Windows.Forms.ContextMenuStrip cmsTreeMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiTreeCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiGenerateJson;
        private System.Windows.Forms.ColumnHeader chTileStyle;
        private System.Windows.Forms.ColumnHeader chTileSize;
        private System.Windows.Forms.ColumnHeader chTileText;
        private System.Windows.Forms.ColumnHeader chControlsCombo;
        private System.Windows.Forms.CheckBox cbAllFiles;
        private System.Windows.Forms.ComboBox cbFiles;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatusModule;
        private System.Windows.Forms.ColumnHeader chControlsBodyEditIDC;
        private System.Windows.Forms.ColumnHeader chControlsBodyEditText;
        private System.Windows.Forms.ToolStripMenuItem tsmiControlsEdit;
        private System.Windows.Forms.ToolStripMenuItem dBTRefactorToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem commitToolStripMenuItem;
        private System.Windows.Forms.Button btnRefreshFields;
        private System.Windows.Forms.ColumnHeader chHidden;
        private System.Windows.Forms.ColumnHeader chGrayed;
        private System.Windows.Forms.ColumnHeader chNoChange_Grayed;
        private System.Windows.Forms.ColumnHeader chMinValue;
        private System.Windows.Forms.ColumnHeader chMaxValue;
        private System.Windows.Forms.ColumnHeader chChars;
        private System.Windows.Forms.ColumnHeader chRows;
    }
}

