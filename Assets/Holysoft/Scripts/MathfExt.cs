using UnityEngine;

namespace Holysoft
{
    public struct MathfExt
    {
        public static int RandomSign() => Random.Range(0, 2) * 2 - 1;

        public static Vector2 RadianToVector2(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        public static Vector2 DegreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);

        /// <summary>
        /// Faster than Mathf.Pow
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public static float Exponate(float value, int exponent)
        {
            float output = 1f;

            for (int i = 0; i < exponent; i++)
            {
                output *= value;
            }

            return output;
        }

        public static float Exponate(float value, float exponent)
        {
            int wholeExponent = Mathf.FloorToInt(exponent);
            float secondPartition = exponent - wholeExponent;

            if (secondPartition == 0)
            {
                return Exponate(value, wholeExponent);
            }

            float firstParition = 1f - secondPartition;
            return (firstParition * Exponate(value, wholeExponent)) + (secondPartition * Exponate(value, wholeExponent + 1));
        }

        /// <summary>
        /// Number of ways of choosing i objects out of n objects
        /// </summary>
        /// <param name="n">Total number</param>
        /// <param name="i">How many to choose out of the total</param>
        /// <returns></returns>
        public static int Combination(int n, int i) => Factorial(n) / (Factorial(i) * Factorial(n - i));

        /// <summary>
        /// Number of ways of choosing i objects out of n objects
        /// the order matters
        /// </summary>
        /// <param name="n">Total number</param>
        /// <param name="i">How many to choose out of the total</param>
        /// <returns></returns>
        public static int Permutation(int n, int i) => Factorial(n) / Factorial(n - i);

        /// <summary>
        ///
        /// </summary>
        /// <param name="n">End number</param>
        /// <param name="i">Starting number</param>
        /// <returns></returns>
        public static float Summation(int n, int i)
        {
            float output = 0;

            for (int j = i; j <= n; j++)
            {
                output += i;
            }

            return output;
        }

        /// <summary>
        /// n!
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Factorial(int n)
        {
            int output = 1;

            for (int i = n; i > 0; i--)
            {
                output *= i;
            }

            return output;
        }

        public static float RoundDecimalTo(uint decimalPlaces,float value)
        {
            var factor = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                factor *= 10;
            }
            return Mathf.Round(value * factor) / factor;
        }

        public static Vector2 RoundVectorValuesTo(uint decimalPlace, Vector2 vector2)
        {
            return new Vector2(MathfExt.RoundDecimalTo(decimalPlace, vector2.x), MathfExt.RoundDecimalTo(decimalPlace, vector2.y));
        }
    }
}