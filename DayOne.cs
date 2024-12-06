namespace AdventOfCode;
public class DayOne {
    public static void FindTotalDistance() {
        var teamLists = GetTeamListsFromFile("./PuzzleInputs/DayOne.txt");
        var teamOneResults = teamLists[0];
        var teamTwoResults = teamLists[1];
        
        teamOneResults.Sort();
        teamTwoResults.Sort();
        int distance = 0;
        for (int i = 0; i < teamOneResults.Count; i++) {
            distance += Math.Abs(teamOneResults[i] - teamTwoResults[i]);
        }

        System.Console.WriteLine($"Total distance between teams: {distance}");
    }

    public static void GetTeamSimilarityScores() {
        var teamLists = GetTeamListsFromFile("./PuzzleInputs/DayOne.txt");
        var teamOneResults = teamLists[0];
        var teamTwoResults = teamLists[1];

        int score = 0;
        teamOneResults.ForEach(r => {
            score += teamTwoResults.Where(x => x == r).Count() * r;
        });
        System.Console.WriteLine($"Similarity score: {score}");
    }

    private static List<int>[] GetTeamListsFromFile(string filePath) {
        var teamOneResults = new List<int>();
        var teamTwoResults = new List<int>();
        
        using (StreamReader input = new StreamReader(filePath)) {
            string? line;
            while ((line = input.ReadLine()) != null) {
                var splitLine = line.Split("   ");
                teamOneResults.Add(Int32.Parse(splitLine[0]));
                teamTwoResults.Add(Int32.Parse(splitLine[1]));
            }
        }

        return [teamOneResults, teamTwoResults];
    }
}