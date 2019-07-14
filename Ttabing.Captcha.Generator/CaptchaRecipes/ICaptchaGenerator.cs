using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ttabing.Captcha.Generator.CaptchaRecipes
{
    public interface ICaptchaGenerator 
    {
        void ToJpgFile(string captcha, int width, int height, Color color);
        string ToPngBase64(string captcha, int width, int height, Color color);
    }
}
