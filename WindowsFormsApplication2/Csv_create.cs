using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Csv_create : Form
    {
        public Csv_create()
        {
            InitializeComponent();
        }
        string Category,Name_file, Format, Size_file, Tipe_size,All_line;
        Random size = new Random(DateTime.Now.Second);
        Random random = new Random();
        string[] Readme = new string[] {"ReadMe!","Readme!!!", "ReadMe", "ReadMe", "Readme!!" };
        string[] Sites = new string[] { "Kickass.to", "Digtorrent.org", "Thepiratebay.sx", "YIFY-TORRENTS.COM", "1337x.org","extremlymtorrents.me","H33t.to",
                    "BTS.to","Flashtorrents.org","Divxhunt.me"};
       
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            Create_data();

        }

        private void Csv_create_Load(object sender, EventArgs e)
        {
            textBox_min.Text = "25";
            textBox_max.Text = "50";
        }

        void Create_data()
        {
        string[] Data = textBox1.Lines;
        foreach(var s in Data)
            {
                textBox2.Text+=Create_line(s)+"\r\n";
                textBox2.Text += Category + "," + Name + "," + Readme[random.Next(0,Readme.Length)] + "," + "txt" + "," + Ranodom_size(1,2) / 1024 + "," + Tipe_size + ",0,0,0,"+random.Next(0,1)+",0" + "\r\n";
                if (Regex.IsMatch(Name, @"[s,S]erial|[k,K]ey"))
                textBox2.Text += Category + "," + Name + "," + "Keys" + "," + "txt" + "," + Ranodom_size(1, 2)/1024 + "," + Tipe_size + ",0,0,0," + random.Next(0, 1) + ",0" + "\r\n";
                if(Regex.IsMatch(Name, @"[c,C]rack"))
                    textBox2.Text += Category + "," + Name + "," + "crack/run_560d4" + "," + "exe" + "," + Ranodom_size(1, 2) + "," + Tipe_size + ",0,0,0," + random.Next(0, 1) + ",0" + "\r\n";

                for(var i=random.Next(0,Convert.ToInt32(files_variable(0,true)));i< Convert.ToInt32(files_variable(0, true)); i++)
                {
                    textBox2.Text += files_variable(i, false);
                }                
            }
            System.IO.File.WriteAllText(@"data" + (textBox1.Lines.Length-1) + ".csv", textBox2.Text, Encoding.UTF8);
        }



        string Create_line(string Namein) {
            Size_file =(Ranodom_size(Convert.ToInt16(textBox_min.Text), Convert.ToInt16(textBox_max.Text)).ToString());
            Format = "exe";
            Name = Name_file = Namein;
            Tipe_size = "B";
            Category = "software";
            All_line = Category + "," + Name + "," + Name_file + "," + Format + "," + Size_file + "," + Tipe_size + ",0,0,0,1,0";            
            return All_line;
        }
        int Ranodom_size(int min,int max)
        {
         
            min = min * 1048576;
            max = max * 1048576;
         //   Random size = new Random(DateTime.Now.Second);
            return size.Next(min, max);
        }

        string torrent_dl()
        {
         return "Torrent downloaded from "+Sites[random.Next(0,Sites.Length)];
        }
        public string files_variable(int i,bool getcount)
        {

            string[] _files_variable = new string[] {
        Category + "," + Name + "," + torrent_dl() + "," + "txt" + "," + Ranodom_size(1, 2) / 1024 + "," + Tipe_size + ",0,0,0," + random.Next(0, 1) + ",0" + "\r\n",
        Category + "," + Name + "," + Name + "," + "nfo" + "," + Ranodom_size(1, 2) / 1024 + "," + Tipe_size + ",0,0,0," + random.Next(0, 1) + ",0" + "\r\n" };
         if (!getcount)
                return _files_variable[i];
            else
                return _files_variable.Length.ToString();

        }
    }
}
