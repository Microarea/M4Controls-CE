namespace M4ControlsExplorer
{
    partial class DBTFild
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
            this.Refact = new System.Windows.Forms.Button();
            this.BindingData = new System.Windows.Forms.TextBox();
            this.DBTClass = new System.Windows.Forms.TextBox();
            this.DBTClassL = new System.Windows.Forms.Label();
            this.NameSapceL = new System.Windows.Forms.Label();
            this.NameSapce = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Refact
            // 
            this.Refact.Location = new System.Drawing.Point(634, 12);
            this.Refact.Name = "Refact";
            this.Refact.Size = new System.Drawing.Size(115, 23);
            this.Refact.TabIndex = 0;
            this.Refact.Text = "button1";
            this.Refact.UseVisualStyleBackColor = true;
            this.Refact.Click += new System.EventHandler(this.button1_Click);
            // 
            // BindingData
            // 
            this.BindingData.Location = new System.Drawing.Point(12, 88);
            this.BindingData.Multiline = true;
            this.BindingData.Name = "BindingData";
            this.BindingData.Size = new System.Drawing.Size(737, 533);
            this.BindingData.TabIndex = 1;
            // 
            // DBTClass
            // 
            this.DBTClass.Location = new System.Drawing.Point(91, 19);
            this.DBTClass.Name = "DBTClass";
            this.DBTClass.Size = new System.Drawing.Size(369, 20);
            this.DBTClass.TabIndex = 2;
            // 
            // DBTClassL
            // 
            this.DBTClassL.AutoSize = true;
            this.DBTClassL.Location = new System.Drawing.Point(16, 22);
            this.DBTClassL.Name = "DBTClassL";
            this.DBTClassL.Size = new System.Drawing.Size(57, 13);
            this.DBTClassL.TabIndex = 3;
            this.DBTClassL.Text = "DBT Class";
            // 
            // NameSapceL
            // 
            this.NameSapceL.AutoSize = true;
            this.NameSapceL.Location = new System.Drawing.Point(16, 48);
            this.NameSapceL.Name = "NameSapceL";
            this.NameSapceL.Size = new System.Drawing.Size(69, 13);
            this.NameSapceL.TabIndex = 4;
            this.NameSapceL.Text = "Name Space";
            // 
            // NameSapce
            // 
            this.NameSapce.Location = new System.Drawing.Point(91, 45);
            this.NameSapce.Name = "NameSapce";
            this.NameSapce.Size = new System.Drawing.Size(369, 20);
            this.NameSapce.TabIndex = 5;
            // 
            // DBTFild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 633);
            this.Controls.Add(this.NameSapce);
            this.Controls.Add(this.NameSapceL);
            this.Controls.Add(this.DBTClassL);
            this.Controls.Add(this.DBTClass);
            this.Controls.Add(this.BindingData);
            this.Controls.Add(this.Refact);
            this.Name = "DBTFild";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Refact;
        private System.Windows.Forms.TextBox BindingData;
        private System.Windows.Forms.TextBox DBTClass;
        private System.Windows.Forms.Label DBTClassL;
        private System.Windows.Forms.Label NameSapceL;
        private System.Windows.Forms.TextBox NameSapce;
    }
}