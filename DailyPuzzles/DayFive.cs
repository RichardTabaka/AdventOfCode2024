namespace AdventOfCode;

public static class DayFive
{
    // Method to check manual page orders and calculate the sum of middle page numbers
    public static void CheckManualPageOrders()
    {
        // Retrieve rules and sets of updates from the file
        var (rules, updateSets) = GetUpdateInfoFromFile("./PuzzleInputs/DayFive.txt");
        var invalidSets = new List<List<int>>();
        var sum = 0;

        // Process each update set to check its validity against the rules
        foreach (var set in updateSets)
        {
            var applicableRules = rules.Where(r => set.Contains(r[0]) && set.Contains(r[1])).ToList();

            // Skip if no applicable rules are found
            if (!applicableRules.Any()) continue;

            // Validate the set against the applicable rules
            var isValidSet = applicableRules.All(rule =>
                set.FindIndex(p => p == rule[0]) < set.FindIndex(p => p == rule[1]));

            if (isValidSet)
            {
                // Add the middle page number to the sum if valid
                sum += set[set.Count / 2];
            }
            else
            {
                // Collect invalid sets for later processing
                invalidSets.Add(new List<int>(set));
            }
        }

        // Output the sum of middle page numbers for valid sets
        Console.WriteLine($"Sum of middle page #s: {sum}");

        // Fix invalid sets and calculate their sum.
        FixManualPageOrders(invalidSets, rules);
    }

    // Method to fix invalid manual page orders and calculate the sum of fixed middle page numbers
    public static void FixManualPageOrders(List<List<int>> updateSets, List<List<int>> rules)
    {
        var sum = 0;

        foreach (var set in updateSets)
        {
            var applicableRules = rules.Where(r => set.Contains(r[0]) && set.Contains(r[1])).ToList();
            sum += ApplyBrokenRules(set, applicableRules);
        }

        // Output the sum of fixed middle page numbers
        Console.WriteLine($"Sum of fixed middle page #s: {sum}");
    }

    // Method to apply and fix broken rules within a set, ensuring rule compliance
    public static int ApplyBrokenRules(List<int> set, List<List<int>> rules)
    {
        // Identify rules that are violated (order is incorrect)
        var brokenRules = rules.Where(r => set.FindIndex(p => p == r[0]) > set.FindIndex(p => p == r[1])).ToList();

        // Resolve broken rules iteratively
        while (brokenRules.Count != 0)
        {
            foreach (var rule in brokenRules)
            {
                // Adjust the set to fix the rule violation
                set.Remove(rule[0]);
                set.Insert(set.FindIndex(p => p == rule[1]), rule[0]);
            }

            // Reevaluate broken rules after adjustments
            brokenRules = rules.Where(r => set.FindIndex(p => p == r[0]) > set.FindIndex(p => p == r[1])).ToList();
        }

        // Return the middle page number after fixing the set
        return set[set.Count / 2];
    }

    // Method to parse update information (rules and update sets) from a file
    public static (List<List<int>> Rules, List<List<int>> UpdateSets) GetUpdateInfoFromFile(string filePath)
    {
        var rules = new List<List<int>>();
        var updateSets = new List<List<int>>();
        var updateInfo = File.ReadLines(filePath);

        foreach (var line in updateInfo)
        {
            if (line.Contains('|'))
            {
                // Parse rules separated by '|'
                rules.Add(line.Split('|').Select(int.Parse).ToList());
            }
            else if (line.Contains(','))
            {
                // Parse update sets separated by ','
                updateSets.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }

        return (rules, updateSets);
    }
}
