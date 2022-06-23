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

            try
            {
                if (client.DownloadString("http://www.roblox.com/mobileapi/userinfo").Contains("ThumbnailUrl"))
                {
                    string username = Misc.MiscUserData.GrabUsername(Settings.UserId);
                    Misc.Output.Success($"Logged in! Welcome, {username}!");
                }
            }
            catch (Exception ex)
            {
                Misc.Output.Error("Unable to login! Closing in 5 seconds...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static string GrabRegData()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://www.roblox.com/My/Groups.aspx?gid=1");
            webRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format(".ROBLOSECURITY={0}", Settings.Cookie));
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
