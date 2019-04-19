using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class AddToIgnoreList : Form
    {
        public AddToIgnoreList()
        {
            InitializeComponent();
        }
        public string Nick;
        public int count;
        private void AddToIgnoreList_Load(object sender, EventArgs e)
        {
            label1.Text = "Do you want add " +Nick+" with "+count+ " to ignore list?";
        }

        private void button_addtolist_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.NIKs.Add(Nick);
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.Close();            
        }
    }
}
