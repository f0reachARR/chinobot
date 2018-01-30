using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Drawing.Imaging;
using System.Net;
using System.IO;

namespace chinobot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("API");

        static void Main(string[] args)
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
            {
                if (e.Message.Text == "おはよう")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "おはようございます" + e.Message.Chat.Username + "さん");
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "今日も天気がいいですね");
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "http://crypto.peijun.info/wp-content/uploads/2018/01/7ca47fac-s.jpg");

                }
                else if (e.Message.Text == "おやすみ")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "おやすみなさい" + e.Message.Chat.Username + "さん");
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "http://crypto.peijun.info/wp-content/uploads/2018/01/maxresdefault.jpg");
                }
                else if (e.Message.Text == "おみくじ")
                {
                    System.Random r = new System.Random();
                    int i1 = r.Next(10);
                    //int i1 = 2; for debug
                    if (i1 == 1)
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "おめでとうございます。大吉です。");
                    }
                    else if (i1 == 2)
                    {
                        /* trying send photo
                        string url = "URL";
                        WebClient wc = new WebClient();
                        Stream stream = wc.OpenRead(url);
                        Bitmap bitmap = new Bitmap(stream);
                        stream.Close();
                        */

                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "大凶です。元気出してお兄ちゃん。");
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "https://wp.me/a9uW6J-1C");
                    }
                    else
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "中吉です。");
                    }
                }
                else
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "ごめんなさい、よくわからないです。");
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "http://crypto.peijun.info/wp-content/uploads/2018/01/CezUMjHWwAAUtLq.jpg");
                }
            }

        }

       }
}
