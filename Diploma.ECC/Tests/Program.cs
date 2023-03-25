using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Diploma.ECC.Math.Extensions;
using Diploma.ECC.Math.Utils;

namespace Diploma.ECC.Tests
{
	class Program
	{
		[SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
		static void Main(string[] args)
		{
			// var message = "Тестовое сообщение";
			// var messageBytes = Encoding.UTF8.GetBytes(message);
			//
			// var curve = new Curve(CurveName.secp256r1);
			//
			// var keyGen = new KeysGenerator();
			//
			// var sender = keyGen.GetKeyPair(curve);
			// var receiver  = keyGen.GetKeyPair(curve);
			//
			// Point aliceSharedSecret = keyGen.GetSharedSecret(sender.PrivateKey, receiver.PublicKey);
			// Point bobSharedSecret = keyGen.GetSharedSecret(receiver.PrivateKey, sender.PublicKey);
			//
			// // Encryption
			// AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
			//
			// Rfc2898DeriveBytes aliceCryptoKey = new Rfc2898DeriveBytes(aliceSharedSecret.ToString(), new byte[] {0, 0, 0, 0, 0, 0, 0, 0}, 1000, HashAlgorithmName.SHA256);
			//
			// aes.Key = aliceCryptoKey.GetBytes(32);
			// aes.Mode = CipherMode.ECB; 
			//
			// var encryptor = aes.CreateEncryptor(aes.Key, null);
			// var encryptedMessage = encryptor.TransformFinalBlock(messageBytes, 0, messageBytes.Length);
			//
			// Console.WriteLine(Convert.ToBase64String(encryptedMessage));
			//
			// // Decryption
			// Rfc2898DeriveBytes bobCryptoKey = new Rfc2898DeriveBytes(bobSharedSecret.ToString(), new byte[] {0, 0, 0, 0, 0, 0, 0, 0}, 1000, HashAlgorithmName.SHA256);
			//
			// aes.Key = bobCryptoKey.GetBytes(32);
			// aes.Mode = CipherMode.ECB; 
			//
			// var decryptor = aes.CreateDecryptor(aes.Key, null);
			// var decryptedMessage = decryptor.TransformFinalBlock(encryptedMessage, 0, encryptedMessage.Length);
			//
			// Console.WriteLine(Encoding.UTF8.GetString(decryptedMessage));


			var str = "lalka";
			var message = new BigInteger(Encoding.UTF8.GetBytes(str));

			var curve = new Curve(CurveName.nistp192);
			var keys = new KeyPair
			{
				PrivateKey = 123456789012345,
				PublicKey = curve.Parameters.G.Multiply(123456789012345)
			};

			Console.WriteLine("Get signature");
			var k = BigIntGenerator.GenerateRandom(curve.Parameters.N);

			var r_point = curve.Parameters.G.Multiply(k);
			
			var r = r_point.X % curve.Parameters.N;
			
			var k_inverse = k.ModuleInverse(curve.Parameters.N);
			
			var s = k_inverse * (message + r * keys.PrivateKey) % curve.Parameters.N;
			Console.WriteLine($"R = {r}\nS = {s}");
			
			Console.WriteLine("\n\n\n=================================");
			Console.WriteLine("Verify");
			var s_inverse = s.ModuleInverse(curve.Parameters.N);
			
			var u = message * s_inverse % curve.Parameters.N;
			
			var v = r * s_inverse % curve.Parameters.N;
			
			var c_point = curve.Parameters.G.Multiply(u).Add(keys.PublicKey.Multiply(v));

			Console.WriteLine(r);
			var valid = c_point.X == r;
			Console.WriteLine($"{c_point.X} ?= {r}");
			Console.WriteLine($"Is Valid = {valid}");
		}
	}
}