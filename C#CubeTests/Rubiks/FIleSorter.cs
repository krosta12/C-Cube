using System.IO;
using System.Linq;

namespace RubiksCubeSimulator.Rubiks
{
    internal class FileSorter
    {
        public static void SortFileByDigits(string inputFileName, string outputFileName)
        {
            string[] lines = File.ReadAllLines(inputFileName);

            var sortedLines = lines
                .Select(line => new
                {
                    Line = line,
                    Digits = GetDigitsFromEnd(line)
                })
                .OrderBy(item => item.Digits)
                .Select(item => item.Line);

            File.WriteAllLines(outputFileName, sortedLines);
        }

        private static int GetDigitsFromEnd(string text)
        {
            string digitsString = new string(text.Reverse().TakeWhile(char.IsDigit).Reverse().ToArray());

            if (int.TryParse(digitsString, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}