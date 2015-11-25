using System;
using System.Management.Automation;
using System.Net;
using HtmlAgilityPack;

namespace PowerShellCmdletSample
{
    [Cmdlet(VerbsCommon.Get, "BashRecord")]
    public class GetBashRecord : PSCmdlet
    {
        private const int Count = 436790;

        private readonly string BashUrl = @"http://bash.im/quote/";

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 1)]
        [ValidateRange(1, 436790)]
        public int Number { get; set; }

        protected override void ProcessRecord()
        {
            var random = new Random(DateTime.Now.Millisecond);
            int quoteNum = Number == 0 ? random.Next(Count) : Number;
            WriteObject(quoteNum);
            string downloadString = GetPageFromServer(BashUrl + quoteNum);
            string quote = GetQuote(downloadString);
            ShowQuote(quote);
        }

        private string Decode(string html)
        {
            return html.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
        }

        private string GetPageFromServer(string url)
        {
            var client = new WebClient();
            if (AppSettingsHelper.UseProxy())
            {
                IWebProxy proxy = new WebProxy(AppSettingsHelper.ProxyUrl());

                proxy.Credentials = new NetworkCredential(AppSettingsHelper.ProxyUser(), AppSettingsHelper.ProxyPassword(), AppSettingsHelper.ProxyDomain());
                client.Proxy = proxy;
            }

            string downloadString = client.DownloadString(url);
            return downloadString;
        }

        private string GetQuote(string webPageString)
        {
            var document = new HtmlDocument();
            document.LoadHtml(webPageString);

            HtmlNode node = document.DocumentNode.SelectSingleNode(".//*[@class='quote']/*[@class='text']");
            return Decode(node.InnerText);
        }

        private void ShowQuote(string quote)
        {
            WriteObject(Environment.NewLine);
            WriteObject(quote);
            WriteObject(Environment.NewLine);
        }
    }
}
