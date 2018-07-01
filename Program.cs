
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
                    // Store in a dictionary for faster lookup via location ID
                    Dictionary<string, Location> locations = GetLocationsFromFile(filePath);
                    if (locations == null)
                    {
                        Console.WriteLine("Error reading locations");
                    }
                    else
                    {
                        List<string> result = GetMostIsolated(locations);
                        if (result == null)
                        {
                            Console.WriteLine("Error finding most isolated point");
                        }
                        else
                        {
                            foreach (string entry in result)
                            {
                                Console.WriteLine(entry);
                            }
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

        private static List<string> GetMostIsolated (Dictionary<string, Location> locations)
        {
            KdTree<float, string> tree = GenerateTreeFromLocations(locations);

            List<string> mostIsolated = new List<string>();
            double furthestDistanceSqd = 0;
            foreach (KeyValuePair<string,Location> locationKvp in locations)
            {
                Location location = locationKvp.Value;
                var nearestNeighbourNode = tree.GetNearestNeighbours(new float[] { location.X, location.Y }, 2);
                string nearestNeighbourID = nearestNeighbourNode[1].Value; // Take the SECOND entry, first will always be self
                Location nearestNeighbour = locations[nearestNeighbourID];

                double distanceSqd = GetDistanceSqd (location, nearestNeighbour);
                if (distanceSqd > furthestDistanceSqd)
                {
                    mostIsolated.Clear();
                    mostIsolated.Add(location.ID);
                    furthestDistanceSqd = distanceSqd;
                }
                else if (distanceSqd == furthestDistanceSqd)
                {
                    mostIsolated.Add(location.ID);
                }
            }

            return mostIsolated;
        }

        private static double GetDistanceSqd (Location locationA, Location locationB)
        {
            return (Math.Pow(locationA.X - locationB.X, 2) + Math.Pow(locationA.Y - locationB.Y, 2));
        }

        private static Dictionary<string, Location> GetLocationsFromFile(string filePath)
        {
            // Read our input
            Dictionary<string, Location> locations = new Dictionary<string, Location>();

            TextReader input = File.OpenText(filePath);
            string line;
            int numLocations = 0;
            while ((line = input.ReadLine()) != null)
            {
                Location location = ParseLocation(line);
                if (location != null)
                {
                    locations.Add(location.ID, location);
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
                return locations;
            }
        }

        private static KdTree<float, string> GenerateTreeFromLocations(Dictionary<string,Location> locations)
        {
            // Generate a 2D tree from our input locations
            KdTree<float, string> tree = new KdTree<float, string>(2, new FloatMath());
            foreach (KeyValuePair<string,Location> locationKvp in locations)
            {
                Location location = locationKvp.Value;
                tree.Add(new float[] { location.X, location.Y }, location.ID);
            }

            return tree;
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
