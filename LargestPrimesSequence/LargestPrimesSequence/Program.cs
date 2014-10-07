using System;
using System.Threading;

namespace LargestPrimesSequence
{
  class Program
  {
    public const int LengthOfFile = 6 * 10000000;

    private static void Main( string[] args )
    {
      if ( args.Length < 1 )
      {
        Console.WriteLine( "Please enter full path." );
        return;
      }
      string binName = args[0];

      if ( args.Length >= 2 )
      {
        string generate = args[1];
        if (generate == "-generateRandom")
        {
          Console.WriteLine("Generating binary file \"{0}\" with {1} random numbers", binName, LengthOfFile);
          BinaryGenerator.GenerateRandom(binName, LengthOfFile);
        }
        else if (generate == "-generate")
        {
          int[] mass = {2, 3, 5, 3, 5, 11};
//          int[] mass = {2, 3, 4, 5, 6, 7, 3, 5, 7};
//          int[] mass = {2, 3, 10,5, 6, 7, 3, 5, 7};
//          int[] mass = {2, 3, 3, 5, 6, 7, 3, 5, 7};
//          int[] mass = {2, 3, 5, 3, 5, 11};
//          int[] mass = { 2, 3, 5, 2, 5, 11 };
          Console.WriteLine( "Generating binary file \"{0}\" with numbers: {1}", binName, string.Join( " ", mass ) );

          BinaryGenerator.GenerateFromMassive(binName, mass);
        }
      }


      Thread workerThread = new Thread( Worker.DoWork );
      workerThread.Start( binName );

      Console.WriteLine( "Press ESCAPE key to stop" );
      while ( workerThread.IsAlive )
      {
        ConsoleKeyInfo cki = Console.ReadKey();
        if ( cki.Key == ConsoleKey.Escape && workerThread.IsAlive )
        {
          Worker.Stop();
          SequenceSearcher.GetResultCorrect();
          break;
        }
      }
    }
  }
}