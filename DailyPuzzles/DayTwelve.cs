namespace AdventOfCode;

public static class DayTwelve
{
    private static char[][] Farm = [];
    private static int MaxX, MaxY;
    private static HashSet<(int x, int y)> MappedPlots = [];
    private static HashSet<Region> Regions = [];
    public static void AnalyzeFarmPlan()
    {
        Farm = GetFarmMapFromFile("./PuzzleInputs/DayTwelve.txt");
        MaxY = Farm.Length - 1;
        MaxX = Farm[MaxY].Length - 1;


        for (int y = 0; y < Farm.Length; y++)
        {
            for (int x = 0; x < Farm[y].Length; x++)
            {
                if (MappedPlots.Any(p => p.x == x && p.y == y))
                    continue;
                else
                {
                    var r = new Region(x, y);
                    Regions.Add(r);
                }
            }
        }
        List<Region> regions = new List<Region>(Regions);

        //System.Console.WriteLine($"Total Farm Cost: {regions.Sum(r => r.TotalCost)}");

        //regions.ForEach(r => Console.WriteLine(r.ToString()));

        System.Console.WriteLine($"Total bulk cost: {regions.Sum(r => r.BulkCost)}");
    }

    public static bool CanRegionEscapeSector(Region r, IEnumerable<int> xRange, IEnumerable<int> yRange, char surroundingPlant) =>
        r.Plots.Any(p => RouteOut(p, xRange, yRange, surroundingPlant, new HashSet<(int x, int y)>()));

    public static bool RouteOut((int x, int y) p, IEnumerable<int> xRange, IEnumerable<int> yRange, char bad, HashSet<(int x, int y)> visited)
    {
        // bad route
        if (visited.Contains(p) || (IsValidPosition(p) && Farm[p.y][p.x] == bad))
            return false;
        // We've escaped!
        if (!IsValidPosition(p) || !xRange.Contains(p.x) || !yRange.Contains(p.y))
            return true;

        visited.Add(p);
        return Directions.Values.Any(d => RouteOut((p.x + d.xDiff, p.y + d.yDiff), xRange, yRange, bad, visited));
    }

    public static bool IsValidPosition((int x, int y) pos) =>
      pos.x >= 0 && pos.y >= 0 && pos.x <= MaxX && pos.y <= MaxY;

    // used for subsections of map
    public static bool IsValidPosition((int x, int y) pos, int maxX, int minX, int maxY, int minY) =>
      pos.x >= minX && pos.y >= minY && pos.x <= maxX && pos.y <= maxY;

    public static char[][] GetFarmMapFromFile(string filePath) =>
      File.ReadLines(filePath)
        .Select(s => s.ToArray())
        .ToArray();

    // Movement directions for navigating the grid
    private static readonly Dictionary<string, (int xDiff, int yDiff, string right, string left)> Directions = new()
    {
        { "up", (0, -1, "right", "left") },
        { "down", (0, 1, "left", "right") },
        { "left", (-1, 0, "up", "down") },
        { "right", (1, 0, "down", "up") }
    };
    public class Region
    {
        public Region() { }
        public Region(int x, int y)
        {
            Plant = Farm[y][x];
            loadPlotInfo(x, y);
            MappedPlots.UnionWith(Plots.Select(p => (p.x, p.y)));
        }

        // returns true if connected
        private bool loadPlotInfo(int x, int y)
        {
            // If it's already visited, invalid, or the wrong plant, exit
            if (Plots.Any(p => p.x == x && p.y == y))
                return true;
            if (!IsValidPosition((x, y)) || Farm[y][x] != Plant)
                return false;

            // It's part of the region, add it
            Plots.Add((x, y));

            int neighbors = 0;
            foreach (var dir in Directions.Values)
            {
                var testX = x + dir.xDiff;
                var testY = y + dir.yDiff;

                neighbors += loadPlotInfo(testX, testY) ? 1 : 0;
            }
            Perimeter += 4 - neighbors;

            return true;
        }

