namespace HeiaMeg.Utils
{
    public class IdentityUtils
    {
        /// <summary>
        /// Using a Cantor pairing function to create a unique id from themeId and messageId
        /// </summary>
        /// <returns>Unique natural number representing a notification within a theme and message</returns>
        public static int CantorPair(int a, int b) => (a + b) * (a + b + 1) / 2 + 1;
    }
}
