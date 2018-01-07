using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainForm
{
    public partial class MdlListForm : Form
    {
        public event EventHandler EventMdlSelected;
        public MdlListForm()
        {
            InitializeComponent();
        }
        public MdlListForm(List<string> strList)
        {
            InitializeComponent();
            foreach (string str in strList)
            {
                listBox1.Items.Add(str);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EventMdlSelected != null)
                EventMdlSelected(listBox1.SelectedItem, new System.EventArgs());
            this.Close();
        }
    }
}
