using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{    
     internal class Telegram_bot
    {
        BackgroundWorker bw;
        public Telegram_bot()
        {

        }

        public void  Start()
        {
           

            this.bw = new BackgroundWorker();
            this.bw.DoWork += this.bw_DoWork; // метод bw_DoWork будет работать асинхронно
            BtnRunClick(null,null);
            
        }

       async  void bw_DoWork(object sender, DoWorkEventArgs e)
        {
         //   var worker = sender as BackgroundWorker; // получаем ссылку на класс вызвавший событие
        //    var key = e.Argument as String; // получаем ключ из аргументов


            try
            {                
                var Bot = new Telegram.Bot.TelegramBotClient("828891254:AAEzi5yCDjdJAX4Pwbrd7UWN5rqBPnQiPj8"); // инициализируем API
                await  Bot.DeleteWebhookAsync();
                await Bot.SendTextMessageAsync(-369470069, "отправка без запроса");

         /*   int offset = 0; // отступ по сообщениям
                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset); // получаем массив обновлений

                    foreach (var update in updates) // Перебираем все обновления
                    {
                        Console.WriteLine(update.Type);
                        offset = update.Id + 1;
                    }


                    foreach (var update in updates) // Перебираем все обновления
                    {
                        var message = update.Message;
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        {
                        

                            if (message.Text == "/saysomething")
                            {
                                // в ответ на команду /saysomething выводим сообщение
                                await Bot.SendTextMessageAsync(message.Chat.Id, "тест",
                                       replyToMessageId: message.MessageId);
                            }
                        }
                        offset = update.Id + 1;
                    }

                }                
                */

            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                MessageBox.Show(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }  
        }

        void BtnRunClick(object sender, EventArgs e) // обработка клика кнопки
        {
            var text = "1213213423452342";
            if (text!="" && this.bw.IsBusy != true)
            {
                this.bw.RunWorkerAsync(text); // передаем эту переменную в виде аргумента методу bw_DoWork
            }
        }

     async public void Sent_message(string text)
        {
            var Bot = new Telegram.Bot.TelegramBotClient("828891254:AAEzi5yCDjdJAX4Pwbrd7UWN5rqBPnQiPj8"); // инициализируем API
            await Bot.DeleteWebhookAsync();
            await Bot.SendTextMessageAsync(-369470069, text);
        }

    }
}
