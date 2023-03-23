using System;
using System.Numerics;

namespace Diploma.ECC.Math.Utils
{
    public static class BigIntGenerator
    {
        public static BigInteger GenerateRandom(BigInteger N)
        {
            var random = new Random();
            var  bytes = N.ToByteArray ();
            BigInteger R;

            do {
                random.NextBytes (bytes);
                bytes [bytes.Length - 1] &= (byte)0x7F;
                R = new BigInteger (bytes);
            } while (R >= N);

            return R;
        }
    }
}