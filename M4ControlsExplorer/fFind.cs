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
    public partial class fFind : Form
    {
        public fFind()
        {
            InitializeComponent();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                DialogResult = DialogResult.OK;
        }

        public string GetText()
        {
            return tbFind.Text;
        }
    }
}
