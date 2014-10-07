using System;

namespace LargestPrimesSequence
{
    static class PrimeManager
    {
        public static bool IsPrime(ulong number)
        {
            if (number == 1 || number == 2)
                return true;

            if (number%2 != 0)
            {
                if (MillerRabin(number))
                {
                    return true;
                }
            }
            return false;
        }

        static bool MillerRabin(ulong number)
        {
            ulong[] ar;
            if (number < 4759123141) ar = new ulong[] { 2, 7, 61 };
            else if (number < 341550071728321) ar = new ulong[] { 2, 3, 5, 7, 11, 13, 17 };
            else ar = new ulong[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
            ulong d = number - 1;
            int s = 0;
            while ((d & 1) == 0) { d >>= 1; s++; }
            int i, j;
            for (i = 0; i < ar.Length; i++)
            {
                ulong a = Math.Min(number - 2, ar[i]);
                ulong now = Pow(a, d, number);
                if (now == 1) continue;
                if (now == number - 1) continue;
                for (j = 1; j < s; j++)
                {
                    now = Mul(now, now, number);
                    if (now == number - 1) break;
                }
                if (j == s) return false;
            }
            return true;
        }

        static ulong Mul(ulong a, ulong b, ulong mod)
        {
            int i;
            ulong now = 0;
            for (i = 63; i >= 0; i--) if (((a >> i) & 1) == 1) break;
            for (; i >= 0; i--)
            {
                now <<= 1;
                while (now > mod) now -= mod;
                if (((a >> i) & 1) == 1) now += b;
                while (now > mod) now -= mod;
            }
            return now;
        }

        static ulong Pow(ulong a, ulong p, ulong mod)
        {
            if (p == 0) return 1;
            if (p % 2 == 0) return Pow(Mul(a, a, mod), p / 2, mod);
            return Mul(Pow(a, p - 1, mod), a, mod);
        }
    }
}
