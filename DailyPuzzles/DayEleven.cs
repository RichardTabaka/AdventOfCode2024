namespace AdventOfCode;

public static class DayEleven
{
    public static void BlinkAndChangeStones()
    {
        // Brute force has failed me, time to work smarter
        var stones = GetStonesFromFile("./PuzzleInputs/DayEleven.txt");

        // Perform the first 25 iterations of blinking
        for (int i = 0; i < 25; i++)
        {
            stones = BlinkFaster(stones);
        }
        // Output the total count of stones after 25 iterations
        Console.WriteLine($"After 25 blinks: {stones.Values.Sum()}");

        // Perform the next 50 iterations of blinking
        for (int i = 0; i < 50; i++)
        {
            stones = BlinkFaster(stones);
        }
        // Output the total count of stones after 75 iterations
        Console.WriteLine($"After 75 blinks: {stones.Values.Sum()}");
    }

    // Simulates one iteration of the blinking process
    public static Dictionary<long, long> BlinkFaster(Dictionary<long, long> previousStones)
    {
        var newStones = new Dictionary<long, long>();

        foreach (var (key, count) in previousStones)
        {
            // Case 1: If the stone value is 0, it turns into a stone with value 1
            if (key == 0)
            {
                AddToDictionary(newStones, 1, count);
            }
            else
            {
                int digits = key.ToString().Length;

                // Case 2: If the number of digits is odd, multiply the stone by 2024
                if (digits % 2 != 0)
                {
                    AddToDictionary(newStones, key * 2024, count);
                }
                else
                {
                    // Case 3: If the number of digits is even, split the number into two parts
                    int divisor = (int)Math.Pow(10, digits / 2);
                    long left = key / divisor;
                    long right = key % divisor;

                    // Add the two parts as new stones
                    AddToDictionary(newStones, left, count);
                    AddToDictionary(newStones, right, count);
                }
            }
        }

        return newStones;
    }

    // Helper method to add or update a value in the dictionary
    private static void AddToDictionary(Dictionary<long, long> dict, long key, long valueToAdd)
    {
        // Check if the key already exists; if so, update its value; otherwise, add a new key-value pair
        if (dict.TryGetValue(key, out var existingValue))
        {
            dict[key] = existingValue + valueToAdd;
        }
        else
        {
            dict[key] = valueToAdd;
        }
    }

    public static Dictionary<long, long> GetStonesFromFile(string filePath) =>
        File.ReadLines(filePath)
            .Single()
            .Split(" ")
            .Select(long.Parse)
            .GroupBy(s => s)
            .ToDictionary(g => g.Key, g => (long)g.Count());
}
