namespace BlazorCRUD.Shared
{
	static public class ConvertExtensions
	{
		public static sbyte ToSByte<T>(this T src) => Convert.ToSByte(src);
		public static byte ToByte<T>(this T src) => Convert.ToByte(src);
		public static short ToInt16<T>(this T src) => Convert.ToInt16(src);
		public static ushort ToUInt16<T>(this T src) => Convert.ToUInt16(src);
		public static int ToInt32<T>(this T src) => Convert.ToInt32(src);
		public static uint ToUInt32<T>(this T src) => Convert.ToUInt32(src);
		public static long ToInt64<T>(this T src) => Convert.ToInt64(src);
		public static ulong ToUInt64<T>(this T src) => Convert.ToUInt64(src);
		public static float ToSingle<T>(this T src) => Convert.ToSingle(src);
		public static double ToDouble<T>(this T src) => Convert.ToDouble(src);
		public static decimal ToDecimal<T>(this T src) => Convert.ToDecimal(src);
	}
}
