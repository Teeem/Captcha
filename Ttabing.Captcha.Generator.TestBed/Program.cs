using System;
using System.Drawing;
using Ttabing.Captcha.Generator.Common;

namespace Ttabing.Captcha.Generator.TestBed
{
    class Program
    {
        static void Main(string[] args)
        {
            CaptchaValidator cv = new CaptchaValidator();
            var cap = cv.Generate(5, System.Drawing.Color.DarkOrange, TimeSpan.FromHours(1));

            string b64 = cap.Base64Image;
            string password = "asdf";

            bool isValid = cap.Validate(password);

        }
    }
}
