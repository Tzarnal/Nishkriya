using System.Text;

namespace Nishkriya.Extensions
{
    public static class ByteExtensions
    {
        public static string ConvertToString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}