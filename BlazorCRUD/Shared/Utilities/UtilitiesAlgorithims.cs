using System.Text.RegularExpressions;

namespace BlazorCRUD.Shared
{
	public static class UtilitiesAlgorithims
	{
		public static TNullable? NullUncoalesce<TNullable>(this TNullable? nullable, TNullable uncoalesce) =>
			nullable is null ? nullable : uncoalesce;
		public static string? NullOrEmptyUncoalesce(this string? nullable, string uncoalesce) =>
			string.IsNullOrEmpty(nullable) ? nullable : uncoalesce;

		public static T MinMax<T>(this T value, T firstBoundary, T secondBoundary) where T : IComparable<T>
		{
			(T min, T max) = firstBoundary.CompareTo(secondBoundary) < 0 ?
				(firstBoundary, secondBoundary) : (secondBoundary, firstBoundary);

			return (value.CompareTo(min) < 0) ?
				min : (value.CompareTo(max) > 0) ?
					max : value;
		}

		public static string RegexReplace(this string src, string pattern, string replacement) =>
			Regex.Replace(src, pattern, replacement);

		public static string FormatCamelCase(this string camelCaseSrc) =>
			camelCaseSrc
				.RegexReplace("^_", "")
				.RegexReplace("([a-z])([A-Z])", "$1 $2")
				.RegexReplace("([A-Z])([A-Z][a-z])", "$1 $2").Trim();

		public static string ToFormatedString(this Enum? enumIstance) =>
			enumIstance?.ToString().FormatCamelCase() ?? "NULL";

		public static IEnumerable<T> GetValues<T>() where T : Enum => (T[])Enum.GetValues(typeof(T));

	}
}
