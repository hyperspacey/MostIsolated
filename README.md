# MostIsolated
Most isolated point in a set

- Build using VS2017; tested on Windows 10. Requires a Windows machine to operate.

- This particular solution uses a KD tree to represent the relationship between our various points.
- From this we can calculate the nearest neighbour distance of all points in O(n log n) time.
- If we then do a fast sort (.Net's default Sort for lists/ Dictionaries is a Quicksort that works in O(n log N) - O(n) time depending on 
	input) and take the largest, we'll have the furthest distance to the nearest neighbour.
- The point with the furthest distance to its nearest neighbour is our most isolated point.