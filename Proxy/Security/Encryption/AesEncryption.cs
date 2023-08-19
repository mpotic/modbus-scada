using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Proxy.Security
{
	public class AesEncryption : IEncryption
	{
		private byte[] key;

		private byte[] iv;

		private readonly string filepath = "Resources/Security/AesKeyAndIV.json";

		public AesEncryption()
		{
			GenerateAndSaveOrReadKeyAndIV(filepath);
		}

		private void GenerateAndSaveOrReadKeyAndIV(string keyAndIVFilePath)
		{
			bool isFileAccessedRecently = File.Exists(filepath) &&
				(DateTime.Now - File.GetLastAccessTime(filepath)) < TimeSpan.FromMinutes(5);

			if (isFileAccessedRecently)
			{
				string jsonContent = File.ReadAllText(filepath);
				JObject keyValuePair = JObject.Parse(jsonContent);

				key = Convert.FromBase64String(keyValuePair.GetValue("key").Value<string>());
				iv = Convert.FromBase64String(keyValuePair.GetValue("iv").Value<string>());

				return;
			}

			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.GenerateKey();
				key = aesAlg.Key;

				aesAlg.GenerateIV();
				iv = aesAlg.IV;

				JObject json = new JObject();
				json["key"] = Convert.ToBase64String(key);
				json["iv"] = Convert.ToBase64String(iv);

				File.WriteAllText(keyAndIVFilePath, json.ToString());
			}
		}

		public byte[] Encrypt(byte[] plaintext)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = key;
				aesAlg.IV = iv;
				aesAlg.Padding = PaddingMode.PKCS7;

				using (MemoryStream memoryStream = new MemoryStream())
				{
					ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
					{
						cryptoStream.Write(plaintext, 0, plaintext.Length);
					}

					byte[] encryptedData = memoryStream.ToArray();
					byte[] header = BitConverter.GetBytes(encryptedData.Length);
					encryptedData = header.Concat(encryptedData).ToArray();

					return encryptedData;
				}
			}
		}

		public byte[] Decrypt(byte[] cipherdata)
		{
			int encryptionLength = BitConverter.ToInt32(cipherdata, 0);
			byte[] ciphertext = new byte[encryptionLength];
			Array.Copy(cipherdata, 4, ciphertext, 0, encryptionLength);

			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = key;
				aesAlg.IV = iv;
				aesAlg.Padding = PaddingMode.PKCS7;

				using (MemoryStream memoryStream = new MemoryStream())
				{
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
					{
						cryptoStream.Write(ciphertext, 0, encryptionLength);
					}

					byte[] decryptedData = memoryStream.ToArray();

					return decryptedData;
				}
			}
		}

		~AesEncryption()
		{
			if (File.Exists(filepath))
			{
				File.Delete(filepath);
			}
		}
	}
}
