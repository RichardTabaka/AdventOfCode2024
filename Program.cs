﻿using AdventOfCode;

Console.WriteLine("Welcome to my Advent Of Code 2024 Program!");
Console.Write("Enter a day number (1-25) to run that day's code: ");
if (int.TryParse(Console.ReadLine(), out int day))
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
        case 4:
            DayFour.SolveWordSearch();
            DayFour.XMasNotXmas();
            break;
        case 5:
            DayFive.CheckManualPageOrders();
            break;
        case 6:
            DaySix.PredictPatrolRoute();
            break;
        default:
            Console.WriteLine("Sorry, I haven't got there yet!");
            break;
    }
}
