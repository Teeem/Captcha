using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Ttabing.Captcha.Generator.CaptchaRecipes;

namespace Ttabing.Captcha.Generator
{
    internal class TimeSensitiveCaptcha : ICaptcha
    {
        public const string HashingFormat = "{0}__{1}++{2}";
        private string _salt;
        private byte[] _computedHash;
        private readonly DateTime _deadline;
        public TimeSensitiveCaptcha(string b64, string salt, byte[] hashedCaptcha, TimeSpan span)
        {
            _deadline = DateTime.UtcNow.Add(span);
            _salt = salt;
            _computedHash = hashedCaptcha;
            Base64Image = b64;
        }
        public string Base64Image { get; }

        public bool Validate(string trial)
        {
            if(_deadline < DateTime.UtcNow)
            {
                return false;
            }
            string formated = string.Format(DefaultCaptcha.HashingFormat, Base64Image, trial.ToLower(), _salt);
            var sha512 = SHA512.Create();
            var compare = sha512.ComputeHash(Encoding.ASCII.GetBytes(formated));

            return compare.SequenceEqual(_computedHash);
        }
    }

    internal class DefaultCaptcha : ICaptcha
    {
        public const string  HashingFormat = "{0}__{1}++{2}";
        private string _salt;

        private byte[] _computedHash;
        public DefaultCaptcha(string b64, string salt, byte [] hashedCaptcha)
        {
            _salt = salt;
            _computedHash = hashedCaptcha;
            Base64Image = b64;
        }
        public string Base64Image { get; }

        public bool Validate(string trial)
        {
            string formated = string.Format(DefaultCaptcha.HashingFormat, Base64Image, trial.ToLower(), _salt);
            var sha512 = SHA512.Create();
            var compare = sha512.ComputeHash(Encoding.ASCII.GetBytes(formated));

            return compare.SequenceEqual(_computedHash);
        }
    }
    public interface ICaptcha
    {
        bool Validate(string trial);
        string Base64Image { get; }
    }
    public class CaptchaValidator
    {
        private static readonly ICaptchaGenerator _generator = GeneratorFactory.Create();


        public ICaptcha Generate(int numberOfCharacters, Color color)
        {
            string c = Common.StringDataHelper.RandomString(numberOfCharacters);
            string b64 = _generator.ToPngBase64(c, 150, 100, color);
            string salt = Common.StringDataHelper.RandomString(10) + color.ToArgb().ToString();
            string formated = string.Format(DefaultCaptcha.HashingFormat, b64, c.ToLower(), salt);
            var sha512 = SHA512.Create();

            return  new DefaultCaptcha(b64, salt, sha512.ComputeHash(Encoding.ASCII.GetBytes(formated)));
        }

        public ICaptcha Generate(int numberOfCharacters, Color color, TimeSpan span)
        {
            string c = Common.StringDataHelper.RandomString(numberOfCharacters);
            string b64 = _generator.ToPngBase64(c, 150, 100, color);
            string salt = Common.StringDataHelper.RandomString(10) + color.ToArgb().ToString();
            string formated = string.Format(DefaultCaptcha.HashingFormat, b64, c.ToLower(), salt);
            var sha512 = SHA512.Create();

            return new TimeSensitiveCaptcha(b64, salt, sha512.ComputeHash(Encoding.ASCII.GetBytes(formated)), span);
        }
    }
}
