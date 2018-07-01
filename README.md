# MostIsolated
Most isolated point in a set

- Build using VS2017; tested on Windows 10. Requires a Windows machine to operate.
- Run via command line, eg in PowerShell type ./MostIsolated filename.txt
- Naive implementation with regards to input data validation, ignores bad data and outputs an error
- Uses Code And Cats kdTree; well-tested, popular implementation. Link: https://github.com/codeandcats/KdTree

- This particular solution uses a KD tree to represent the relationship between our various points.
- From this we can calculate the nearest neighbour distance of all points in O(n log n) time.
- Since we have to iterate over all locations to get their nearest neighbour we can track which one is furthest away
- The point with the furthest distance to its nearest neighbour is our most isolated point.