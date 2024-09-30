using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Utils
{
    public static class Slug
    {
        public static string GenerateSlug(this string slug, string phrase, int Id)
        {
            if (phrase is null)
            {
                return null;
            }
            string str = phrase.ToLowerInvariant();
            // invalid chars
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens

            var idHash = Hash.GetHash(Id.ToString());

            return str + "-" + idHash;
        }
    }
}
