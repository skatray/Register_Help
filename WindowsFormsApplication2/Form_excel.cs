using ExcelLibrary.SpreadSheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form_excel : Form
    {
        public Form_excel()
        {
            InitializeComponent();
        }
        string str;
        public string mail,name,oldname,filepath,nik;
        private void Form_excel_Load(object sender, EventArgs e)
        {

            if (mail == null)
            {
                mail = name + "_" + oldname + "@yahoo.com";
            }
            
           
                textBox1.Text = mail;            
                textBox2.Text = nik;
                textBox3.Text = "Qwerty14";
            
            

        }

         

        public  string LastRowCell(Microsoft.Office.Interop.Excel.Application XL)
        {
            int lastrow = XL.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
            for (int i = lastrow; i >= 1; i--)
            {
                if (XL.Cells[i, 1].Value != null)
                {
                    str = i.ToString();
                    break;
                }
            }

            return str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string _time = Convert.ToString(DateTime.Now.ToShortTimeString()+" "+DateTime.Now.Day +"."+DateTime.Now.Month+"." + DateTime.Now.Year);
            
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " +
                                       filepath +
                                       "; Extended Properties=Excel 8.0";
            string wquery = "Insert into [Лист1$] (email,pass,login,discription,mailtrue,Createtime) Values (?,?,?,?,?,?)";

            OleDbConnection conn = new OleDbConnection(connString);
            OleDbCommand wcmd = new OleDbCommand(wquery, conn);
            conn.Open();           
            OleDbParameter namep = wcmd.Parameters.Add("email", OleDbType.VarChar, 80);
            OleDbParameter passp = wcmd.Parameters.Add("pass", OleDbType.VarChar, 80);
            OleDbParameter loginp = wcmd.Parameters.Add("login", OleDbType.VarChar, 80);
            OleDbParameter discriptionp = wcmd.Parameters.Add("discription", OleDbType.VarChar,580);           
            OleDbParameter mail_yes = wcmd.Parameters.Add("mailtrue", OleDbType.VarChar, 80);
            OleDbParameter Createtime = wcmd.Parameters.Add("Createtime", OleDbType.VarChar, 80);



            namep.Value = textBox1.Text;
            passp.Value = textBox3.Text;
            loginp.Value = textBox2.Text;
            mail_yes.Value = checkBox1.Checked;
            discriptionp.Value = textBox4.Text.ToString();
            Createtime.Value = _time;
            wcmd.ExecuteNonQuery();
            conn.Close();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }


    }
}
