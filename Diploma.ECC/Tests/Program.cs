using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Diploma.ECC.Math.Extensions;
using Diploma.ECC.Math.Utils;

namespace Diploma.ECC.Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			var curve = new Curve(CurveName.brainpoolp192r1);
			var keys = new KeyPair
			{
				PrivateKey = 123456789012345,
				PublicKey = curve.Parameters.G.Multiply(123456789012345)
			};
			
			var filename = "test.png";
			var hash = new BigInteger(Encoding.UTF8.GetBytes(ComputeFileHash(filename)));
			
			SignFile(filename, hash, curve, keys.PrivateKey);
			Console.WriteLine(VerifySignature(filename, hash, curve, keys.PublicKey));
		}

		private static void SignFile(string path, BigInteger hash, Curve curve, BigInteger privateKey)
		{
			// Sign file
			var k = BigIntGenerator.GenerateRandom(curve.Parameters.N);
			var rPoint = curve.Parameters.G.Multiply(k);
			var r = rPoint.X % curve.Parameters.N;
			var kInverse = k.ModuleInverse(curve.Parameters.N);
			var s = kInverse * (hash + r * privateKey) % curve.Parameters.N;
			
			// Save signature to file
			var signatureFilePath = $"{Path.GetFileName(path)}.sig";
			SaveSignature(signatureFilePath, r, s);
		}

		private static bool VerifySignature(string path, BigInteger hash, Curve curve, Point publicKey)
		{
			// Read signature
			var signatureFilePath = $"{Path.GetFileName(path)}.sig";
			if (!File.Exists(signatureFilePath))
			{
				Console.WriteLine("Подпись не обнаружена");
				return false;
			}

			using var streamReader = new StreamReader(signatureFilePath, Encoding.Default);
			var rString = streamReader.ReadLine() ?? throw new ArgumentException("rString");
			var sString = streamReader.ReadLine() ?? throw new ArgumentException("sString");

			var rBytes = Convert.FromBase64String(rString);
			var sBytes = Convert.FromBase64String(sString);
			
			var r = new BigInteger(rBytes);
			var s = new BigInteger(sBytes);
			

			// Verify
			var sInverse = s.ModuleInverse(curve.Parameters.N);
			var u = hash * sInverse % curve.Parameters.N;
			var v = r * sInverse % curve.Parameters.N;
			var cPoint = curve.Parameters.G.Multiply(u).Add(publicKey.Multiply(v));
			
			return cPoint.X == r;
		}
			
		private static string ComputeFileHash(string path)
		{
			var md5 = new MD5CryptoServiceProvider();
			
			using var fileStream = new FileStream(path, FileMode.Open);
			var fileData = new byte[fileStream.Length];
			fileStream.Read(fileData, 0, (int)fileStream.Length);
			
			var checkSum = md5.ComputeHash(fileData);
			return BitConverter.ToString(checkSum).Replace("-", string.Empty);
		}
		
		private static void SaveSignature(string path, BigInteger r, BigInteger s)
		{
			using var writer = new StreamWriter(path);

			var rString = Convert.ToBase64String(r.ToByteArray());
			var sString = Convert.ToBase64String(s.ToByteArray());

			writer.WriteLine(rString);
			writer.WriteLine(sString);
		}
	}
}
 