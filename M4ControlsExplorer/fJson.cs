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
    public partial class fJson : Form
    {
        public fJson(string idd, string json, string errors)
        {
            InitializeComponent();
            this.Text = idd;
            tbErrors.Text = errors;
            tbJson.Text = json;
            tbJson.SelectionStart = tbJson.SelectionLength;
            Clipboard.SetData(DataFormats.Text, json);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(tbJson.Text);
        }
    }
}
