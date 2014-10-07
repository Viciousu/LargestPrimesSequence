using System;
using System.Collections.Generic;
using System.Linq;

namespace LargestPrimesSequence
{
    public static class SequenceSearcher
    {
        private static List<PrimePosition> resultList = new List<PrimePosition>();
        private static List<PrimePosition> currentResultList = new List<PrimePosition>();
        private static object syncLock = new object();

        public static void AddPrimes(List<PrimePosition> primesList)
        {
            lock (syncLock)
            {
                CalculateLongestSequence(primesList);
            }
        }

        public static void GetResult()
        {
            lock (syncLock)
            {
                resultList.ForEach(b => Console.WriteLine("Prime {0} in position {1}", b.Prime, b.Position));
            }
        }

        public static void GetResultCorrect()
        {
            lock (syncLock)
            {
                if (resultList.Count == 0)
                    Console.WriteLine("Nothing found");
                else
                    Console.WriteLine(
                        "First prime={0} in position {1}, last prime={2} in position {3}, sequence length={4}",
                        resultList.First().Prime, resultList.First().Position, resultList.Last().Prime,
                        resultList.Last().Position, resultList.Count);
            }
        }

        private static void CalculateLongestSequence(IEnumerable<PrimePosition> currentList)
        {
            var sortedPrimesList = currentList.OrderBy(x => x.Position).ToList();
            var lastPrimePosition = new PrimePosition();

            foreach (var primePosition in sortedPrimesList)
            {
                if (primePosition.Prime <= lastPrimePosition.Prime)
                {
                    currentResultList.Clear();
                }
                currentResultList.Add(primePosition);
                lastPrimePosition = primePosition;
                if (CompareLists(currentResultList, resultList))
                    resultList = currentResultList.ToList();
            }
        }

        private static bool CompareLists( List<PrimePosition> compareNew, List<PrimePosition> compareOld)
        {
            if (compareNew.Count == compareOld.Count)
            {
                if (compareNew[0].Prime == compareOld[0].Prime)
                {
                    return compareNew[0].Position < compareOld[0].Position;
                }
                return compareNew[0].Prime > compareOld[0].Prime;
            }
            return compareNew.Count > compareOld.Count;
        }
    }


    public class PrimePosition
    {
        public PrimePosition()
        {
            this.Prime = 0;
            this.Position = 0;
        }

        public PrimePosition(ulong prime, int position)
        {
            this.Prime = prime;
            this.Position = position;
        }

        public ulong Prime { get; set; }
        public int Position { get; set; }
    }
}
