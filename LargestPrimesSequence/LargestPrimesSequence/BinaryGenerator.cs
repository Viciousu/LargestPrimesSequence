using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace LargestPrimesSequence
{
    static class BinaryGenerator
    {
        private static readonly Random GetRandom = new Random();

        static byte[] IntToBytes(int intValue)
        {
            var result = new byte[6];
            var intBytes = BitConverter.GetBytes(intValue);
            Buffer.BlockCopy(intBytes, 0, result, 0, 4);
            result[5] = result[4] = 0;
            return result;
        }

        public static void GenerateFromMassive(string filePath, int[] mass)
        {
            var uintsList = mass.Select(IntToBytes).ToList();
            WriteBinary(filePath, uintsList);
        }


        static void WriteBinary(string filePath, List<byte[]> bytesList)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (var binWriter = new BinaryWriter(fileStream))
                {
                    bytesList.ForEach(binWriter.Write);
                }
            }
        }

        public static void GenerateRandom(string filePath,int amount)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (var binWriter = new BinaryWriter(fileStream))
                {
                    for (var i = 0; i <= amount; i++)
                    {
                        var buf = new byte[6];
                        GetRandom.NextBytes(buf);
                        binWriter.Write(buf);
                    }
                }
            }

        }
    }
}
