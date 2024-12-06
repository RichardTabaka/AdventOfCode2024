using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class DayThree
{
    // Method to analyze and process the corrupted code for valid multiplication commands
    public static void AnalyzeCorruptedCode(string? input = null)
    {
        // If no input is provided, read it from the specified file
        input ??= GetCodeFromFile("./PuzzleInputs/DayThree.txt");

        // Regex pattern to match valid multiplication commands in the format mul(num1,num2)
        var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

        // Find all matches of the regex in the input string
        var matches = regex.Matches(input);

        // Calculate the result by summing up the products of matched numbers
        var result = matches.Sum(match =>
        {
            int num1 = int.Parse(match.Groups[1].Value);
            int num2 = int.Parse(match.Groups[2].Value);
            return num1 * num2;
        });

        Console.WriteLine($"Multiplication result: {result}");
    }

    // Method to analyze the corrupted code while removing disabled command blocks
    public static void AnalyzeCorruptedCodeWithDisables()
    {
        var fileContents = GetCodeFromFile("./PuzzleInputs/DayThree.txt");

        // Regex to identify and remove sections between "don't()" and "do()"
        var regex = new Regex(@"don't\(\).*?do\(\)");

        // Remove the disabled command blocks from the file contents
        var cleanedString = string.Join("", regex.Split(fileContents));

        // Analyze the cleaned string for valid multiplication commands
        AnalyzeCorruptedCode(cleanedString);
    }

    // Helper method to read the corrupted code from a file
    private static string GetCodeFromFile(string filePath)
    {
        var builder = new StringBuilder();

        using (var input = new StreamReader(filePath))
        {
            string? line;
            while ((line = input.ReadLine()) != null)
            {
                builder.Append(line);
            }
        }

        return builder.ToString();
    }
}
