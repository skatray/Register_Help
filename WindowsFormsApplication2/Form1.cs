using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace WindowsFormsApplication2
{
    public partial class Helper : Form
    {
       
        public Helper()
        {
            
             
            TopMost = true;
            InitializeComponent();
            this.DesktopLocation = Properties.Settings.Default.положение;
            
        }

        public string _oldname, _name, _mail, _file_path, _nik, _status, _comment, _mailsufiks;
          int _count, _ban;
          List<string> niks = new List<string> { };
        private void Form1_Load(object sender, EventArgs e)
        {
            
            _oldname = null;
            _name = null;
            _mail = null;
            _mailsufiks = null;
        
            this.ShowInTaskbar = false;
            proverka_bd();
            comboBox1.SelectedIndex = 4;
            read_file();

           




        }

        void proverka_bd()
        {

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@"option.ini", true);

                _file_path += file.ReadLine();
                file.Close();

                if(_file_path=="")
                {                   
                    File.Delete("option.ini");
                   // proverka_bd();
                }

            }
            catch
            {
                using (FileStream fs = File.Create("option.ini"))
                {
                    openFileDialog1.ShowDialog();

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(openFileDialog1.FileName);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
                Form1_Load(null, null);
            }
        
        
        }


        private FormWindowState _OldFormState;
        void read_file()
        {            
            string line;
            List<string> oldname = new List<string> { };
            List<string> name = new List<string> { };
           
            oldname.Clear();
            name.Clear();
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(@"lastname.txt");
            while ((line = file.ReadLine()) != null)
            {
                oldname.Add(line);                
            }
            file.Close();
            file.Dispose();
            Random rand_name = new Random();            
            _oldname =oldname.ElementAt(rand_name.Next(0, oldname.Count - 1));

            // Read the file and display it line by line.
         //   System.IO.StreamReader file = new System.IO.StreamReader(@"c:\name.txt");
            file = new System.IO.StreamReader(@"name.txt");
            while ((line = file.ReadLine()) != null)
            {
                name.Add(line);
            }
            file.Close();
            _name = name.ElementAt(rand_name.Next(0, name.Count - 1));
           

            


            _mail = _name + "" + _oldname;
        
        }

        string comment()
        {
            List<string> comment = new List<string> { };
            Random rand_name = new Random();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"сomments.txt");            
            while ((line = file.ReadLine()) != null)
            {
                comment.Add(line);
            }
            file.Close();

            _comment = comment.ElementAt(rand_name.Next(0, comment.Count));
            return _comment;

        }
       


        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            // ShowInTaskbar = false;
            Hide();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {


            

        }

        private void contextMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {


            if (e.Button == MouseButtons.Left)
            {
                if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
                {
                    _OldFormState = WindowState;
                    WindowState = FormWindowState.Minimized;
                }
                else
                {
                    Show();
                    WindowState = _OldFormState;
                }

            }
            if (e.Button == MouseButtons.Right)
            {
                           

                contextMenuStrip1.Show(Cursor.Position);
            }

        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            read_file();
            
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void имяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_name);
        }

        private void фамилияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_oldname);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("Qwerty14");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Random phone = new Random();
            Clipboard.SetText("63"+phone.Next(1111111, 9999999).ToString());
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            _mail = Clipboard.GetText();
            
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_mail + _mailsufiks);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Random avatar = new Random();
            Random s = new Random();
      
      switch (s.Next(0, 2))
            {
                case 0: Clipboard.SetText("http://www.avatava.org/pics100/2d-3d/avatar-"+avatar.Next(1,52)+".jpg"); break;
                case 1: Clipboard.SetText("http://www.doodoo.ru/avatar/animation/av" + avatar.Next(100, 600) + ".gif"); break;
                case 2: Clipboard.SetText("http://www.doodoo.ru/avatar/animation/av" + avatar.Next(100, 600) + ".gif"); break;
          }
        }

        private void сохранитьВExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _nik = _name + _oldname;
            if (_nik.Length > 10)
            {
                int cut;
                cut = 10 - _nik.Length;
                cut = Math.Abs(cut);
                _nik = _nik.Remove(_nik.Length - cut);
                
            }
            Form_excel excel = new Form_excel();
            excel.mail = _mail+_mailsufiks;
            excel.name = _name;
            excel.oldname = _oldname;
            excel.filepath = _file_path;
            excel.nik = _nik;

            excel.Show(this);
        }

        void proverka_na_akt()
        {
            _status= null;                      
            niks.Clear();

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + _file_path +
                                     @"; Extended Properties=""Excel 12.0 Xml;HDR=No"";";

            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [Лист1$] WHERE [F1] IS NOT NULL AND [F2] IS NOT NULL";
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            niks.Add(reader["F3"].ToString());
                        }
                    }
                }
                connection.Close();
            }

            
                      

        
            
        
        }


        public void status_akka(string login_site)
       
        {
            

            string url = "http://kat.cr/user/" + login_site + "/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
                    string responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    string error = "<span class=\"aclColor_1\">User</span>";
                    string ok,notfound="Page not found";
                    ok = string.Format("<span class=\"aclColor_1\">Uploader</span>");


                    if (responseFromServer.IndexOf(ok) > 0)
                    {
                        _status += login_site + " - " + "Uploader\r\n";
                        _count = _count + 1;
                    }
                    else
                    {

                        //     MessageBox.Show("Status: Uknown");
                    }

                    if (responseFromServer.IndexOf(error) > 0)
                    {
                       


                        if (responseFromServer.IndexOf("DELETED USER") > 0)
                        {
                            _status += login_site + " - " + "DELETED User \r\n";
                            _ban = _ban + 1;
                        }
                        else
                        {
                            _status += login_site + " - " + "User \r\n";
                        }
                    }
                    if (responseFromServer.IndexOf(notfound) > 0)
                    {
                        _status += login_site + " - " + "Not Found! \r\n";

                    }

                    
                }
                response.Close();
            }
            catch
            {
                _status += login_site + " - " + "Not Found! \r\n";                
            }

        }


        

        private void проверкаАккаунтовToolStripMenuItem_Click(object sender, EventArgs e)
        {

            _count = 0;
            _ban = 0;
         //   proverka_bd();
            proverka_na_akt();
            Progressbar prog = new Progressbar();
            prog.max = niks.Count;
            prog.Show(this);

   //         for (int i = 1; i < niks.Count; i++)
   //         {
    //            status_akka(niks.ElementAt(i));
  //              prog.value = 3;
    //            prog.Refresh();
                
      //      }
       
        }

        private void открытьExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var excelapp = new Excel.Application();
            excelapp.Visible = true;
            excelapp.Workbooks.Open(_file_path);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                TopMost = true;
            }
            else
            {
                TopMost = false;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
          //  Monitor com = new Monitor();
            new System.Threading.Thread(NewForm).Start();            
          //  com.Show(this);
        }

        void NewForm()
        {
            Monitor com = new Monitor();
            com.ShowDialog();
        }

        private void Ник(object sender, EventArgs e)
        {
            _nik = _name + _oldname;
            if (_nik.Length > 10)
            {
                int cut;
                cut = 10 - _nik.Length;
                cut = Math.Abs(cut);
               _nik = _nik.Remove(_nik.Length - cut);
               Clipboard.SetText(_nik);
            }
            Clipboard.SetText(_nik);
        }

       
        public void status()
        {
            resaut _res = new resaut();
            _res._resaut = _status + "\r\n Активных: " + _count + "\r\n Бан: " + _ban + "\r\n Всего: " + niks.Count.ToString();
            _res.Show(this);
          //  MessageBox.Show(_status + "\r\n Активных: " + _count + "\r\n Бан: " + _ban + "\r\n Всего: " + niks.Count.ToString());

        }

        private void Form1_Move(object sender, EventArgs e)
        {
            Properties.Settings.Default.положение = this.DesktopLocation;
            Properties.Settings.Default.Save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.положение = this.DesktopLocation;
            Properties.Settings.Default.Save();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.Opacity = Convert.ToDouble(trackBar1.Value)/10;
            this.Refresh();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
            Comment com = new Comment();
            com.comm = comment(); 
            com.Show(this);
                       
            
            
        } 


      

        private void button14_Click(object sender, EventArgs e)
        {
            string _add_commnet;
            _add_commnet = Clipboard.GetText();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"сomments.txt", true))
            {
                file.WriteLine(_add_commnet);
            }

            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _mailsufiks = comboBox1.Text;
        }





    }
}
