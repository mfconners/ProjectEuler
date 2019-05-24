using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem059 : Problem
	{
		// Slow: 2 minutes
		public override string CorrectAnswer { get { return "107359"; } }

		private static readonly char[] separators = { ',', '\r', '\n' };

		protected override string CalculateSolution()
		{
			List<char> characters = new List<char>();

			string[] cipher_file = Properties.Resources.cipher.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			#region Read in the file of "encrypted" chars.
			foreach (string numstring in cipher_file)
			{
				characters.Add((char)Convert.ToInt16(numstring));
			}
			#endregion

			#region "Decrypt" the message with the password...
			// TODO Garbage Collection: Allocating strings at this rate is causing a lot of issues...
			Dictionary<string, string> passwords = new Dictionary<string, string>();
			for (string password = "a";
					password[0] <= 'z';
					password = Convert.ToString((char)(password[0] + 1)))
			{
				for (password = password + 'a';
						password[1] <= 'z';
						password = password.Substring(0, 1) + (char)(password[1] + 1))
				{
					for (password = password + 'a';
							password[2] <= 'z';
							password = password.Substring(0, 2) + (char)(password[2] + 1))
					{
						string decrypted = string.Empty;
						for (int i = 0; i < characters.Count; ++i)
						{
							decrypted = decrypted + (char)(characters[i] ^ password[i % password.Length]);
						}

						if (decrypted.Replace("the", string.Empty).Replace("The", string.Empty) != decrypted &&
								decrypted.Replace("that", string.Empty).Replace("That", string.Empty) != decrypted)
						{
							passwords.Add(password, decrypted);
						}
					}
				}
			}
			#endregion


			string phrase = passwords.Values.First();
			if (passwords.Count > 1)
			{
				// Add some qualifiers above!
				phrase = passwords.Values.First();
			}

			int sum = 0;
			foreach (char c in phrase)
			{
				sum += c;
			}

			return sum.ToString();
		}
	}
}
