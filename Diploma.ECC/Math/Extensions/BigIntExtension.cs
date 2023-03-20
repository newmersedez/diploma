using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Diploma.ECC.Math.Extensions
{
    /// <summary>
    /// Класс для работы с типами BigInteger
    /// </summary>
    public static class BigIntExtension
    {
        /// <summary>
        /// Получить модуль числа
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="modulo">Модуль</param>
        /// <returns></returns>
        public static BigInteger Module(this BigInteger value, BigInteger modulo)
        {
            var reminder = value % modulo;

            return reminder < 0 ? reminder + modulo : reminder;
        }
        
        /// <summary>
        /// Получить обратный модуль
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="modulo">Модуль</param>
        /// <returns></returns>
        public static BigInteger ModuleInverse(this BigInteger value, BigInteger modulo)
        {
            if (value == 0)
                throw new DivideByZeroException();

            if (value < 0)
                return modulo - ModuleInverse(-value, modulo);

            BigInteger a = 0, oldA = 1;
            BigInteger b = modulo, oldB = value;

            while (b != 0)
            {
                var quotient = oldB / b;

                var prov = b;
                b = oldB - quotient * prov;
                oldB = prov;

                prov = a;
                a = oldA - quotient * prov;
                oldA = prov;
            }

            var gcd = oldB;
            var c = oldA;

            if (gcd != 1)
                throw new ArgumentOutOfRangeException(nameof(gcd), $"Неверное значение gcd - {gcd}");

            if (Module(value * c, modulo) != 1)
                throw new ArithmeticException("Не удалось получить модуль");

            return Module(c, modulo);
        }
        
        /// <summary>
        /// Получить случайное число
        /// </summary>
        /// <param name="length">Длина числа</param>
        /// <returns></returns>
        public static BigInteger GetNumber(uint length)
        {
            var randomGenerator = RandomNumberGenerator.Create();
            var randomNumber = new byte[length];
            randomGenerator.GetBytes(randomNumber);
            
            return new BigInteger(randomNumber);
        }
    }
}