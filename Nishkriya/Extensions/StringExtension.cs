using System;

namespace Nishkriya.Extensions
{
    public static class StringExtenions
    {
        public static byte[] ToByteArray(this string input)
        {
            var bytes = new byte[input.Length * sizeof(char)];
            Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}