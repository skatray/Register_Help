using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
     class Dro1c
    {
      static List<int> dots = new List<int>();
        
       public  void DrowGraphics(List<Nick> Bannedlist)
        {
         Image im=  Image.FromFile(@"image.png");
         Image bmpSave = ScaleImageMain(im, Bannedlist);
         bmpSave.Save(@"resaut.png");
            
        }
        static Image ScaleImageMain(Image img, List<Nick> Bannedlist)
        {
            Monitor Moni2 = new Monitor();
            Image dest = new Bitmap(img.Width, img.Height);
              Graphics gr = Graphics.FromImage(dest);              
              Pen blackPen = new Pen(Color.Red, 2);
              Pen blackPen2 = new Pen(Color.Green, 1);
             Pen Hours = new Pen(Color.LightGray, 1);
            gr.DrawImage(img, 0, 0, img.Width, img.Height);

            var hour = TimeSpan.FromHours(1);
            for (var i = 0; i < 24; i++)
            {
                   int x = (hour.Hours * 60) * 1772 / 1440 * (img.Width - 128) / 1772;
                //   int x = (hour.Hours * 60)  / 1440 * (img.Width - 128);
                hour = hour + TimeSpan.FromHours(1);
                gr.DrawLine(Hours, x + 100, 576, x + 100, 111);
            }


            dots.Clear();
            foreach (var dot in Bannedlist)
            {
                
                var x = Time_to_pixcel(dot.time);
                dots.Add(x);
            }
            foreach (var dot in Bannedlist)
            {
                int y=0;
               
               var x = Time_to_pixcel(dot.time);
                foreach (var getcount in dots)
                {
                    if (getcount == x)
                    {
                        y++;
                    }
                }              
                gr.DrawLine(blackPen, x+100, 574, x+100, 564-(y*5));               
            }

            
            
            var x1= Time_to_pixcel(DateTime.Now);
            gr.DrawLine(blackPen2, x1 + 100, 550, x1 + 100, 110);

            int Time_to_pixcel(DateTime time)
            {
                var time2 = time.Hour * 60 + time.Minute;
                /*  var timepersent2 = time2 * 100 / 1440;
                  float xw = (timepersent2 * (img.Width - 128) / 100);
                */
                var timepersent2 = time2*288 / 1440;
                float xw = (timepersent2 * (img.Width - 128) / 288);
                int x =Convert.ToInt32(Math.Round(xw,0));
                return x;
            }

            



            return dest;
        }
        

    }

    
}
