using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Progressbar : Form
    {
        public Progressbar()
        {
            
            InitializeComponent();
        }
        
        public int max,value;
        private void Progressbar_Load(object sender, EventArgs e)
        {

            progressBar1.Maximum = max-1;
            backgroundWorker1.RunWorkerAsync(); 
           
            //    progressBar1.Value = value;
                
                
           
           
        }

        

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
                string _file_path =null;
                try
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(@"option.ini", true);
                    _file_path = file.ReadLine();
                }
                catch
                {
                    using (FileStream fs = File.Create("option.ini"))
                    {
                        openFileDialog1.ShowDialog();
                        Byte[] info = new UTF8Encoding(true).GetBytes(openFileDialog1.FileName);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                
                }


                List<string> niks = new List<string> { };
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
            
                for (int i = 1; i < niks.Count; i++)
                {
                ((Helper)Owner).status_akka(niks.ElementAt(i));

       
              backgroundWorker1.ReportProgress(i);
                }
             
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            
            this.Close();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Progressbar_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Helper)Owner).status();
        }
    }
}
