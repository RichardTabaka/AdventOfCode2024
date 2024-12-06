namespace AdventOfCode;

public static class DayTwo
{
    // Method to calculate and print the count of safe reports from the input file
    public static void GetSafeReportCount()
    {
        var reports = GetReportsFromFile("./PuzzleInputs/DayTwo.txt");

        // Count the number of reports that pass the CheckReport method
        int safeReports = reports.Count(report => CheckReport(report, out _));

        // Print the total count of safe reports
        Console.WriteLine($"Safe report count: {safeReports}");
    }

    // Method to calculate and print the count of safe reports with the use of a dampener
    public static void GetSafeReportWithDampenerCount()
    {
        var reports = GetReportsFromFile("./PuzzleInputs/DayTwo.txt");
        int safeReports = 0;

        foreach (var report in reports)
        {
            // Check if the report is initially safe
            if (CheckReport(report, out int failedPos))
            {
                safeReports++;
            }
            else
            {
                // Try removing elements around the failed position to make it safe
                for (int offset = -1; offset <= 1; offset++)
                {
                    if (failedPos + offset >= 0 && failedPos + offset < report.Count)
                    {
                        // Create a copy of the report with one element removed
                        var dampenedReport = new List<int>(report);
                        dampenedReport.RemoveAt(failedPos + offset);

                        // Check if the modified report is safe
                        if (CheckReport(dampenedReport, out _))
                        {
                            safeReports++;
                            break; // Exit the loop if the report becomes safe
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Safe report count with Dampener: {safeReports}");
    }

    // Method to validate if a report is safe and identify the first failing position
    public static bool CheckReport(List<int> report, out int failedPosition)
    {
        failedPosition = -1;
        bool? increasing = null;

        for (int i = 0; i < report.Count - 1; i++)
        {
            var difference = report[i + 1] - report[i];

            // Check if the difference is out of the allowed range (1 to 3 inclusive)
            if (Math.Abs(difference) < 1 || Math.Abs(difference) > 3)
            {
                failedPosition = i;
                return false;
            }

            // Determine if the sequence is increasing or decreasing
            if (increasing == null)
            {
                increasing = difference > 0;
            }

            // Check if the sequence violates the increasing/decreasing rule
            if (increasing != (difference > 0))
            {
                failedPosition = i;
                return false;
            }
        }

        return true;
    }

    // Method to read and parse reports from a file
    private static List<List<int>> GetReportsFromFile(string filePath)
    {
        return File.ReadLines(filePath)
            .Select(line => line.Split(" ")
                                .Select(int.Parse)
                                .ToList())
            .ToList();
    }
}