        public int GetInternalBulkPerimeter()
        {
            // get rough sector of map containing region
            var xMin = Plots.Select(p => p.x).Min();
            var xMax = Plots.Select(p => p.x).Max();
            var xRange = Enumerable.Range(xMin, xMax);
            var yMin = Plots.Select(p => p.y).Min();
            var yMax = Plots.Select(p => p.y).Max();
            var yRange = Enumerable.Range(yMin, yMax);

            // grab every region that exists _entirely_ inside this sector
            // This works great
            // Except when it doesn't like for this map :(
            /*
            CCAAA
            CCAAA
            AABBA
            AAAAA
            */
            // Should be 164 but it thinks that B region is internal :(
            var internalRegions = Regions
                .Where(r =>
                    r.Plots
                        .All(p => xRange.Contains(p.x) && yRange.Contains(p.y)))
                        .Where(r => r.Plant != Plant && !CanRegionEscapeSector(r, xRange, yRange, Plant))
                        .ToList();
            if (internalRegions.Count == 0)
                return 0;
            // store current farm so we can modify then reset
            char[][] tempFarm = (char[][])Farm.Clone();

            // make all internal regions same character so we can identfiy only internal perim of region we want
            internalRegions.ForEach(r => r.Plots.ToList().ForEach(p => Farm[p.y][p.x] = '1'));

            var newInternalRegions = new List<Region>();
            for (int y = yMin; y < yMax; y++)
            {
                for (int x = xMin; x < xMax; x++)
                {
                    if (Farm[y][x] == Plant || newInternalRegions.Any(r => r.Plots.Contains((x, y))))
                        continue;
                    else
                    {
                        var r = new Region(x, y);
                        newInternalRegions.Add(r);
                    }
                }
            }

            // reset the farm at the end for future use
            Farm = (char[][])tempFarm.Clone();

            return newInternalRegions.Sum(r => r.GetExternalBulkPerimeter());
        }

        public int GetExternalBulkPerimeter()
        {
            var firstPos = Plots.OrderBy(p => p.y).ThenBy(p => p.x).First();
            var currentPos = (firstPos.x, firstPos.y);
            var currentDir = Directions["down"];
            var turns = 0;
            do
            {
                // check if we need to turn right
                var rightDir = Directions[currentDir.right];
                (int x, int y) rightPos = (currentPos.x + rightDir.xDiff, currentPos.y + rightDir.yDiff);
                if (IsValidPosition(rightPos) && Farm[rightPos.y][rightPos.x] == Plant)
                {
                    turns++;
                    currentPos = rightPos;
                    currentDir = rightDir;
                    continue;
                }

                // check if we need can go straight
                (int x, int y) forwardPos = (currentPos.x + currentDir.xDiff, currentPos.y + currentDir.yDiff);
                if (IsValidPosition(forwardPos) && Farm[forwardPos.y][forwardPos.x] == Plant)
                {
                    currentPos = forwardPos;
                    continue;
                }

                // check left
                var leftDir = Directions[currentDir.left];
                (int x, int y) leftPos = (currentPos.x + leftDir.xDiff, currentPos.y + leftDir.yDiff);
                if (IsValidPosition(leftPos) && Farm[leftPos.y][leftPos.x] == Plant)
                {
                    turns++;
                    // Are we at the start and facing down? Exit!
                    if (currentPos.x == firstPos.x && currentPos.y == firstPos.y && currentDir.left == "down")
                        return turns;
                    currentDir = leftDir;
                    currentPos = leftPos;
                    // How about now? (first pos didn't have one below, first actual step was right)
                    if (currentPos.x == firstPos.x && currentPos.y == firstPos.y && currentDir.left == "down")
                        return turns + 1;
                    continue;
                }

                // end reached?
                if (currentPos.x == firstPos.x && currentPos.y == firstPos.y && currentDir.left == "down")
                    return turns + 1;

                // u-turn needed
                currentDir = Directions[leftDir.left];
                turns += 2;
            } while (currentPos.x != firstPos.x || currentPos.y != firstPos.y || currentDir != Directions["down"]);

            return turns;
        }

        public int Area => Plots.Count;
        public int Perimeter = 0;
        public char Plant = '0';
        public int TotalCost => Area * Perimeter;
        public int Corners = 0;
        public int BulkCost => (GetExternalBulkPerimeter() + GetInternalBulkPerimeter()) * Area;
        // public int BulkCost => GetExternalBulkPerimeter() * Area;

        public override string ToString() =>
          $"A region of {Plant} plants with price {Area} * {GetExternalBulkPerimeter()} = {BulkCost}";
        public HashSet<(int x, int y)> Plots = [];
    }
}