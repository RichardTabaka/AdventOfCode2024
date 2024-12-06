using System.Diagnostics;
using AdventOfCode;

Console.WriteLine("Welcome to my Advent Of Code 2024 Program!");
Console.Write("Enter a day number (1-25) to run that day's code: ");
int day;
if (Int32.TryParse(Console.ReadLine(), out day))
{
    switch (day)
    {
        case 1:
            DayOne.FindTotalDistance();
            DayOne.GetTeamSimilarityScores();
            break;
        case 2:
            DayTwo.GetSafeReportCount();
            DayTwo.GetSafeReportWithDampenerCount();
            break;
        case 3:
            DayThree.AnalyzeCorruptedCode();
            DayThree.AnalyzeCorruptedCodeWithDisables();
            break;
        default:
            Console.WriteLine("Sorry, I haven't got there yet!");
            break;
    }
}
