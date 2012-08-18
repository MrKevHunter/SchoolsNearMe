using System;
using System.Web.Mvc;

namespace SchoolMap.Net.Extensions
{
    public static class HtmlExtensions
    {

        public static string SimpleLink(this HtmlHelper html, string url, string text)
        {
            return String.Format("<a href=\"{0}\">{1}</a>", url, text);
        }

    }
}