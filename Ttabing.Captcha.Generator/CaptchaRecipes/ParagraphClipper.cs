using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Ttabing.Captcha.Generator.Drawing;

namespace Ttabing.Captcha.Generator.CaptchaRecipes
{
    public class ParagraphClipper : ICaptchaGenerator
    {
        public ParagraphClipper()
        {

        }

        private SKData ToSkData(string captcha, int width, int height, Color color)
        {
            SKImageInfo inf = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            
            using (SKSurface surface = SKSurface.Create(inf))
            {
                SKCanvas canvas = surface.Canvas;
                // Your drawing code goes here.

                canvas.Clear(SKColors.White);

                using (var paint = new SKPaint())
                {
                    paint.TextSize = 10;
                    paint.Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                    paint.IsAntialias = true;
                    paint.IsStroke = false;

                    canvas.ClippingText(inf, captcha, paint);
                    canvas.Flush();
                }

                // set up drawing tools
                using (var paint = new SKPaint())
                {
                    paint.TextSize = 12;
                    paint.IsAntialias = true;
                    paint.Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Oblique);
                    paint.Color = new SKColor(color.R, color.G, color.B, color.A);
                    paint.IsStroke = false;

                    canvas.FillWithRandomText(inf, paint);
                    canvas.Flush();
                }

                return surface.Snapshot().Encode();
            }
        }

        public void ToJpgFile(string captcha, int width, int height, Color color)
        {
            using (var skData = ToSkData(captcha, width, height, color))
            {
                using (Stream stream2 = File.OpenWrite("tim.jpg"))
                {
                    skData.SaveTo(stream2);
                }
            }
        }

        public string ToPngBase64(string captcha, int width, int height, Color color)
        {
            using (var skData = ToSkData(captcha, width, height, color))
            {
                var dataArray = skData.ToArray();

                return "data:image/jpg;base64," + Convert.ToBase64String(dataArray);

            }
        }
    }
}
