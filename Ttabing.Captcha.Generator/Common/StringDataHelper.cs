using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ttabing.Captcha.Generator.Common
{
    public static class StringDataHelper
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            return RandomString(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
        }

        public static string RandomString(int length, string chars)
        {

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
