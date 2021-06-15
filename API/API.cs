using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using MirumTest.Models;
using Newtonsoft.Json.Linq;

namespace MirumTest.API
{

    public class API
    {
        //Parsing recived JSON and sending it to choosen controler 
        public List<RecentTwitter> SerchTenTweets(string word)
        {

            var t = Task.Run(() =>
               GetAPI(("2/tweets/search/recent?expansions=author_id&tweet.fields=created_at,entities&query=" + word)));
            t.Wait();
            //
            var l = 0;
            try
            {
                var j = JObject.Parse(t.Result);
                List<RecentTwitter> RecTwit = new List<RecentTwitter>();
                var data = j["data"];

                if (data != null) //If Json isn't correct there isn't table named "data" so if there are any errors in received JSON page will display error
                {
                    //Looking for "text" and "created_at" fields in each tweet and passing into model 
                    foreach (var item in data)
                    {
                        RecTwit.Add(new RecentTwitter
                        {
                            Text = item.Value<string>("text"),
                            Created_at = DateTime.ParseExact(item.Value<string>("created_at"), "MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture).ToLocalTime(),

                        }
                        );
                    }


                    //Loop for looking for "start" and "end" points of ilnks in each tweets, not finished and not working properly

                    //foreach (var item in j["data"].Children()["entities"]["urls"].Children())
                    //{
                    //    Console.WriteLine(item);
                    //    if (item.Count() == 0) { Console.WriteLine(item.Count()); continue; }
                    //    for (int i = 0; i < item.Count(); i++)
                    //    {
                    //        RecTwit[i].Urls = new List<entities>
                    //            {
                    //                new entities
                    //                {
                    //                    Start = item.Value<string>("start"),
                    //                    End = item.Value<string>("end"),
                    //                    Expanded_url = item.Value<string>("expanded_url")
                    //                }

                    //            };
                    //    }

                    //}
                    // Looking for "username" field in each tweet and passing into model
                    foreach (var item in j["includes"]["users"])
                    {
                        RecTwit[l].Username = item.Value<string>("username");
                        l++;

                    }

                    return RecTwit;
                }




            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return null;
        }

        public List<MirumTweets> SearchMirumTweets()
        {

            var t = Task.Run(() =>
               GetAPI(("2/users/2828590159/tweets?expansions=author_id&tweet.fields=author_id,created_at")));
            t.Wait();
            try
            {
                var j = JObject.Parse(t.Result);
                List<MirumTweets> MirTweets = new List<MirumTweets>();
                var data = j["data"];

                if (data != null) //If Json isn't correct there isn't table named "data" so if there are any errors in received JSON page will display error
                {
                    //Looking for "text" and "created_at" fields in each tweet and passing into model 
                    foreach (var item in data)
                    {
                        string text = item.Value<string>("text");
                        MirTweets.Add(new MirumTweets
                        {
                            Text = text.Truncate(140),
                            Created_at = DateTime.ParseExact(item.Value<string>("created_at"), "MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture).ToLocalTime(),

                        }
                        );
                    }

                    return MirTweets;
                }




            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return null;
        }

        //Method to connect to API 
        public async Task<string> GetAPI(string apiPath)
        {

            var response = string.Empty;
            using (var client = new HttpClient())
            {

                var baseUri = new Uri("https://api.twitter.com/");
                var encodedConsumerKey = HttpUtility.UrlEncode("O5i2H5GC0v3dhYAHe3alljlt2");
                var encodedConsumerKeySecret = HttpUtility.UrlEncode("YakcQ4QbHSqS3mZ6NWmWvdE5ERYm2J7njrvwLSIU6BqcnP6Slq");
                var encodedPair = Base64Encode(String.Format("{0}:{1}", encodedConsumerKey, encodedConsumerKeySecret));

                var requestToken = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(baseUri, "oauth2/token"),
                    Content = new StringContent("grant_type=client_credentials")
                };

                requestToken.Content.Headers.ContentType =
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
                requestToken.Headers.TryAddWithoutValidation("Authorization", String.Format("Basic {0}", encodedPair));

                var bearerResult = await client.SendAsync(requestToken);
                var bearerData = await bearerResult.Content.ReadAsStringAsync();
                var bearerToken = JObject.Parse(bearerData)["access_token"].ToString();

                var requestData = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(baseUri, apiPath),
                };
                requestData.Headers.TryAddWithoutValidation("Authorization", String.Format("Bearer {0}", bearerToken));

                HttpResponseMessage results = await client.SendAsync(requestData);
                if (results.IsSuccessStatusCode)
                {
                    response = await results.Content.ReadAsStringAsync();

                }
                else
                {
                    response = "nie działa";
                }

            }
            return response;

        }
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

    }
}
