using ExcelLibrary.BinaryFileFormat;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;

namespace WindowsFormsApplication2
{
    public partial class Monitor : Form
    {


        public Monitor()
        {
            InitializeComponent();

        }
        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer
        {
            Interval = (10000 * Properties.Settings.Default.timebox)
        };
        bool button_click = false;
        List<Nick> nikz = new List<Nick>();
        List<Nick> Bannedlist = new List<Nick>();       
        private void Monitor_Load(object sender, EventArgs e)
        {

            Readbanlist();                 


            textBox2.Text = Properties.Settings.Default.Юрл_для_чекера;
            Timebox.Text = Properties.Settings.Default.timebox.ToString();
            foreach (var d in Properties.Settings.Default.NIKs)
            {
                textBox1.Text += d + "\r\n";
            }
            textBox1.Text += Properties.Settings.Default.NIKs.Count.ToString();
            
            

        }
        


        public void Requvest(object sender, EventArgs e)

        {
            textBox1.Clear();
            textBox1.Text ="--- "+DateTime.Now.ToString()+" ---\r\n\r\n";
            string url = "https://" + Properties.Settings.Default.Юрл_для_чекера;
            string html = "", name = "";
            string r = "";
            int count = 0, maxcount = 0;
            try
            {
                using (var http = new HttpClient())
                {

                    html = http.GetStringAsync(url + "/search/1/0/3/300/").Result;
                    if (eneble_page2.Checked)
                        html += http.GetStringAsync(url + "/search/1/1/3/300/").Result;
                }                

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(html);

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='detDesc']");
                var output = nodes
                   .GroupBy(word => word.InnerText);

                foreach (var zzz in output)
                {
                    textBox1.Text += zzz.Key + ": " + zzz.Count() + "\r\n";
                    count = count + zzz.Count();
                }
              
                textBox1.Text += "------------" + count;
            }
            catch { textBox1.Text = "Problem Connect!"; }

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Tick += new System.EventHandler(Requvest);
            if (!button_click)
            {
                button_click = true;
                timer1.Start();
                textBox1.Text = "Started!";
                button1.Text = "Stop";
            }
            else
            {
                button_click = false;
                timer1.Stop();
                button1.Text = "Start";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Юрл_для_чекера = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (Timebox.Text != ""|| Convert.ToInt32(Timebox.Text)<5)
            {
                Properties.Settings.Default.timebox = Convert.ToInt32(Timebox.Text);
                timer1.Interval = (1000 * Convert.ToInt32(Timebox.Text));
                Properties.Settings.Default.Save();

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // --------------------

            timer1.Tick += new System.EventHandler(Getbigdata);
            if (!button_click)
            {
                button_click = true;
                timer1.Start();
                textBox1.Text = "Started!";
                button2.Text = "Stop";
            }
            else
            {
                button_click = false;
                timer1.Stop();
                button2.Text = "Start";
            }

            // ---------------------

            Getbigdata(null,null);
        }


        public void Getbigdata(object sender, EventArgs e)

        {
            textBox1.Clear();
            nikz.Clear();

            string url = "https://" + Properties.Settings.Default.Юрл_для_чекера;
            string html = "";
            int count = 0;
            try
            {
                using (var http = new HttpClient())
                {
                    for (int i = 0; i < Convert.ToInt16(countpages.Text); i++)
                    {
                        var url2 = url + "/search/1/" + i + "/3/300/";
                        html += http.GetStringAsync(url2).Result;
                    }
                }


                var htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(html);

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='detDesc']");
                var output = nodes
                   .GroupBy(word => word.InnerText);

                foreach (var zzz in output)
                {
                    textBox1.Text += zzz.Key + ": \t" + zzz.Count() + "\r\n";
                    count = count + zzz.Count();
                    nikz.Add(new Nick() { name = zzz.Key, count = zzz.Count() });
                }
                textBox1.Text += "------------" + count + "------------";
            }
            catch { textBox1.Text = "Problem Connect!"; }

      //      try
       //     {
                using (var http = new HttpClient())
                {
                    for (int i = 0; i < nikz.Count; i++)
                    {
                        var url2 = "https://" + Properties.Settings.Default.Юрл_для_чекера + "/user/" + nikz[i].name;
                     //   html = http.GetStringAsync(url2).Result;
                        html = response(url2);
                        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                        htmlDoc.LoadHtml(html);
                        HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='detLink']");
                    if (nodes == null) { nikz[i].status = "Not found"; Addtobanlist(nikz[i].name); goto Notfaund; }
                        string torrent = nodes[0].Attributes["href"].Value;
                    //  html = http.GetStringAsync("https://" + Properties.Settings.Default.Юрл_для_чекера + torrent).Result;
                    html = response("https://" + Properties.Settings.Default.Юрл_для_чекера + torrent);
                    htmlDoc.LoadHtml(html);
                    nodes = htmlDoc.DocumentNode.SelectNodes("//div[@id='err']");
                        if (nodes != null) { nikz[i].status = "Deleted";
                        Addtobanlist(nikz[i].name);
                    } else { nikz[i].status = "Good"; }
                    }
                Notfaund:
                textBox1.Clear();
                string lines = @"<html><head>" +
                    "<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\">\r\n" +
                    "<meta http-equiv=\"Refresh\" content=\"" + Convert.ToInt32(Timebox.Text) + "\" />" +
                    "<link rel=\"apple-touch-icon\" sizes=\"180x180\" href=\"/apple-touch-icon-180x180.png\">" +
                "</head>\r\n<body>\r\n<div style=\"margin:10% 25% 0 25%; width:50%\">" + textBox1.Text;// + "</div></body></html>";
                    lines += "<p>------------"  +DateTime.Now.ToString()  + " -----</p>";
                    foreach (var person in nikz)
                    {
                        textBox1.Text += person.name + " количество: \t" + person.count + " ответ: \t" +
                            person.status + "\r\n";
                        lines += "<p>" + person.name + " количество: \t" + person.count + " ответ: \t" +
                            person.status + "</p>\r\n";
                    }
                    textBox1.Text += "------------" + count + " -----\r\n";
                    lines += "<p>------------" + count + " -------------</p>";
                    textBox1.Text += "-------------Banned List--------\r\n";
                    lines += "<span style = \"color:#e74c3c\" >";
                    lines += "<p>-----------Banned List----</p>";

                foreach (var bperson in Bannedlist)
                      {
                       textBox1.Text += bperson.name + "\t " + bperson.time.ToString()+"\r\n";
                       lines += "<p>" + bperson.name + "\t" + bperson.time.ToString() + "</p>";
                      }
                lines += "</span>";
                foreach (var person in nikz)
                {
                    if (person.status == "Deleted")
                    {
                        lines += "<p style=\"text-align:center\">&nbsp;</p><p style = \"text-align:center\" ><span style = \"font-size:72px\" ><span style = \"color:#e74c3c\" ><span style = \"background-color:#000000\" >Banned</span ></span></span></p>";
                        break;
                    }
                }
                    lines += "</div></body></html>";
                    System.IO.File.WriteAllText(@"test.html", lines, Encoding.UTF8);
                }
            //       }
            //     catch {

