using Ttabing.Captcha.Generator.CaptchaRecipes;

namespace Ttabing.Captcha.Generator
{
    public static class GeneratorFactory
    {
        private static readonly ICaptchaGenerator _generator = new ParagraphClipper();
        public static ICaptchaGenerator Create()
        {
            return _generator;
        }        
    }
}
