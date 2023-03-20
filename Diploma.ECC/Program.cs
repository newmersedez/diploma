using System;
using System.Security.Cryptography;
using System.Text;
using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;

namespace Diploma.ECC
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = "Тестовое сообщение";
            var messageBytes = Encoding.UTF8.GetBytes(message);
			
            var curve = new Curve(CurveName.secp256r1);

            var keyGen = new KeysGenerator();
            
            var sender = keyGen.GetKeyPair(curve);
            var receiver  = keyGen.GetKeyPair(curve);
			
            Point aliceSharedSecret = keyGen.GetSharedSecret(sender.PrivateKey, receiver.PublicKey);
            Point bobSharedSecret = keyGen.GetSharedSecret(receiver.PrivateKey, sender.PublicKey);
			
            // Encryption
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
			
            Rfc2898DeriveBytes aliceCryptoKey = new Rfc2898DeriveBytes(aliceSharedSecret.ToString(), new byte[] {0, 0, 0, 0, 0, 0, 0, 0}, 1000, HashAlgorithmName.SHA256);
			
            aes.Key = aliceCryptoKey.GetBytes(32);
            aes.Mode = CipherMode.ECB; 
			
            var encryptor = aes.CreateEncryptor(aes.Key, null);
            var encryptedMessage = encryptor.TransformFinalBlock(messageBytes, 0, messageBytes.Length);
			
            Console.WriteLine(Convert.ToBase64String(encryptedMessage));
			
            // Decryption
            Rfc2898DeriveBytes bobCryptoKey = new Rfc2898DeriveBytes(bobSharedSecret.ToString(), new byte[] {0, 0, 0, 0, 0, 0, 0, 0}, 1000, HashAlgorithmName.SHA256);
			
            aes.Key = bobCryptoKey.GetBytes(32);
            aes.Mode = CipherMode.ECB; 
			
            var decryptor = aes.CreateDecryptor(aes.Key, null);
            var decryptedMessage = decryptor.TransformFinalBlock(encryptedMessage, 0, encryptedMessage.Length);
			
            Console.WriteLine(Encoding.UTF8.GetString(decryptedMessage));
        }
    }
}