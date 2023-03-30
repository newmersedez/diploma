using System;
using System.Numerics;
using System.Security.Cryptography;
using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Diploma.ECC.Math.Extensions;
using Diploma.Server.Services.Crypto;

namespace Diploma.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var curve = new Curve(CurveName.secp256r1);
            
            var userPrivateKey = BigInteger.Parse("108846403386794301771416625187350643957588719660200337584406334091787837730955");
            var userPublicKey = new Point(
                BigInteger.Parse("103582556435598853832090408061883780756324489371152261339215917722469422444082"),
                BigInteger.Parse("6978657195598502095342936082043194918325747688188440452381192713826162800550"),
                curve);
            
            var serverPrivateKey = BigInteger.Parse("1678499242152935872409212988779958353660132444820163232286035525403751941287");
            var serverPublicKey = new Point(
                BigInteger.Parse("28298176243003502145659282453608187188578391890158442826988999383266809556234"),
                BigInteger.Parse("56881022602330665782889180903201611797830513781992792090414332153064916328377"),
                curve);

            var encryptedKey = "Zg8X6MW15TUGkDz0nw0qrvfdghsIzbeSVya+ZH7fJjwLU99CrJkpBkklRG+H1Q/Cmo1KVT0aINQPcGYVwfZxeqoo1du4i/hjfgDI+VUDWbQ=";

            var sharedKey = serverPublicKey.Multiply(userPrivateKey);
            Console.WriteLine(sharedKey.ToString());
            var decryptionKey = new Rfc2898DeriveBytes(
                sharedKey.ToString()!, new byte[] {0, 0, 0, 0, 0, 0, 0, 0}, 1000, HashAlgorithmName.SHA256);
            
            var encrypt = new CryptoService();
            var key = encrypt.Decrypt(encryptedKey, decryptionKey.GetBytes(32));
            
            Console.WriteLine(key);
        }
    }
}