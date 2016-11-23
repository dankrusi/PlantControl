using System;
using System.Text;
using System.Security.Cryptography;

namespace PlantControl.Util
{
	public class Hash
	{
		public static string GetHashedString(string stringToHash) {
			byte[] hashstringBytes = new UTF8Encoding().GetBytes(stringToHash);
			byte[] hashBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(hashstringBytes);
			return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
		}
	}
}
