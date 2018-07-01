
using System;
using System.IO;

namespace MostIsolated
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var filePath = args[0];
                if (File.Exists(filePath))
                {
                    TextReader input = File.OpenText(filePath);
                    Console.WriteLine(input.ReadToEnd());
                }
                else
                {
                    Console.WriteLine("File does not exist");
                }
            }
            else
            {
                Console.WriteLine("Please specify a file");
            }
        }
    }
}
