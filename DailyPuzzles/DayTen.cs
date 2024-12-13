namespace AdventOfCode;

public static class DayTen
{
    // Global variables to store map dimensions and data
    private static int MaxX;
    private static int MaxY;
    private static int[][] Map = [];

    // Caches for trail computation
    private static HashSet<(int score, int x, int y)> CompleteTrailPositionsDistinct = new();
    private static HashSet<(int x, int y, HashSet<(int, int)> nines)> CompleteTrailPositions = new();

    // Movement directions for navigating the grid
    private static readonly Dictionary<string, (int xDiff, int yDiff)> Directions = new()
    {
        { "up", (0, -1) },
        { "down", (0, 1) },
        { "left", (-1, 0) },
        { "right", (1, 0) }
    };

    public static void FindTrailScore()
    {
        // Load map from file and set dimensions
        Map = GetTopoMapFromFile("./PuzzleInputs/DayTen.txt");
        MaxY = Map.Length - 1;
        MaxX = Map[0].Length - 1;

        int trails = 0;
        int distinctTrails = 0;

        // Iterate over all positions on the map
        for (int y = 0; y <= MaxY; y++)
        {
            for (int x = 0; x <= MaxX; x++)
            {
                // Only process positions starting at height 0
                if (Map[y][x] != 0)
                    continue;

                trails += GetPositionForwardScore(0, x, y).Count;
                distinctTrails += GetDistinctTrailScore(0, x, y);
            }
        }

        Console.WriteLine($"Complete trails: {trails}");
        Console.WriteLine($"Distinct trails: {distinctTrails}");
    }

    // Calculates all possible positions leading to a height of 9
    private static HashSet<(int x, int y)> GetPositionForwardScore(int current, int x, int y)
    {
        // Check bounds
        if (x < 0 || x > MaxX || y < 0 || y > MaxY)
            return new();

        // If at height 9, return current position
        if (current == 9)
            return Map[y][x] == 9 ? new() { (x, y) } : new();

        // If current position matches the height, process further
        if (Map[y][x] == current)
        {
            // Check if the result is already cached
            if (CompleteTrailPositions.Any(p => p.x == x && p.y == y))
                return CompleteTrailPositions.Single(p => p.x == x && p.y == y).nines;

            HashSet<(int x, int y)> nines = [];

            // Explore all directions recursively
            foreach (var dir in Directions.Values)
            {
                var positions = GetPositionForwardScore(current + 1, x + dir.xDiff, y + dir.yDiff);
                nines.UnionWith(positions);
            }

            // Cache the result
            CompleteTrailPositions.Add((x, y, nines));
            return nines;
        }

        return new();
    }

    // Calculates distinct trail scores
    public static int GetDistinctTrailScore(int current, int x, int y)
    {
        // Check bounds
        if (x < 0 || x > MaxX || y < 0 || y > MaxY || Map[y][x] != current)
            return 0;

        // If at height 9, return 1 if valid
        if (current == 9)
            return 1;

        // Check if the result is already cached
        if (CompleteTrailPositionsDistinct.Any(p => p.x == x && p.y == y))
            return CompleteTrailPositionsDistinct.Single(p => p.x == x && p.y == y).score;

        // Explore all directions recursively and sum scores
        int score = Directions.Values
            .Select(dir => GetDistinctTrailScore(current + 1, x + dir.xDiff, y + dir.yDiff))
            .Sum();

        // Cache the result
        CompleteTrailPositionsDistinct.Add((score, x, y));
        return score;
    }

    // Reads the topographical map from a file
    public static int[][] GetTopoMapFromFile(string filePath) =>
        File.ReadLines(filePath)
            .Select(line => line.Select(ch => int.Parse(ch.ToString())).ToArray())
            .ToArray();
}
