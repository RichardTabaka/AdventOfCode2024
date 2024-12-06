namespace AdventOfCode;

public static class DayOne
{
    // Method to calculate and display the total distance between two teams
    public static void FindTotalDistance()
    {
        var (teamOneResults, teamTwoResults) = GetTeamListsFromFile("./PuzzleInputs/DayOne.txt");

        // Sort both team results for comparison
        teamOneResults.Sort();
        teamTwoResults.Sort();

        // Calculate the total distance as the sum of absolute differences between corresponding team scores
        int distance = teamOneResults
            .Select((result, index) => Math.Abs(result - teamTwoResults[index]))
            .Sum();

        Console.WriteLine($"Total distance between teams: {distance}");
    }

    // Method to calculate and display the similarity score between two teams
    public static void GetTeamSimilarityScores()
    {
        var (teamOneResults, teamTwoResults) = GetTeamListsFromFile("./PuzzleInputs/DayOne.txt");

        // Calculate the similarity score based on matching scores multiplied by their values
        int score = teamOneResults
            .Sum(r => teamTwoResults.Count(x => x == r) * r);

        Console.WriteLine($"Similarity score: {score}");
    }

    // Helper method to read and parse the input file into two lists of integers
    private static (List<int> TeamOneResults, List<int> TeamTwoResults) GetTeamListsFromFile(string filePath)
    {
        var teamOneResults = new List<int>();
        var teamTwoResults = new List<int>();

        foreach (var line in File.ReadLines(filePath))
        {
            var splitLine = line.Split("   ");
            teamOneResults.Add(int.Parse(splitLine[0]));
            teamTwoResults.Add(int.Parse(splitLine[1]));
        }

        // Return the results as a tuple
        return (teamOneResults, teamTwoResults);
    }
}
