using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Net;
using Codeplex.Data;
using System.Text;
using System.Collections.Generic;
using NMeCab;
using System.Diagnostics;

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

                    case "/test":

                        Bot.SendTextMessageAsync(e.Message.Chat.Id, phrasetext());
                        break;

                    case "/starttest":

                        chinotest(e);
                        break;

                    case "おはよう":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "おはようございます");
                        break;
                    case "おはようチノちゃん":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "おはようございます");
                        System.Threading.Thread.Sleep(10);
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "あまり名前で呼ばないでください...");
                        System.Threading.Thread.Sleep(10);
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "恥ずかしいです...//");
                        break;
                    case "チノちゃん":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "はい。なんでしょう");
                        break;
                    case "お兄ちゃんってよんで！":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "嫌です");
                        break;
                    case "おやすみ":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "おやすみなさい");
                        break;
                    case "おやすみチノちゃん":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "おやすみなさい。明日も早いですよ");
                        break;
                    case "もう寝るね":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "今日はまだ...お話していたい...気分です...");
                        break;
                    case "/chatstart":
                        Process.Start(@"C:\Users\jun07\Documents\Visual Studio 2017\Projects\chinobot\chinobot\bin\Debug\chinobot.exe");
                        System.Threading.Thread.Sleep(10);
                        Environment.Exit(100);
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
                else if (0 <= e.Message.Text.IndexOf("/ma"))
                {
                    try
                    {
                        string matext = e.Message.Text.Remove(0, 4);
                        var tagger = MeCabTagger.Create();

                        string result = tagger.Parse(matext);

                        Bot.SendTextMessageAsync(e.Message.Chat.Id, result);
                    }
                    catch
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "エラーです。");
                    }
                }

            }
            else if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.StickerMessage)
            {
                if (e.Message.Sticker.FileId == "CAADBQADCgAD4impGGXQnVlzLgGkAg")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "https://stickershop.line-scdn.net/stickershop/v1/sticker/7115479/IOS/sticker_sound.m4a");
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

        private static void chino(MessageEventArgs e)
        {
            

        }

        private static void chinotest(MessageEventArgs e)
        {
            while (true) {

                if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                {
                    Console.WriteLine(e.Message.Text);

                    switch (e.Message.Text)
                    {
                        
                        case "/stop":
                            break;

                    }
                }
                else
                {
                    break;
                }
            }
        }

        private static string phrasetext()
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(@"D:chino.txt", System.Text.Encoding.UTF8))
            {
                string line = "";
                List<string> list = new List<string>();         //空のListを作成する

                // test.txtを1行ずつ読み込んでいき、末端(何もない行)までwhile文で繰り返す
                while ((line = file.ReadLine()) != null)
                {
                    list.Add(line);
                }

                System.Random l = new System.Random();
                int l1 = l.Next(10);

                string a = list[l1];
                return a;
            }
        }
    }
}