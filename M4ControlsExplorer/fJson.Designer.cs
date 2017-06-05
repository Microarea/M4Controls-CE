namespace M4ControlsExplorer
{
    partial class fJson
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fJson));
            this.tbJson = new System.Windows.Forms.TextBox();
            this.tbErrors = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbJson
            // 
            this.tbJson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbJson.Location = new System.Drawing.Point(0, 143);
            this.tbJson.Multiline = true;
            this.tbJson.Name = "tbJson";
            this.tbJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbJson.Size = new System.Drawing.Size(671, 398);
            this.tbJson.TabIndex = 0;
            // 
            // tbErrors
            // 
            this.tbErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbErrors.Location = new System.Drawing.Point(0, 0);
            this.tbErrors.Multiline = true;
            this.tbErrors.Name = "tbErrors";
            this.tbErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbErrors.Size = new System.Drawing.Size(671, 108);
            this.tbErrors.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(551, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Copy to Clipboard";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fJson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 541);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbErrors);
            this.Controls.Add(this.tbJson);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fJson";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Json";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbJson;
        private System.Windows.Forms.TextBox tbErrors;
        private System.Windows.Forms.Button button1;
    }
}