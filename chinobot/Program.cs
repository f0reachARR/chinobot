using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace chinobot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("523439661:AAGfyMnZdEz9jjNn36UwMMya4xtRiRxFfHU");

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
                if (e.Message.Text == "/おはよう")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "おはようございます" + e.Message.Chat.Username + "さん");
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "今日も天気がいいですね");
                }
                else if (e.Message.Text == "/おやすみ")
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "おやすみなさい" + e.Message.Chat.Username + "さん");
                else
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "ごめんなさい、よくわからないです。");

                }
            }

        }
    }
}
