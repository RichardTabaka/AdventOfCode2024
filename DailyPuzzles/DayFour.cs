namespace AdventOfCode;

public static class DayFour
{
    // Directions representing all possible movements (horizontal, vertical, diagonal)
    private static readonly (int Row, int Col)[] Directions = [
        (0, 1),  // Horizontal ->
        (0, -1), // Horizontal <-
        (1, 0),  // Vertical down
        (-1, 0), // Vertical up
        (1, 1),  // Diagonal down-right
        (1, -1), // Diagonal down-left
        (-1, 1), // Diagonal up-right
        (-1, -1) // Diagonal up-left
    ];

    // Main method to solve the word search
    public static void SolveWordSearch()
    {
        var wordSearch = GetWordSearchFromFile("./PuzzleInputs/DayFour.txt");
        int xmasCount = 0;

        for (int i = 0; i < wordSearch.Count; i++)
        {
            for (int j = 0; j < wordSearch[i].Length; j++)
            {
                // Check for the word "XMAS" in all possible directions
                Directions.ToList().ForEach(direction =>
                {
                    if (IsWordMatch(wordSearch, i, j, "XMAS", direction))
                        xmasCount++;
                });
            }
        }

        Console.WriteLine($"Wordsearch count: {xmasCount}");
    }

    // Checks if a word matches in a given direction starting from a cell
    private static bool IsWordMatch(
        List<string> grid,
        int startRow,
        int startCol,
        string word,
        (int Row, int Col) direction)
    {
        int rows = grid.Count;
        int cols = grid[0].Length;

        for (int k = 0; k < word.Length; k++)
        {
            int newRow = startRow + k * direction.Row;
            int newCol = startCol + k * direction.Col;

            // Check if the new position is within bounds
            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                return false;

            // Check if the character matches the expected character
            if (grid[newRow][newCol] != word[k])
                return false;
        }

        return true;
    }

    // Finds patterns where an "A" is at the center of an X shape formed by "MAS"
    public static void XMasNotXmas()
    {
        var wordSearch = GetWordSearchFromFile("./PuzzleInputs/DayFour.txt");
        int xMasCount = 0;

        // Iterate through all possible center positions for the "A"
        for (int row = 1; row < wordSearch.Count - 1; row++)
        {
            for (int col = 1; col < wordSearch[row].Length - 1; col++)
            {
                // Check if the current cell is "A" and part of a valid X shape
                if (wordSearch[row][col] == 'A' && IsXMasA(wordSearch, row, col))
                    xMasCount++;
            }
        }

        Console.WriteLine($"Wordsearch count: {xMasCount}");
    }

    // Checks if an "A" is at the center of a valid X shape formed by "MAS"
    private static bool IsXMasA(List<string> grid, int startRow, int startCol)
    {
        // Get characters in the four diagonals around the "A"
        var upRight = grid[startRow - 1][startCol + 1];
        var upLeft = grid[startRow - 1][startCol - 1];
        var downRight = grid[startRow + 1][startCol + 1];
        var downLeft = grid[startRow + 1][startCol - 1];

        // Check all combinations of diagonals to form a valid X of "MAS"
        var masUpRight = downLeft == 'M' && upRight == 'S';
        var masUpLeft = downRight == 'M' && upLeft == 'S';
        var masDownLeft = upRight == 'M' && downLeft == 'S';
        var masDownRight = upLeft == 'M' && downRight == 'S';

        // Return true if any two valid diagonals form the X shape
        return (masUpRight && masUpLeft) ||
               (masDownRight && masDownLeft) ||
               (masUpRight && masDownRight) ||
               (masDownLeft && masUpLeft);
    }

    private static List<string> GetWordSearchFromFile(string filePath) =>
        File.ReadLines(filePath).ToList();
}
