// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("gw5KaIDXix/P65ZX+0dYISG1A9k+IC67GdN+5Y/hKeeGQcsHZzpamJrLN0962o917LnT/a4dQ7dlD91DPtmIddmo+yyAzKXy7ebIrORMt8neLc7LYpGSgMw00FqdwJJik5sG9UmTEOExxSmBdI605Ys4K4cExl8gMtsZpvyu8lS6Uyfs307CKoHIVtpy4szCTMefK1WgVX0KEHpJxBKmqb4Mj6y+g4iHpAjGCHmDj4+Pi46NFh34aprN18/xMkdyXP83STZaTZaL0ZFAPr3Ni+KjmpciRbyO4jNNmwyPgY6+DI+EjAyPj44lWiucaOX7P3UfCClQtMOJGgZLDUqeaLRc71MCzJ9u2JaD6/jDQu5Yy3wDym2+KTh1EmqFIvxiQ4yNj46P");
        private static int[] order = new int[] { 12,4,5,10,5,6,10,8,12,9,11,11,13,13,14 };
        private static int key = 142;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
