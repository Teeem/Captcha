using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Ttabing.Captcha.Generator.Common;

namespace Ttabing.Captcha.Generator.Drawing
{
    internal static class SKSurfaceExtension
    {

        public static SKCanvas ClippingText(this SKCanvas canvas, SKImageInfo info, string text, SKPaint paint)
        {
            using (SKPath textPath = paint.GetTextPath(text, 0, 0))
            {
                // Set transform to center and enlarge clip path to window height
                SKRect bounds;
                textPath.GetTightBounds(out bounds);
                textPath.FillType = SKPathFillType.InverseWinding;
                canvas.Translate(info.Width / 2, info.Height / 2);
                canvas.Scale((info.Width / bounds.Width) * 0.9f, (info.Height / bounds.Height) * 0.9f);
                canvas.Translate(-bounds.MidX, -bounds.MidY);

                // Set the clip path
                canvas.ClipPath(textPath);
            }

            // Reset transforms
            canvas.ResetMatrix();
            return canvas;
        }
        public static SKCanvas FillWithRandomText(this SKCanvas canvas, SKImageInfo info, SKPaint paint)
        {
            int numberOfLines = (int)Math.Ceiling((decimal)(info.Height / paint.TextSize));

            int numberOfCharacters = 12;
            string text;

            do
            {
                numberOfCharacters *= 2;
                text = StringDataHelper.RandomString(numberOfCharacters);
            } while (paint.MeasureText(text) < (1.25 * info.Width));

            for (int i = 0; i < numberOfLines; ++i)
            {
                canvas.DrawText(StringDataHelper.RandomString(numberOfCharacters), 0f, (float)((i + 1) * paint.TextSize), paint);
            }

            return canvas;

        }
    }
}
