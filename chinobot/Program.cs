using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Net;
using Codeplex.Data;
using System.Text;
using System.Collections.Generic;
using NMeCab;
using System.Diagnostics;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;
using System.Web;

namespace chinobot
{
    class Program
    {
        private static TelegramBotClient Bot = null;
        const string NO_VALUE = "---";
        struct StampAction
        {
            public string Text;
            public string Voice;
        }
        private static Regex TwitterRegex = new Regex("^https://twitter\\.com/(\\S+)");
        private static Dictionary<string, StampAction> Stamp = new Dictionary<string, StampAction>()
        {
            {"CAADBQADCgAD4impGGXQnVlzLgGkAg", new StampAction{Text="むー", Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/7115479/IOS/sticker_sound.m4a" } },
            {"CAADBQADAwAD4impGBRKcXYNv6a9Ag", new StampAction{Text="素人が扱えるものじゃない",Voice="https://stickershop.line-scdn.net/stickershop/v1/product/5033/IOS/main_sound.m4a"} },
            {"CAADBQADDgAD4impGODqgxESU1YDAg", new StampAction{Text="ありがとうございます", Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/7115478/IOS/sticker_sound.m4a"} },
            {"CAADBQADDQAD4impGPskcUw4H7xmAg", new StampAction{Text="話がころころ変わっていく",Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/7115482/IOS/sticker_sound.m4a"} },
            {"CAADBQADCwAD4impGAIxus96VFmvAg", new StampAction{Text="今日のところはこれくらいにしといてあげます",Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/7115480/IOS/sticker_sound.m4a"} },
            {"CAADBQADDAAD4impGD1jq6rFPZG-Ag", new StampAction{Text="なぜですか", Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/7115481/IOS/sticker_sound.m4a"} },
            {"CAADBQADCAAD4impGBqovJ00o17tAg", new StampAction{Text="おちついて", Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/7115477/IOS/sticker_sound.m4a"} },
            {"CAADBQAD4QoAAsGPESCAZmHn5KsAAVEC", new StampAction{Text="とっても疲れました", Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/30889677/IOS/sticker_sound.m4a"} },
            {"CAADBQAD4woAAsGPESA8OtWcGOdoPQI", new StampAction{Text="いいからさっさとしてください！", Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/30889679/IOS/sticker_sound.m4a" } },
            {"CAADBQAD8goAAsGPESD56VWnct6gPAI", new StampAction{Text="恥ずかしいです..//", Voice="https://stickershop.line-scdn.net/stickershop/v1/sticker/30889694/IOS/sticker_sound.m4a"} }
        };
        private static bool IsChatMode = false;

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient(Environment.GetEnvironmentVariable("TELEGLAM_TOKEN"));
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static string ChinoTalk(string message)
        {
            var ApiKey = Environment.GetEnvironmentVariable("USERLOCAL_API");
            var url = $"https://chatbot-api.userlocal.jp/api/chat?message={HttpUtility.UrlEncode(message)}&key={ApiKey}";
            var req = WebRequest.Create(url);
            var res = req.GetResponse();
            var s = res.GetResponseStream();
            dynamic json = DynamicJson.Parse(s);
            return json.result;
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
            {
                Console.WriteLine(e.Message.Text);
                if (IsChatMode)
                {
                    if (e.Message.Text.StartsWith("/stop"))
                    {
                        IsChatMode = false;
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "おやすみなさい");
                        return;
                    }
                    try
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, ChinoTalk(e.Message.Text));
                    }
                    catch
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "何かおかしいです・・");
                    }
                    return;
                }
                switch (e.Message.Text)
                {
                    case "/hello":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"おはようございます {e.Message.From.FirstName} {e.Message.From.LastName} さん");
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "今日も天気がいいですね");
                        break;
                    case "/omikuji":
                        var r = new Random();
                        int i1 = r.Next(10);
                        string Text = "中吉です。";
                        if (i1 == 1)
                        {
                            Text = "おめでとうございます。大吉です。";
                        }
                        else if (i1 == 2)
                        {
                            Text = "大凶です。元気出してお兄ちゃん。";
                        }
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, Text);
                        break;
                    case "/leave":
                        await Bot.LeaveChatAsync(e.Message.Chat.Id);
                        break;

                    case "/weather":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, GetWeatherText());
                        break;

                    case "/help":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "現在使用できるコマンドは\n/hello\n/leave\n/omikuji\n/wether\n/help\n/chatstart\n/an\n/ma\nです");
                        break;

                    case "/help@chino_talk_bot":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "現在使用できるコマンドは\n/hello\n/leave\n/omikuji\n/wether\n/help\n/chatstart\n/an\n/ma\nです");
                        break;

                    case "おはよう":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "おはようございます");
                        break;

                    case "おはようチノちゃん":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "おはようございます");
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "あまり名前で呼ばないでください...");
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "恥ずかしいです...//");
                        break;

                    case "チノちゃん":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "はい。なんでしょう");
                        break;

                    case "お兄ちゃんってよんで！":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "嫌です");
                        break;

                    case "おやすみ":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "おやすみなさい");
                        break;

                    case "おやすみチノちゃん":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "おやすみなさい。明日も早いですよ");
                        break;

                    case "もう寝るね":
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "今日はまだ...お話していたい...気分です...");
                        break;

                    case "/chatstart":
                        IsChatMode = true;
                        break;

                    case "/an":
                        var sentence = chinosentence.chino.matext();
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, sentence);
                        chinosentence.chino.test();
                        break;
                }


                if (e.Message.Text.StartsWith("/twiarchive"))
                {
                    var Match = TwitterRegex.Match(e.Message.Text);
                    if (Match != null)
                    {
                        var archiveurl = "https://web.archive.org/save/https://twitter.com/" + Match.Groups[1].Value;
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, archiveurl);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "正しく入力してください");
                    }
                }
                if (e.Message.Text.StartsWith("/ma"))
                {
                    try
                    {
                        string matext = e.Message.Text.Remove(0, 4);
                        var tagger = MeCabTagger.Create();

                        string result = tagger.Parse(matext);

                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, result);
                    }
                    catch
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id, "エラーです。");
                    }
                }
            }
            else if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.StickerMessage)
            {
                if (Stamp.ContainsKey(e.Message.Sticker.FileId))
                {
                    var Action = Stamp[e.Message.Sticker.FileId];
                    await Bot.SendVoiceAsync(e.Message.Chat.Id, new FileToSend(new Uri(Action.Voice)), Action.Text);
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
                    sbTempMax,
                    sbTempMin,
                    situation,
                    link,
                    title
                    );

            }
        }

        /*
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
        }*/
    }
}