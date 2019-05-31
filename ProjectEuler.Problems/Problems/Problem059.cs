using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem059 : Problem
	{
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
			List<char> password = new List<char>();
			List<char> decrypted = new List<char>();

			for (password.Add('a'); password[0] <= 'z'; ++password[0])
			{
				password.RemoveRange(1, password.Count - 1);
				for (password.Add('a'); password[1] <= 'z'; ++password[1])
				{
					password.RemoveRange(2, password.Count - 2);
					for (password.Add('a'); password[2] <= 'z'; ++password[2])
					{
						decrypted.Clear();
						for (int i = 0; i < characters.Count; ++i)
						{
							decrypted.Add((char)(characters[i] ^ password[i % password.Count]));
						}

						bool has_the = false, has_that = false;
						for (int j = 0; j + 2 < decrypted.Count && (!has_the || !has_that); ++j)
						{
							if (decrypted[j] == 't' || decrypted[j] == 'T')
							{
								if (decrypted[j + 1] == 'h')
								{
									has_the = has_the || decrypted[j + 2] == 'e';
									has_that = has_that || (j + 3 < decrypted.Count && decrypted[j + 2] == 'a' && decrypted[j + 3] == 't');
								}
							}
						}

						if (has_the && has_that)
						{
							StringBuilder new_password = new StringBuilder();
							for (int k = 0; k < password.Count; ++k)
							{
								new_password.Append(password[k]);
							}

							StringBuilder new_decrypted = new StringBuilder();
							for (int k = 0; k < decrypted.Count; ++k)
							{
								new_decrypted.Append(decrypted[k]);
							}

							passwords.Add(new_password.ToString(), new_decrypted.ToString());
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
