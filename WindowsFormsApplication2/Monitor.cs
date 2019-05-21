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
            this.DesktopLocation = Properties.Settings.Default.положение;
        }
        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer
        {
            Interval = (10000 * Properties.Settings.Default.timebox)
        };
        
        bool button_click = false;
        List<Nick> nikz = new List<Nick>();
        List<Nick> Bannedlist = new List<Nick>();
        List<Nick> Ignorelist = new List<Nick>();
        Dro1c dro = new Dro1c();
        BackgroundWorker bw;
        BackgroundWorker worker;
        string menu_html = null;

        public  void Monitor_Load(object sender, EventArgs e)
        {
         //   Addtobanlist("test_person_for_telegam");
            worker = new BackgroundWorker();
            worker.DoWork += Getbigdata2;
            this.bw = new BackgroundWorker();
            this.bw.DoWork += this.Alarm;

            Readbanlist();
            Readignorelist();
            Bannedlist.Sort(delegate (Nick mc1, Nick mc2)            
            { return mc2.time.CompareTo(mc1.time); });

            textBox2.Text = Properties.Settings.Default.Юрл_для_чекера;
            Timebox.Text = Properties.Settings.Default.timebox.ToString();
            countpages.Text = Properties.Settings.Default.countpages.ToString();
            foreach (var d in Properties.Settings.Default.NIKs)
            {
                textBox1.Text += d + "\r\n";
            }
            textBox1.Text += Properties.Settings.Default.NIKs.Count.ToString();

            
            string[] lines = System.IO.File.ReadAllLines(@"menu.html");
            foreach (string line in lines)
            {
                menu_html+= line;                
            }
          

            dro.DrowGraphics(Bannedlist);
          
    }

        

        public void Requvest(object sender, EventArgs e)

        {
            textBox1.Clear();
            textBox1.Text ="--- "+DateTime.Now.ToString()+" ---\r\n\r\n";
            string url = "https://" + Properties.Settings.Default.Юрл_для_чекера;
            string html = "";
            
            int count = 0;
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
            try { 
            if (Timebox.Text != ""|| Convert.ToInt32(Timebox.Text)<5)
            {
                Properties.Settings.Default.timebox = Convert.ToInt32(Timebox.Text);
                timer1.Interval = (1000 * Convert.ToInt32(Timebox.Text));
                Properties.Settings.Default.Save();

            }
            }
            catch { Timebox.Text = Properties.Settings.Default.timebox.ToString(); }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer1.Tick += new System.EventHandler(timer);
            
            if (!button_click)
            {
                button_click = true;
                timer1.Start();
                textBox1.Text = "Started!";
                button2.Text = "Stop";
                timer(null, null);
            }
            else
            {
                button_click = false;
                timer1.Stop();
                button2.Text = "Start";
            }

            // ---------------------            
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
                textBox1.Text += "Собираю ники ";
                using (var http = new HttpClient())
                {
                    for (int i = 0; i < Convert.ToInt16(countpages.Text); i++)
                    {
                        var url2 = url + "/search/1/" + i + "/3/300/";
                        html += http.GetStringAsync(url2).Result;
                    }
                }
                textBox1.Text += "Собрал \r\n ";

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

           try
           {
                textBox1.Clear();
          //      textBox1.Text += "Захожу в аккаунты в поисках торрент ссылок \r\n ";
                using (var http = new HttpClient())
                {
                    for (int i = 0; i < nikz.Count; i++)
                    {
                    var url2 = "https://" + Properties.Settings.Default.Юрл_для_чекера + "/user/" + nikz[i].name;
                    html = response(url2);
                    var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='detLink']");
                    if(nodes == null){
                                                  
                            System.IO.File.WriteAllText(@"log/" + nikz[i].name + "log.html", html, Encoding.UTF8);
                            System.IO.File.WriteAllText(@"log/" + nikz[i].name + "file.txt", url2, Encoding.UTF8);

                            nodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='cf-error-type']");
                            if (nodes != null)
                            {
                                nikz[i].status = "Error 502";
                                goto Notfaund;
                            }
                            nikz[i].status = "Not found";
                            Addtobanlist(nikz[i].name);
                                goto Notfaund;
                    }                        
                    string torrent = nodes[0].Attributes["href"].Value;
            //            textBox1.Text += "Захожу в торренты \r\n ";
                        html = response("https://" + Properties.Settings.Default.Юрл_для_чекера + torrent);
                    htmlDoc.LoadHtml(html);
                    nodes = htmlDoc.DocumentNode.SelectNodes("//div[@id='err']");
                    if (nodes != null) {
                        nikz[i].status = "Deleted";
                     string info=nikz[i].status+"\r\n";
                            info += "https://" + Properties.Settings.Default.Юрл_для_чекера + torrent + "\r\n";
                            info += nodes[0].InnerHtml + "\r\n";
                     System.IO.File.WriteAllText(@"log/" + nikz[i].name + "fileDELETE.txt", info, Encoding.UTF8);
                     System.IO.File.WriteAllText(@"log/" + nikz[i].name + "logDELETE.html", html, Encoding.UTF8);
                            Addtobanlist(nikz[i].name);
                    } else {
                        nikz[i].status = "Good";
                    }
                        Notfaund:;
                    }
                
               
                string lines = @"<html><head>" +
                    "<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\">\r\n" +
                    "<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1\">" +
                    "<meta http-equiv=\"Refresh\" content=\"" + Convert.ToInt32(Timebox.Text) + "\" />" +
                    "<link rel=\"apple-touch-icon\" sizes=\"180x180\" href=\"/apple-touch-icon-180x180.png\">" +
                    "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\">" +
                    "</head>\r\n<body>\r\n";
                    lines += menu_html;
                    lines += "<p>--------"  +DateTime.Now.ToString()+ " ---</p>";
                    lines += ("<table style=\"\"><tbody>");
                    foreach (var person in nikz)
                    {
                    textBox1.Text += person.name + " количество: \t" + person.count + " ответ: \t" +
                    person.status + "\r\n";
                    lines += "<tr><td><a class=\"good\" target=\"_blank\" href=\"http://thepiratebay.org/user/" + person.name + "\">" + person.name + "</a></td><td>" + person.count + "</td><td>" +
                    person.status + "</td></tr>\r\n";
                    }
                    lines += ("</tbody></table>");
                    textBox1.Text += "------------" + count + " -----\r\n";
                    lines += "<p>------------" + count + " -------------</p>";
                    textBox1.Text += "-------------Banned List--------\r\n";
                    lines += "<span style = \"color:#e74c3c;margin:auto;\" >";
                    lines += "<p>-----------Banned List----</p>";
                    lines += "</span>";
                    lines += ("<table style=\"width: 100 %;color:#e74c3c;margin:auto;\"><tbody>");
                  
                    foreach (var bperson in Bannedlist)
                    {
                    textBox1.Text += bperson.name + "\t " + bperson.time.ToString()+"\r\n";
                    if(DateTime.Now.AddMinutes(-30)<bperson.time){
                     lines += "<tr style = \"color:#a52019;font-size:24px;\"><td><a class=\"firsts\" target=\"_blank\" href=\"http://thepiratebay.org/user/" + bperson.name + "\">" + bperson.name + "</a></td><td>" + bperson.time.ToString() + "</td></tr>";
                     }else
                    lines += "<tr ><td><a class=\"second\" target=\"_blank\" href=\"http://thepiratebay.org/user/" + bperson.name + "\">" + bperson.name + "</a></td><td>" + bperson.time.ToString() + "</td></tr>";
                    }
                    lines += ("</tbody></table>");
                    
                foreach (var person in nikz)
                {
                    if (person.status == "Deleted")
                    {
                    lines += "<p style=\"text-align:center\">&nbsp;</p><p style = \"text-align:center\" ><span style = \"font-size:72px\" ><span style = \"color:#e74c3c\" ><span style = \"background-color:#000000\" >Banned</span ></span></span></p>";
                    break;
                    }
                }
                lines+= "<p><a class=\"firsts\" href=\"http://redcams.asuscomm.com/tpb/log/?sort=t&rev=1\">LoG File's</a></p>";
                lines += "<img width = \"100%\" src = \"resaut.png\" />";
                lines += 
                        "</body></html>";
                    System.IO.File.WriteAllText(@"test.html", lines, Encoding.UTF8);
                }
                    dro.DrowGraphics(Bannedlist);
              }
              catch {
              textBox1.Text += "Возникла ошибка!";
                     }
                     

        }

        private void buttontest_Click(object sender, EventArgs e)
        {

         timer(null,null);
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
                Bannedlist.Insert(0, new Nick() { name = name, time = DateTime.Now });
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
            catch {
                textBox1.Text += "With BD has problem!!!\r\n";
            }
        }
        void Readignorelist()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"ignorelist.txt");
                foreach (string line in lines)
                {
                  Ignorelist.Add(new Nick() { name = line });
                }
                textBox1.Text += "BD ignore list connect\r\n";
            }
            catch
            {
                textBox1.Text += "With ignore list has problem!!!\r\n";
            }
        }


        public  void timer(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();

        }
        //------------------------------v2----------------------------/
         void Getbigdata2(object sender, DoWorkEventArgs e)
        {
            string personfortelegram = "Banned List\r\n";
            textBox1.Invoke(new Action(() => textBox1.Clear()));
            progressBar1.Invoke(new Action(() => progressBar1.Value = 0));
            nikz.Clear();
            string url = "https://" + Properties.Settings.Default.Юрл_для_чекера;
            string html = "";
            int count = 0;
            try
            {
         //       textBox1.Text += "Собираю ники ";
                using (var http = new HttpClient())
                {
                    for (int i = 0; i < Convert.ToInt16(countpages.Text); i++)
                    {
                        var url2 = url + "/search/1/" + i + "/3/300/";
                        html += http.GetStringAsync(url2).Result;
                    }
                }
         //       textBox1.Text += "Собрал \r\n ";

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(html);

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='detDesc']");
                var output = nodes
                   .GroupBy(word => word.InnerText);

                foreach (var zzz in output)
                {
                    textBox1.Invoke(new Action(() => textBox1.Text += zzz.Key + ": \t" + zzz.Count() + "\r\n"));
                    count = count + zzz.Count();
                 /*   bool test_ignorelist = false;
                    foreach(var aaa in Ignorelist)
                    {
                        if (aaa.name == zzz.Key)
                            test_ignorelist = true;
                    }
                    if(!test_ignorelist) */
                    nikz.Add(new Nick() { name = zzz.Key, count = zzz.Count() });
                }
                textBox1.Invoke(new Action(() => textBox1.Text += "---" + count + "---"+output.Count()+"----------"));
            }
            catch { textBox1.Invoke(new Action(() => textBox1.Text += "Problem Connect!")); }
           
            try
            {
                textBox1.Invoke(new Action(() => textBox1.Clear()));                
                using (var http = new HttpClient())
                {
                    progressBar1.Invoke(new Action(() => progressBar1.Maximum = nikz.Count));
                    for (int i = 0; i < nikz.Count; i++)
                    {
                        bool test_ignorelist = false;
                        foreach (var aaa in Ignorelist)
                        {
                            if (aaa.name == nikz[i].name)
                                test_ignorelist = true;
                        }
                        if (test_ignorelist)
                        {
                            nikz[i].status="WhiteList";
                            goto Notfaund;
                        }


                        var url2 = "https://" + Properties.Settings.Default.Юрл_для_чекера + "/user/" + nikz[i].name;
                        html = response(url2);
                        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                        htmlDoc.LoadHtml(html);
                        HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='detLink']");
                        if (nodes == null)
                        {

                        //    System.IO.File.WriteAllText(@"log/" + nikz[i].name + "log.html", html, Encoding.UTF8);
                        //    System.IO.File.WriteAllText(@"log/" + nikz[i].name + "file.txt", url2, Encoding.UTF8);

                            nodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='cf-error-type']");
                            if (nodes != null)
                            {
                                nikz[i].status = "Error 502";
                                goto Notfaund;
                            }
                            nikz[i].status = "Not found";
                            Addtobanlist(nikz[i].name);
                            goto Notfaund;
                        }
                        string torrent = nodes[0].Attributes["href"].Value;
                        
                        html = response("https://" + Properties.Settings.Default.Юрл_для_чекера + torrent);
                        htmlDoc.LoadHtml(html);
                        nodes = htmlDoc.DocumentNode.SelectNodes("//div[@id='err']");
                        if (nodes != null)
                        {
                            nikz[i].status = "Deleted";
                            string info = nikz[i].status + "\r\n";
                            info += "https://" + Properties.Settings.Default.Юрл_для_чекера + torrent + "\r\n";
                            info += nodes[0].InnerHtml + "\r\n";
                        //    System.IO.File.WriteAllText(@"log/" + nikz[i].name + "fileDELETE.txt", info, Encoding.UTF8);
                        //    System.IO.File.WriteAllText(@"log/" + nikz[i].name + "logDELETE.html", html, Encoding.UTF8);
                            Addtobanlist(nikz[i].name);
                        }
                        else
                        {
                            nikz[i].status = "Good";
                        }
                        Notfaund:;
                        progressBar1.Invoke(new Action(() => progressBar1.Value = progressBar1.Value+1));
                    }


                    string lines = @"<html><head>" +
                        "<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\">\r\n" +
                        "<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1\">" +
                        "<meta http-equiv=\"Refresh\" content=\"" + Convert.ToInt32(Timebox.Text) + "\" />" +
                        "<link rel=\"apple-touch-icon\" sizes=\"180x180\" href=\"/apple-touch-icon-180x180.png\">" +
                        "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\">" +
                        "<link rel=\"stylesheet\" type=\"text/css\" href=\"css_menu.css\">" +
                        "<script type=\"text / javascript\" src=\"js.js\"></script>" +
                        "</head>\r\n<body>\r\n";
                    lines += menu_html;
                    lines += "<div id=\"home\"><p>-------- " + DateTime.Now.ToString() + " --------</p>";
                    lines += ("<table style=\"\"><tbody>");
                    nikz.Sort(delegate (Nick mc1, Nick mc2)
                    { return mc1.status.CompareTo(mc2.status); });
                    foreach (var person in nikz)
                    {
                        textBox1.Invoke(new Action(() => textBox1.Text += person.name + " количество: \t" + person.count + " ответ: \t" +
                        person.status + "\r\n"));
                        lines += "<tr><td><a class=\"good\" target=\"_blank\" href=\"http://thepiratebay.org/user/" + person.name + "\">" + person.name + "</a></td><td>" + person.count + "</td><td>" +
                        person.status + "</td></tr>\r\n";
                    }
                    lines += ("</tbody></table>");
                    textBox1.Invoke(new Action(() => textBox1.Text += "-------- " + count + " -- " + nikz.Count + " ---\r\n"));
                    lines += "<p>-------- " + count + " -- "+ nikz.Count + " ---</p></div>";
                    textBox1.Invoke(new Action(() => textBox1.Text += "--------Banned List--------\r\n"));
                    lines += "<div id=\"banned\" style = \"color:#e74c3c;margin:auto;\">";
                    lines += "<p>--------Banned List--------</p>";
                    lines += "</div>";
                    lines += ("<table style=\"width: 100 %;color:#e74c3c;margin:auto;\"><tbody>");
                    string banlist=null;
                    foreach (var bperson in Bannedlist)
                    {
                        banlist += bperson.name + "\t " + bperson.time.ToString() + "\r\n";
                        if (DateTime.Now.AddMinutes(-30) < bperson.time)
                        {
                            lines += "<tr style = \"color:#a52019;font-size:24px;\"><td><a class=\"firsts\" target=\"_blank\" href=\"http://thepiratebay.org/user/" + bperson.name + "\">" + bperson.name + "</a></td><td>" + bperson.time.ToString() + "</td></tr>";
                        }
                        else
                            lines += "<tr ><td><a class=\"second\" target=\"_blank\" href=\"http://thepiratebay.org/user/" + bperson.name + "\">" + bperson.name + "</a></td><td>" + bperson.time.ToString() + "</td></tr>";
                        if (DateTime.Now.AddMinutes(-2) < bperson.time)
                        personfortelegram += bperson.name + "\t " + bperson.time.ToString() + "\r\n"; ;
                    }
                    textBox1.Invoke(new Action(() => textBox1.Text += banlist + "\r\n"));
                    lines += ("</tbody></table>");

                    foreach (var person in nikz)
                    {
                        if (person.status == "Deleted")
                        {
                            lines += "<p style=\"text-align:center\">&nbsp;</p><p style = \"text-align:center\" ><span style = \"font-size:72px\" ><span style = \"color:#e74c3c\" ><span style = \"background-color:#000000\" >Banned</span ></span></span></p>";
                            break;
                        }
                    }
                    lines += "<p><a class=\"firsts\" href=\"http://redcams.asuscomm.com/tpb/log/?sort=t&rev=1\">LoG File's</a></p>";
                    lines += "<div id=\"graphic\"><img width = \"100%\" src = \"resaut.png\" /></div>";
                    lines +=
                            "</body></html>";
                    System.IO.File.WriteAllText(@"test.html", lines, Encoding.UTF8);
                    if (this.bw.IsBusy != true&&personfortelegram!= "Banned List\r\n")
                    {
                        this.bw.RunWorkerAsync(personfortelegram); // передаем эту переменную в виде аргумента методу bw_DoWork
                    }

                }
                dro.DrowGraphics(Bannedlist);
                }    catch{textBox1.Invoke(new Action(() => textBox1.Text += "Возникла ошибка!")); }

          
        }
        //-----------------------------------------------------------/
        private void countpages_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (countpages.Text != "" || Convert.ToInt32(countpages.Text) < 5)
                {
                    if(Convert.ToInt32(countpages.Text)>30)
                    { countpages.Text = "30"; }
                    
                    Properties.Settings.Default.countpages = Convert.ToInt32(countpages.Text);                    
                    Properties.Settings.Default.Save();

                }
            }
            catch { countpages.Text = Properties.Settings.Default.countpages.ToString(); }
        }

        private void countpages_Click(object sender, EventArgs e)
        {
            countpages.SelectAll();
        }


       void Alarm(object sender, DoWorkEventArgs e)
        {
            var text = e.Argument as String;
            if (Alarm_mode.Checked)
            {
             /*   foreach(var banned in nikz)
                {
                    if(banned.status== "Deleted" || banned.status == "Not found") {

                        if (Bannedlist.Any(d => d.name != banned.name)){

                        }
                    }
                }*/

                Telegram_bot t_bot = new Telegram_bot();
                t_bot.Sent_message(text);
                Thread.Sleep(1000 * 60 * 10);
            }
           
        }
        

    }
}