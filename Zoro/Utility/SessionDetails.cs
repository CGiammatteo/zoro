using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;

namespace Zoro.Utility
{
    internal class SessionDetails
    {
        public static void CheckCookie()
        {
            Misc.Output.Basic("Logging in via cookie...");
            WebClient client = new WebClient();
            client.Headers["Cookie"] = ".ROBLOSECURITY=" + Settings.Cookie;
            client.Proxy = WebProxy.GetDefaultProxy();
            client.Credentials = WebProxy.GetDefaultProxy().Credentials;

            try
            {
                if (client.DownloadString("http://www.roblox.com/mobileapi/userinfo").Contains("ThumbnailUrl"))
                {
                    string username = Misc.MiscUserData.GrabUsername(Settings.UserId);
                    Misc.Output.Success($"Logged in! Welcome, {username}!");
                    client.Dispose();
                }
                else
                {
                    Misc.Output.Error("Cookie is expired! Closing in 5 seconds...");
                    client.Dispose();
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
            }
            catch (WebException ex)
            {
                if ((int)ex.Status == 429 || (int)ex.Status == 401 || (int)ex.Status == 400)
                {
                    WebData.ProxyHelper.RotateProxy();
                    Misc.Output.Basic("Rotated proxy.");
                }

                client.Dispose();
                Misc.Output.Error("Unable to login! Closing in 5 seconds...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static string GrabRegData()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.roblox.com/My/Groups.aspx?gid=1");
            httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format(".ROBLOSECURITY={0}", Settings.Cookie));
            httpWebRequest.Proxy = Settings.LoadedProxy;
            httpWebRequest.Credentials = Settings.LoadedProxy.Credentials;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);

                var list = doc.DocumentNode.SelectNodes("/html/body/meta");

                foreach(var node in list)
                {
                    string htmlTag = node.OuterHtml.ToString();

                    htmlTag = htmlTag.Substring(36);
                    htmlTag = htmlTag.Substring(0, htmlTag.Length - 2);
                    return htmlTag;
                }
            }
            return "";
        }
    }
}
