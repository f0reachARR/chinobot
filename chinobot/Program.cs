using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Net;
using Codeplex.Data;
using System.Text;

namespace chinobot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("APIKEY");
        const string NO_VALUE = "---";

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

                switch (e.Message.Text)
                {

                    case "/hello":

                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "おはようございます" + e.Message.NewChatMember + "さん");
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "今日も天気がいいですね");
                        break;

                    case "/omikuji":

                        System.Random r = new System.Random();
                        int i1 = r.Next(10);
                        if (i1 == 1)
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "おめでとうございます。大吉です。");
                        }
                        else if (i1 == 2)
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "大凶です。元気出してお兄ちゃん。");
                        }
                        else
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "中吉です。");
                        }
                        break;

                    case "/leave":

                        Bot.LeaveChatAsync(e.Message.Chat.Id);
                        break;

                    case "/wether":

                        Bot.SendTextMessageAsync(e.Message.Chat.Id, GetWeatherText());
                        break;

                    case "/help":

                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "現在使用できるコマンドは\n/hello\n/leave\n/omikuji\n/wether\n/help\nです");
                        break;

                    default:
                        break;

                    
                }

                if (0 <= e.Message.Text.IndexOf("/twitter"))
                {
                    try
                    {
                        string twiurl = "https://twitter.com/" + e.Message.Text.Remove(0, 9);
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, twiurl);
                    }
                    catch
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "正しく入力してください");
                    }

                }
                else if (0 <= e.Message.Text.IndexOf("/twiarchive"))
                {
                    try
                    {
                        string archiveurl = "https://web.archive.org/save/https://twitter.com/" + e.Message.Text.Remove(0, 12) + "/";
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, archiveurl);
                    }
                    catch
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "正しく入力してください");
                    }
                }

            }

            
            

                        
        }


        private static string GetWeatherText()
        {
            var url = "http://weather.livedoor.com/forecast/webservice/json/v1?city=130010";
            var req = WebRequest.Create(url);

            using (var res = req.GetResponse())
            using (var s = res.GetResponseStream())
            {
                dynamic json = DynamicJson.Parse(s);

                //天気(今日)
                dynamic today = json.forecasts[0];

                string dateLabel = today.dateLabel;
                string date = today.date;
                string telop = today.telop;

                var sbTempMax = new StringBuilder();
                dynamic todayTemperatureMax = today.temperature.max;
                if (todayTemperatureMax != null)
                {
                    sbTempMax.AppendFormat("{0}℃", todayTemperatureMax.celsius);
                }
                else
                {
                    sbTempMax.Append(NO_VALUE);
                }

                var sbTempMin = new StringBuilder();
                dynamic todayTemperatureMin = today.temperature.min;
                if (todayTemperatureMin != null)
                {
                    sbTempMin.AppendFormat("{0}℃", todayTemperatureMin.celsius);
                }
                else
                {
                    sbTempMin.Append(NO_VALUE);
                }

                //天気概況文
                var situation = json.description.text;

                //Copyright
                var link = json.copyright.link;
                var title = json.copyright.title;

                return string.Format("{0}\n天気 {1}\n最高気温 {2}\n最低気温 {3}\n\n{4}\n\n{5}\n{6}",
                    date,
                    telop,
                    sbTempMax.ToString(),
                    sbTempMin.ToString(),
                    situation,
                    link,
                    title
                    );

            }
        }

    }
}