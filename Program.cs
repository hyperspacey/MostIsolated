
using System;
using System.Collections.Generic;
using System.IO;
using KdTree;
using KdTree.Math;

namespace MostIsolated
{
    class Program
    {
        class Location
        {
            public string ID { get; private set; }
            public float X { get; private set; }
            public float Y { get; private set; }

            public Location (string ID, float X, float Y)
            {
                this.ID = ID;
                this.X = X;
                this.Y = Y;
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var filePath = args[0];
                if (File.Exists(filePath))
                {
                    KdTree<float, string> tree = GenerateTreeFromFile(filePath);
                    if (tree == null)
                    {
                        Console.WriteLine("Tree generation failure");
                    }
                    else
                    {
                        List<string> result = new List<string> { "No output", "No output" };//GetMostIsolated(tree);
                        foreach (string entry in result)
                        {
                            Console.WriteLine(entry);
                        }
                    }
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

        private static KdTree<float, string> GenerateTreeFromFile(string filePath)
        {
            // Read our input and get it into a KD Tree
            KdTree<float, string> tree = new KdTree<float, string>(2, new FloatMath());

            TextReader input = File.OpenText(filePath);
            string line;
            int numLocations = 0;
            while ((line = input.ReadLine()) != null)
            {
                Location location = ParseLocation(line);
                if (location != null)
                {
                    tree.Add(new float[] { location.X, location.Y }, location.ID);
                    ++numLocations;
                }
                else
                {
                    Console.WriteLine("Error parsing text into location: " + line);
                }
            }

            if (numLocations < 3)
            {
                Console.WriteLine("Insufficient locations; need at least three to establish a furthest point");
                return null;
            }
            else
            {
                return tree;
            }
        }

        private static Location ParseLocation (string locationText)
        {
            string[] entries = locationText.Split(' ');
            if (entries.Length == 3)
            {
                string id = entries[0];
                if (float.TryParse(entries[1], out float x) && float.TryParse(entries[2], out float y))
                {
                    return new Location(id, x, y);
                }
            }
            return null;
        }
    }
}
