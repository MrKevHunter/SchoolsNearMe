using System;
using System.Web.Mvc;

namespace SchoolsNearMe.Extensions
{
    public static class HtmlExtensions
    {
        public static string SimpleLink(this HtmlHelper html, string url, string text)
        {
            return String.Format("<a target=\"_blank\" href=\"{0}\">{1}</a>", url, text);
        }
    }
}