using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class DaySix
{
    public static void PredictPatrolRoute()
    {
        var puzzle = new DaySixPuzzle("./PuzzleInputs/DaySix.txt");
        puzzle.PredictPatrolRoute();
    }
}

public class DaySixPuzzle
{
    // Constructor initializes the map and guard's starting position
    public DaySixPuzzle(string filePath)
    {
        Map = GetMapFromFile(filePath);
        YMax = Map.Length;
        XMax = Map[0].Length;

        // Locate the guard's starting position
        var reg = new Regex(@"\^|[v]|\>|\<");
        var guardPosY = Map.ToList().FindIndex(l => reg.IsMatch(l));
        var guardPosX = reg.Match(Map[guardPosY]).Index;

        GuardPosition = new Position
        {
            Y = guardPosY,
            X = guardPosX,
            Dir = Directions[Map[guardPosY][guardPosX]]
        };
        InitialGuardPosition = Position.Copy(GuardPosition);
    }

    private string[] Map; // Map grid
    private Position GuardPosition; // Current guard position
    private Position InitialGuardPosition; // Initial guard position for loop detection
    private int XMax, YMax; // Width and height of the map
    private HashSet<(int X, int Y)> PossibleBlockers = []; // Possible loop-causing positions
    private int DistinctPositions = 0; // Count of distinct positions visited

    // Directions the guard can move and their corresponding actions
    private static readonly Dictionary<char, Direction> Directions = new()
    {
        { '^', new Direction { Name = "up", NextDir = '>', XDiff = 0, YDiff = -1 } },
        { '>', new Direction { Name = "right", NextDir = 'v', XDiff = 1, YDiff = 0 } },
        { 'v', new Direction { Name = "down", NextDir = '<', XDiff = 0, YDiff = 1 } },
        { '<', new Direction { Name = "left", NextDir = '^', XDiff = -1, YDiff = 0 } },
    };

    // Predicts the patrol route based on the map and guard's movement
    public void PredictPatrolRoute()
    {
        var currentMap = Map.ToArray(); // Copy of the map to track visited positions
        StringBuilder sb;

        do
        {
            char currentCell = currentMap[GuardPosition.Y][GuardPosition.X];

            switch (currentCell)
            {
                case '#': // Blockage, step back and turn
                    GuardPosition.StepBackAndTurn();
                    break;

                case 'X': // Already visited position, step forward
                    GuardPosition.Step();
                    break;

                default: // New position
                    DistinctPositions++;

                    // Check if this position might cause a loop
                    if (CheckPositionForLoop(GuardPosition.X, GuardPosition.Y))
                        PossibleBlockers.Add((GuardPosition.X, GuardPosition.Y));

                    // Mark the position as visited
                    sb = new StringBuilder(currentMap[GuardPosition.Y]);
                    sb[GuardPosition.X] = 'X';
                    currentMap[GuardPosition.Y] = sb.ToString();

                    GuardPosition.Step();
                    break;
            }
        } while (IsInBounds(GuardPosition.X, GuardPosition.Y));

        Console.WriteLine($"Total distinct positions: {DistinctPositions}");
        Console.WriteLine($"Total possible blocker positions: {PossibleBlockers.Count}");
    }

    // Checks if a position might cause a loop
    public bool CheckPositionForLoop(int x, int y)
    {
        // Skip if this position is already a known blocker
        if (PossibleBlockers.Contains((x, y)))
            return false;

        // Simulate the guard's movements with the position blocked
        var testMap = Map.ToArray();
        var testPosition = Position.Copy(InitialGuardPosition);
        var visited = new HashSet<(int, int, string)>(); // Track visited positions

        // Block the specified position
        var sb = new StringBuilder(testMap[y]);
        sb[x] = '#';
        testMap[y] = sb.ToString();

        do
        {
            char currentCell = testMap[testPosition.Y][testPosition.X];

            switch (currentCell)
            {
                case '#': // Blockage, step back and turn
                    testPosition.StepBackAndTurn();
                    break;

                case 'X': // Already visited position, check for loops
                    // At face value this seems more complex than just storing/checking Positions
                    // but it's ~20x slower to do so on the full puzzle file...
                    var positionKey = (testPosition.X, testPosition.Y, testPosition.Dir.Name);
                    if (visited.Contains(positionKey))
                        return true;

                    visited.Add(positionKey);
                    testPosition.Step();
                    break;

                default: // New position
                    visited.Add((testPosition.X, testPosition.Y, testPosition.Dir.Name));
                    sb = new StringBuilder(testMap[testPosition.Y]);
                    sb[testPosition.X] = 'X';
                    testMap[testPosition.Y] = sb.ToString();
                    testPosition.Step();
                    break;
            }
        } while (IsInBounds(testPosition.X, testPosition.Y));

        return false;
    }

    // Checks if a position is within map boundaries
    public bool IsInBounds(int x, int y) =>
        x >= 0 && x < XMax && y >= 0 && y < YMax;

    // Reads the map from a file
    public static string[] GetMapFromFile(string filePath) =>
        File.ReadLines(filePath).ToArray();

    // Represents a direction with its properties
    public class Direction
    {
        public string Name = "";
        public char NextDir = ' ';
        public int XDiff = 0;
        public int YDiff = 0;
    }

    // Represents a position on the map
    public class Position
    {
        public int X = 0;
        public int Y = 0;
        public Direction Dir = new();

        // Creates a copy of a position
        public static Position Copy(Position original) => new Position
        {
            X = original.X,
            Y = original.Y,
            Dir = original.Dir
        };

        // Moves the position one step in the current direction
        public void Step()
        {
            X += Dir.XDiff;
            Y += Dir.YDiff;
        }

        // Moves the position one step back and changes direction
        public void StepBackAndTurn()
        {
            X -= Dir.XDiff;
            Y -= Dir.YDiff;
            Dir = Directions[Dir.NextDir];
        }
    }
}
