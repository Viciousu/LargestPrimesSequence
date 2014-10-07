using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace LargestPrimesSequence
{
    class Worker
    {
        private static int _positionCounter = 0;
        private static int _position = 0;
        private const int BufferSize = 6 * 1000000;
        private static readonly ManualResetEvent StopEvent = new ManualResetEvent(false);
        public static CancellationTokenSource cts = new CancellationTokenSource();

        private static bool Running
        {
            get { return !StopEvent.WaitOne(0); }
        }

        public static void DoWork(object data)
        {
            var filePath = data as string;
            var buffer = new byte[BufferSize];
  
            var taskStarted = DateTime.Now;
            Console.WriteLine("Task started at {0}", taskStarted);

            try
            {
                using (var fileStream = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
                {
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0 && Running)
                    {
                        _position++;

                        Console.WriteLine("Processing chunk: {0}", _position);

                        var splittedList = SplitArrayBySixBytes(buffer, read);

                        var primeNumbers = splittedList
                            .AsParallel()
                            .AsOrdered()
                            .Where(element => PrimeManager.IsPrime(element.Prime))
                            .Select(x => x).WithCancellation(cts.Token).ToList();

                        SequenceSearcher.AddPrimes(primeNumbers);
                    }
                }
                SequenceSearcher.GetResultCorrect();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Can't access file");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
            catch (IOException)
            {
                Console.WriteLine("Can't read file");
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Process was stopped");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Process was stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with exception:{0}", ex);
            }

            var taskEnded = DateTime.Now;
            var timeDifference = taskEnded - taskStarted;
            Console.WriteLine("Task ended at {0}, done in {1}", taskEnded, timeDifference.ToString());
        }

        public static void Stop()
        {
            StopEvent.Set();
            cts.Cancel();
        }

        static ulong ConvertBytesToUlong(byte[] bytes)
        {
            var curr = new byte[8];
            curr[7] = curr[6] = 0;
            Buffer.BlockCopy(bytes, 0, curr, 0, 6);
            ulong l = BitConverter.ToUInt64(curr, 0);
            return l;
        }

        static List<PrimePosition> SplitArrayBySixBytes(byte[] byteArrayIn, int length)
        {
            var listToReturn = new List<PrimePosition>();

            for (int u = 0; u < length / 6; u++)
            {
                _positionCounter++;
                var interByte = new byte[6];
                for (int p = 0; p < 6; p++)
                {
                    interByte[p] = byteArrayIn[(6 * u) + p];
                }
                var l = ConvertBytesToUlong(interByte);
                var pos = new PrimePosition(l, _positionCounter);
                listToReturn.Add(pos);
            }
            return listToReturn;
        }
    }
}
