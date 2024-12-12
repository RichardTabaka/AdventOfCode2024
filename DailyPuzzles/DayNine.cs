namespace AdventOfCode;

public static class DayNine
{
    public static void CompactDisk()
    {
        var diskMap = GetDiskMapFromFile("./PuzzleInputs/DayNine.txt");
        var diskBlocks = ConvertMapToBlocks(diskMap!);
        var sortedDisk = SortDiskBlocks(diskBlocks);

        System.Console.WriteLine(CalculateDiskCheckSum(sortedDisk));
        var newSort = SortDiskWholeBlocks(diskBlocks);
        System.Console.WriteLine(CalculateDiskCheckSum(newSort));

    }

    public static long CalculateDiskCheckSum(int[] disk)
    {
        long sum = 0;
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i] != -1)
                sum += i * int.Parse($"{disk[i]}");
        }
        return sum;
    }

    public static int[] ConvertMapToBlocks(string map)
    {
        List<int> blocks = [];
        int currentNumber = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (i % 2 == 0)
            {
                blocks.AddRange(Enumerable.Repeat(currentNumber++, int.Parse($"{map[i]}")));
            }
            else
            {
                blocks.AddRange(Enumerable.Repeat(-1, int.Parse($"{map[i]}")));
            }
        }

        return blocks.ToArray();
    }

    public static int[] SortDiskBlocks(int[] blocks)
    {
        var sorted = (int[])blocks.Clone();
        int i = 0;
        int j = sorted.Length - 1;


        while (i < j)
        {
            if (sorted[i] != -1)
                i++;
            else if (sorted[j] == -1)
                j--;
            else
            {
                var temp = sorted[i];
                sorted[i] = sorted[j];
                sorted[j] = temp;
            }
        }

        return sorted;
    }

    public static int[] SortDiskWholeBlocks(int[] blocks)
    {
        var openSpaces = new List<(int start, int len)>();
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == -1)
            {
                int spaceLen = 0;
                for (int j = i; j < blocks.Length && blocks[j] == -1; j++)
                {
                    spaceLen++;
                }
                openSpaces.Add((i, spaceLen));
                i += spaceLen;

            }
        }

        var sorted = (int[])blocks.Clone();
        int fileId = int.MaxValue;
        for (int i = sorted.Length - 1; i > 0; i--)
        {
            if (sorted[i] != -1)
            {
                // get fileLength
                int fileLength = 0;
                fileId = sorted[i];
                for (int j = i; j >= 0 && sorted[j] == fileId; j--)
                {
                    fileLength++;
                }
                if (openSpaces.Any(sp => i > sp.start && sp.len >= fileLength))
                {
                    // replace file with empty space
                    for (int j = 0; j < fileLength; j++)
                    {
                        sorted[i - j] = -1;
                    }

                    // pick best empty space
                    var space = openSpaces
                        .Where(sp => sp.len >= fileLength)
                        .OrderBy(sp => sp.start)
                        .First();

                    // replace empty space with file
                    for (int j = 0; j < fileLength; j++)
                    {
                        sorted[space.start + j] = fileId;
                    }

                    // remove or modify the filled space
                    if (space.len > fileLength)
                        openSpaces.Add((
                            space.start + fileLength,
                            space.len - fileLength));
                    openSpaces.Remove(space);


                }
                i -= fileLength - 1;
            }
        }

        return sorted;
    }

    public static string? GetDiskMapFromFile(string filePath) =>
        File.ReadLines(filePath).SingleOrDefault();
}
