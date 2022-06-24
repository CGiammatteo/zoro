using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Zoro.Utility
{
    internal class SessionDetails
    {
        public static void CheckCookie()
        {
            Misc.Output.Basic("Logging in via cookie...");
            WebClient client = new WebClient();
            client.Headers["Cookie"] = ".ROBLOSECURITY=" + Settings.Cookie;
            client.Proxy = Settings.LoadedProxy;
            client.Credentials = Settings.LoadedProxy.Credentials;

            try
            {
                if (client.DownloadString("http://www.roblox.com/mobileapi/userinfo").Contains("ThumbnailUrl"))
                {
                    string username = Misc.MiscUserData.GrabUsername(Settings.UserId);
                    Misc.Output.Success($"Logged in! Welcome, {username}!");
                    client.Dispose();
                }
            }
            catch (WebException ex)
            {
                if ((int)ex.Status == 429 || (int)ex.Status == 401)
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
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://www.roblox.com/My/Groups.aspx?gid=1");
            webRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format(".ROBLOSECURITY={0}", Settings.Cookie));
            webRequest.Proxy = Settings.LoadedProxy;
            webRequest.Credentials = Settings.LoadedProxy.Credentials;
            using (HttpWebResponse _response = (HttpWebResponse)webRequest.GetResponse())
            using (Stream _stream = _response.GetResponseStream())
            using (StreamReader sR = new StreamReader(_stream))
            {
                Regex regex = new Regex(@"Roblox\.XsrfToken.*?\'(.*)\'");
                Match matched = regex.Match(sR.ReadToEnd());

                if (matched.Success)
                {
                    return matched.Groups[1].Value;
                }

                return "Token Not Found!";
            }
        }
    }
}
