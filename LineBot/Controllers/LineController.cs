using LineMessagingAPISDK.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LineBot.Controllers
{
	public class LineController : ApiController
	{

		public async Task<HttpResponseMessage> Post()
		{

			var settings = new Properties.Settings();


			var contentString = await Request.Content.ReadAsStringAsync();
			dynamic contentObj = JsonConvert.DeserializeObject(contentString);
			var result = contentObj.events[0];

			//BaseEvents events = new BaseEvents(result);

			var client = new HttpClient();
			try
			{
				client.DefaultRequestHeaders
					.TryAddWithoutValidation("Content-Type", "application/json; charset=UTF-8");
				client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", settings.Authorization);
				//LINEから受信したイベント検証
				//メッセージイベント検証
				if (result.type == nameof(EventType.Message).ToLower())
				{
					//メッセージタイプがテキストならLUISに接続し、結果を返信する
					if (result.message.type == nameof(MessageType.Text).ToLower())
					{
						var clients = new HttpClient();

						// ↓この行で落ちる
						var queryString = HttpUtility.ParseQueryString(result.message.text);

						// Request headers
						client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", settings.SubscriptionKey);

						//↓このURLの組み立ては意味不明
						var uri = "https://westus.api.cognitive.microsoft.com/luis/v1.0/prog/apps/?subscription-key=" + settings.SubscriptionKey + "&q=" + queryString;

						var response = await client.GetAsync(uri);
						var res = JsonConvert.DeserializeObject(response);
						//TextMessage replyMessage = new TextMessage(result.message.text);
						var Response = await client.PostAsJsonAsync("https://api.line.me/v2/bot/message/reply",
						new
						{
							replyToken = result.replyToken,
							messages = new[] { new { type = "text", text = "受信メッセージは\"" + result.message.text + "\"ですね" } }
						});
						System.Diagnostics.Debug.WriteLine(await Response.Content.ReadAsStringAsync());
					}
					//メッセージタイプが画像なら####に接続し、結果を返信する
					if (result.message.type == nameof(MessageType.Image).ToLower())
					{

						var Response = await client.PostAsJsonAsync("https://api.line.me/v2/bot/message/reply",
						new
						{
							replyToken = result.replyToken,
							messages = new[] { new { type = "text", text = "画像を受信しました。" } }
						});
						System.Diagnostics.Debug.WriteLine(await Response.Content.ReadAsStringAsync());
					}

				}

				else if (result.message.type == nameof(MessageType.Video).ToLower())
				{
					var Response = await client.PostAsJsonAsync("https://api.line.me/v2/bot/message/reply",
					new
					{
						replyToken = result.replyToken,
						messages = new[] { new { type = "text", text = "動画を受信しました。" } }
					});
					System.Diagnostics.Debug.WriteLine(await Response.Content.ReadAsStringAsync());
				}
				return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e);
				return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
			}
		}


		/*
            public async Task<HttpResponseMessage> Post()
            {
                var contentString = await Request.Content.ReadAsStringAsync();

                Debug.WriteLine(contentString);

                dynamic contentObj = JsonConvert.DeserializeObject(contentString);
                var events = contentObj.events[0];

                var client = new HttpClient();
                try
                {
                    client.DefaultRequestHeaders
                      .Add("X-Line-ChannelID", "1496900892");
                    client.DefaultRequestHeaders
                      .Add("X-Line-ChannelSecret", "11a8198e225522f93c41aac78be95f7e");
                    client.DefaultRequestHeaders
                      .Add("X-Line-Trusted-User-With-ACL", "payjZQCNhfBreBxIpqXhao0dfh5Htg/aHbky3W7pp10NedytEPeLuwxeMnjJK3rJvbloP1Orxl7vLlPcsuqQBa8ZATqcvw4ml5LwWuEEnQ78sTSpozq959ua7oAAWyTN15fGialEMdT0619gu4NIZQdB04t89/1O/w1cDnyilFU=");


                    var res = await client.PostAsJsonAsync("https://trialbot-api.line.me/v1/events",
                        new
                        {
                            to = new[] { events.userId.Value },
                            toChannel = "1383378250",
                            eventType = "138311608800106203",
                            content = new
                            {
                                contentType = 1,
                                toType = 1,
                                text = $"「{events.message.text.Value}」と言ったか にゃ？"
                            }
                        });

                    System.Diagnostics.Debug.WriteLine(await res.Content.ReadAsStringAsync());
                    return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                }
            }

            public async Task<HttpResponseMessage> Get(int id)
            {
                switch (id)
                {
                    case 1:
                        Trace.TraceInformation("Information");
                        break;
                    case 2:
                        Trace.TraceWarning("Warning");
                        break;
                    case 3:
                        Trace.TraceError("Error");
                        break;
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

    */
	}
}
