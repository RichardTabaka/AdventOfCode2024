namespace AdventOfCode;
public class DayTwo {
    public static void GetSafeReportCount() {
        var reports = GetReportsFromFile("./PuzzleInputs/DayTwo.txt");
        int safeReports = 0;
        reports.ForEach(r => {
            if (CheckReport(r, out _))
                safeReports++;
        });

        System.Console.WriteLine($"Safe report count: {safeReports}");
    }

    public static void GetSafeReportWithDampenerCount() {
        var reports = GetReportsFromFile("./PuzzleInputs/DayTwo.txt");

        int safeReports = 0;
        int failedPos;
        reports.ForEach(r => {
            if (CheckReport(r, out failedPos))
                safeReports++;
            else {
                // first, try removing i
                List<int> dampenedReport = [.. r];
                dampenedReport.RemoveAt(failedPos);
                if (CheckReport(dampenedReport, out _))
                    safeReports++;
                else {
                    dampenedReport = [.. r];
                    dampenedReport.RemoveAt(failedPos + 1);
                    if (CheckReport(dampenedReport, out _))
                        safeReports++;
                    else if (failedPos != 0) {
                        dampenedReport = [.. r];
                        dampenedReport.RemoveAt(failedPos - 1);
                        if (CheckReport(dampenedReport, out _))
                            safeReports++;
                    }
                }
            }
        });

        System.Console.WriteLine($"Safe report count with Dampener: {safeReports}");
    }

    public static bool CheckReport(List<int> report, out int failedPosition) {
        bool increasing = true;
        failedPosition = -1;
        for (int i = 0; i < report.Count - 1; i++) {
            var difference = report[i + 1] - report[i];
            if (Math.Abs(difference) < 1 || Math.Abs(difference) > 3) {
                failedPosition = i;
                return false;
            }
            if (i == 0) {
                increasing = difference > 0;
            }
            if (increasing != difference > 0) {
                failedPosition = i;
                return false;
            }
        }
        return true;
    }

    private static List<List<int>> GetReportsFromFile(string filePath) {
        List<List<int>> reports = new();

        using (StreamReader input = new StreamReader(filePath)) {
            string? line;
            while ((line = input.ReadLine()) != null) {
                reports.Add(
                    line.Split(" ")
                    .Select(s => Int32.Parse(s)).ToList());
            }
        }

        return reports;
    }
}