            //           textBox1.Text += "Problem Connect!";

            //      }

           
        }

        private void buttontest_Click(object sender, EventArgs e)
        {

            textBox1.Text=response("https://thepiratebay.org/search/1/0/99/300");

        }

        string response(string url)
        {
            WebClient client = new WebClient();
            try
            {
               return client.DownloadString(url);
              
            }
            catch (System.Net.WebException exception)
            {
                string responseText;

                using (var reader = new System.IO.StreamReader(exception.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                   return responseText.ToString();
                   
                }
            }
        }

        void Addtobanlist(string name)
        {
            int i = 0;
            foreach (var person in Bannedlist)
            {
                if (person.name == name)
                    i++;
            }
            if (i == 0)
            {
                Bannedlist.Add(new Nick() { name = name, time = DateTime.Now });
                var lines = name + ";" + DateTime.Now;
                System.IO.StreamWriter writer = new System.IO.StreamWriter("banlist.txt", true);
                writer.WriteLine(lines);
                writer.Close();
            }

        }
        void Readbanlist()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"banlist.txt");
                foreach (string line in lines)
                {
                    string[] words= line.Split(';');                    
                    Bannedlist.Add(new Nick() { name = words[0], time = Convert.ToDateTime(words[1])});
                }
                textBox1.Text += "BD Banned list connect\r\n";
            }
            catch { }
        }

       
    }
}