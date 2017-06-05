using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using M4ControlsDBMaker;

namespace M4ControlsExplorer
{
    public partial class DBTFild : Form
    {
        private DBTRefactor Refactor;
        public DBTFild(ListView lvControls, List<_CONTROL> mControls)
        {
            Refactor = new DBTRefactor(lvControls, mControls);

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refactor.DBTclass = DBTClass.Text;
            Refactor.DBTnamespace = NameSapce.Text;
            Refactor.ParsBindData(BindingData.Text);
            Refactor.RefactorFild();
            DialogResult = DialogResult.OK;
        }
        public class DBTRefactor
        {
            private List<_CONTROL> mControls = null;
            private Dictionary<string, string> BindData;
            private ListView lvControls;
            private string dBTclass;
            private string dBTnamespace;

            public string DBTclass {
                get { return dBTclass; }
                set { this.dBTclass = value; }
            }
            public string DBTnamespace {
                get { return dBTnamespace; }
                set { this.dBTnamespace = value; }
            }
            public DBTRefactor(ListView lvControls, List<_CONTROL> mControls)
            {
               this.mControls = mControls;
                this.lvControls = lvControls;
                BindData =new Dictionary<string, string>();
            }

            public List<ListViewItem> LoadSelectedList()
            {
                _CONTROL c = new _CONTROL();
                ListViewItem lvp = null;
                List<ListViewItem> controls = new List<ListViewItem>();

                foreach (ListViewItem lvi in lvControls.SelectedItems)
                {
                    controls.Add(lvi);
                }
                return controls;
            }

            public bool ParsBindData(string text)
            {
                List<string> bindData = new List<string>(text.Split(';'));
                string nS, fild;
                foreach (string row in bindData)
                {
                    if (String.IsNullOrWhiteSpace(row))
                        continue;
                    nS = row.Substring(row.IndexOf("_NS_FLD(\"") + 9);
                    fild = nS.Substring(nS.IndexOf(")") + 2).Trim();
                    fild = fild.Substring(0, fild.Length - 1);
                    nS = nS.Substring(0, nS.IndexOf(")") - 1);

                    //fild = row.Substring((row.IndexOf("\")") + 2), row.LastIndexOf(")")).Trim();
                    //fild = fild.Substring((row.IndexOf("\")") + 2), row.LastIndexOf(")")).Trim();
                    if (!String.IsNullOrWhiteSpace(fild))
                        BindData.Add(fild, nS);
                }
                return true;
            }

            //c._field = lvi.SubItems[16].Text;
            //c._fieldnamespace = lvi.SubItems[17].Text;
            //c._dbtnamespace = lvi.SubItems[13].Text;
            public string Refactor()
            {
                foreach (ListViewItem lvC in lvControls.SelectedItems)
                {
                     if (lvC.SubItems[13].Text == DBTnamespace)
                        lvC.SubItems[17].Text = BindData[lvC.SubItems[16].Text];
                }

                return "";
            }


            public string RefactorFild()
            {
                foreach (_CONTROL cl in mControls)
                {
                    if (cl._dbtnamespace == DBTnamespace && !string.IsNullOrEmpty(cl._field) )
                        if(BindData.ContainsKey(cl._field))
                            cl._fieldnamespace = BindData[cl._field];
                }
                foreach (ListViewItem lvC in lvControls.Items)
                {
                    if (lvC.SubItems[13].Text == DBTnamespace)
                        if (BindData.ContainsKey(lvC.SubItems[16].Text))
                            lvC.SubItems[17].Text = BindData[lvC.SubItems[16].Text];
                }
                return "";
            }

        }
    }
    


    
}
