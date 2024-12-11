namespace AdventOfCode;

public static class DayEight
{
    public static void FindTheAntinodes()
    {
        var map = GetMapFromFile("./PuzzleInputs/DayEight.txt");
        var yMax = map.Length;
        var xMax = map[0].Length;
        var validAntinodeCount = 0;

        // Dictionary to store antenna positions grouped by type
        Dictionary<char, List<(int x, int y)>> antennae = new();
        // HashSets to store discovered antinodes and continuous antinodes
        HashSet<(int x, int y)> antinodes = new();
        HashSet<(int x, int y)> continuousAntinodes = new();

        // Load antenna positions into the dictionary
        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                if (map[y][x] != '.')
                {
                    if (!antennae.ContainsKey(map[y][x]))
                        antennae[map[y][x]] = new List<(int x, int y)>();

                    antennae[map[y][x]].Add((x, y));
                }
            }
        }

        // Calculate antinodes for each antenna type
        foreach (var key in antennae.Keys)
        {
            var antennaSet = antennae[key];

            for (int i = 0; i < antennaSet.Count - 1; i++)
            {
                for (int j = i + 1; j < antennaSet.Count; j++)
                {
                    var xDiff = antennaSet[j].x - antennaSet[i].x;
                    var yDiff = antennaSet[j].y - antennaSet[i].y;

                    // Calculate symmetric antinodes
                    antinodes.Add((antennaSet[i].x - xDiff, antennaSet[i].y - yDiff));
                    antinodes.Add((antennaSet[j].x + xDiff, antennaSet[j].y + yDiff));

                    // Calculate continuous antinodes along the line defined by the antennas
                    foreach (var coord in GetAntinodeCoords(antennaSet[i].x, antennaSet[i].y, xDiff, yDiff, xMax, yMax))
                    {
                        continuousAntinodes.Add(coord);
                    }
                }
            }
        }

        // Count valid antinodes within the map bounds
        foreach (var node in antinodes)
        {
            if (node.x >= 0 && node.x < xMax && node.y >= 0 && node.y < yMax)
            {
                validAntinodeCount++;
            }
        }

        // Output results
        Console.WriteLine($"Antinodes: {validAntinodeCount}");
        Console.WriteLine($"Continuous Antinodes: {continuousAntinodes.Count}");
    }

    // Method to calculate antinode coordinates along a vector direction
    public static HashSet<(int x, int y)> GetAntinodeCoords(int x, int y, int xDiff, int yDiff, int xMax, int yMax)
    {
        HashSet<(int x, int y)> result = new();

        // Forward direction
        int testX = x, testY = y;
        while (testX >= 0 && testX < xMax && testY >= 0 && testY < yMax)
        {
            result.Add((testX, testY));
            testX += xDiff;
            testY += yDiff;
        }

        // Reset and test backward direction
        testX = x;
        testY = y;
        while (testX >= 0 && testX < xMax && testY >= 0 && testY < yMax)
        {
            result.Add((testX, testY));
            testX -= xDiff;
            testY -= yDiff;
        }

        return result;
    }

    // Method to read the map from a file
    public static string[] GetMapFromFile(string filePath) =>
        File.ReadLines(filePath).ToArray();
}
