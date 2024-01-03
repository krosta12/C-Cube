using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RubiksCubeSimulator.Rubiks
{
    internal class FileSorter
    {
        public static void SortFileByDigits(string inputFileName, string outputFileName, int batchSize = 10000)
        {
            using (var inputFileStream = new StreamReader(inputFileName))
            using (var outputFileStream = new StreamWriter(outputFileName))
            {
                List<string> batchLines = new List<string>();

                while (!inputFileStream.EndOfStream)
                {
                    string line = inputFileStream.ReadLine();
                    batchLines.Add(line);

                    if (batchLines.Count >= batchSize) { ProcessAndWriteBatch(batchLines, outputFileStream); batchLines.Clear(); }
                }

                if (batchLines.Count > 0) { ProcessAndWriteBatch(batchLines, outputFileStream); }
            }
        }

        private static void ProcessAndWriteBatch(List<string> batchLines, StreamWriter outputFileStream)
        {
            var sortedLines = batchLines
                .Select(line => new
                {
                    Line = line,
                    Digits = GetDigitsFromEnd(line)
                })
                .OrderBy(item => item.Digits)
                .Select(item => item.Line);

            foreach (var sortedLine in sortedLines) { outputFileStream.WriteLine(sortedLine); }
        }

        private static int GetDigitsFromEnd(string text)
        {
            string digitsString = new string(text.Reverse().TakeWhile(char.IsDigit).Reverse().ToArray());

            if (int.TryParse(digitsString, out int result)) { return result; }
            else { return 0; }
        }
    }
}