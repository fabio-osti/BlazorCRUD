using System.Security.Cryptography;
using System.Text;

namespace BlazorCRUD.Shared
{
	static public class PassHelper
	{
		public static string SaltAndHash(string subject, string salt)
		{
			var salted = Encoding.Default.GetBytes(subject + salt);
			return Encoding.Default.GetString(SHA256.Create().ComputeHash(salted));
		}

		private const string availableChars =
			"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!#$%&*+-/:;?@_";
		public static string GenSalt(int size)
		{
			var saltBytes = new byte[size];
			using (var random = RandomNumberGenerator.Create()) { random.GetNonZeroBytes(saltBytes); }
			string salt = new("");
			foreach (var i in saltBytes) salt += availableChars[i % availableChars.Length];
			return salt;
		}
	}
}
