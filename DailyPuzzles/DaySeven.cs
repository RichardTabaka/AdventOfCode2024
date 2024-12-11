using System.Runtime.InteropServices;

namespace AdventOfCode;

public static class DaySeven
{
    public static void TestOperators()
    {
        // Read equations from the input file
        var equations = GetEquationsFromFile("./PuzzleInputs/DaySeven.txt");

        long sumOfValidEquations = 0;
        long sumWithConcat = 0;

        equations.ForEach(eq =>
        {
            // Split the equation into the solution and terms
            var splitEq = eq.Split(' ');
            var solution = long.Parse(splitEq[0].TrimEnd(':')); // Extract solution
            var terms = splitEq.Skip(1).Select(int.Parse).ToArray(); // Extract terms

            // Generate operator combinations
            var simpleOperands = GenerateOperatorCombinations(terms.Length - 1);
            var operandsWithConcatenation = GenerateOperatorCombinations(terms.Length - 1, true);

            // Check if the equation is valid for simple operators
            if (simpleOperands.Any(o => IsValidEquation(solution, terms, o)))
                sumOfValidEquations += solution;

            // Check if the equation is valid for operators including concatenation
            if (operandsWithConcatenation.Any(o => IsValidEquationWithConcat(solution, terms, o)))
                sumWithConcat += solution;
        });

        Console.WriteLine($"Sum of valid solutions (without concatenation): {sumOfValidEquations}");
        Console.WriteLine($"Sum of valid solutions (with concatenation): {sumWithConcat}");
    }

    // Validates an equation using simple operators (+, *)
    public static bool IsValidEquation(long expected, int[] terms, string operands)
    {
        long actual = terms[0];

        for (int i = 1; i < terms.Length; i++)
        {
            if (operands[i - 1] == '+')
                actual += terms[i];
            else if (operands[i - 1] == '*')
                actual *= terms[i];

            // Early exit if actual exceeds the solution
            if (actual > expected)
                return false;
        }

        return actual == expected;
    }

    // Validates an equation including concatenation 'c'
    public static bool IsValidEquationWithConcat(long expected, int[] terms, string operands)
    {
        // If no concatenation operator is present, fall back to simple validation
        if (!operands.Contains('c'))
            return IsValidEquation(expected, terms, operands);

        long actual = terms[0];

        for (int i = 1; i < terms.Length; i++)
        {
            switch (operands[i - 1])
            {
                case '+':
                    actual += terms[i];
                    break;
                case '*':
                    actual *= terms[i];
                    break;
                case 'c':
                    actual = long.Parse($"{actual}{terms[i]}");
                    break;
            }

            // Early exit if actual exceeds the solution
            if (actual > expected)
                return false;
        }

        return actual == expected;
    }

    // Recursively generates all possible operator combinations
    public static List<string> GenerateOperatorCombinations(int count, bool concat = false)
    {
        List<string> combinations = new();

        // Base case: No operators needed
        if (count < 1)
            return combinations;

        // Base case: Single operator
        if (count == 1)
        {
            combinations.Add("+");
            combinations.Add("*");
            if (concat)
                combinations.Add("c");
            return combinations;
        }

        // Recursive case: Generate combinations for smaller count
        var smallerCombinations = GenerateOperatorCombinations(count - 1, concat);

        foreach (var combo in smallerCombinations)
        {
            combinations.Add(combo + "+");
            combinations.Add(combo + "*");

            if (concat)
                combinations.Add(combo + "c");
        }

        return combinations;
    }

    // Reads equations from a file
    public static List<string> GetEquationsFromFile(string filePath) =>
        File.ReadLines(filePath).ToList();
}
