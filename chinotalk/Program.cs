using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Web;
using System.Net;
using Codeplex.Data;
using System.Diagnostics;

namespace gotiusatalk
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("APIKEY");
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
                Console.WriteLine(e.Message.Text);

                switch (e.Message.Text) {

                    case "/stop":

                    Process.Start(@"C:\Users\jun07\Documents\Visual Studio 2017\Projects\chinobot\chinobot\bin\Debug\chinobot.exe");
                    System.Threading.Thread.Sleep(10);
                    Environment.Exit(100);
                        break;
                    
                    default:
  
                    Console.WriteLine(e.Message.Text);

                    var url = " https://chatbot-api.userlocal.jp/api/chat?message=" + e.Message.Text + "&key=0556302ad5d7df3280bb";
                    var req = WebRequest.Create(url);
                    var res = req.GetResponse();
                    var s = res.GetResponseStream();

                    dynamic json = DynamicJson.Parse(s);

                    var sendtext = json.result;

                    Bot.SendTextMessageAsync(e.Message.Chat.Id, sendtext);
                        break;
            }
            }
        }
    }
}
