using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HexadecimalAddition
{

    class Program
    {
        public static int Radix = 16;

        private static readonly Dictionary<char, int> DictionaryValidValues = new Dictionary<char, int>
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
            ['a'] = 10,
            ['b'] = 11,
            ['c'] = 12,
            ['d'] = 13,
            ['e'] = 14,
            ['f'] = 15
        };

        private static void Main(string[] args)
        {
            const string firstHex = "bc614e";
            const string secondHex = "343efcea";
            try
            {
                if (IsHexFormat(firstHex) && IsHexFormat(secondHex))
                {
                    var simpleSolution = SimpleResolver(firstHex, secondHex);
                    var byteAdditionSolution = ByteAdditionResolver(firstHex, secondHex);
                    var cSharp89FeauturesSolution = CSharp89FeauturesResolver(firstHex, secondHex);
                    Console.WriteLine(simpleSolution);
                    Console.WriteLine(byteAdditionSolution);
                    Console.WriteLine(cSharp89FeauturesSolution);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("The parameters is not in hexadecimal format");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            Console.ReadLine();
        }

        private static bool IsHexFormat(string val)
        {
            string pattern = @"^[A-Fa-f0-9]*$";
            Match m = Regex.Match(val, pattern);
            return m.Success;
        }

        private static string SimpleResolver(string firstHex, string secondHex)
        {
            var result = Convert.ToInt32(firstHex, Radix) + Convert.ToInt32(secondHex, Radix);
            return Convert.ToString(result, Radix);
        }

        private static string ByteAdditionResolver(string firstHex, string secondHex)
        {
            var lengthFirstHex = firstHex.Length;
            var lengthSecondHex = secondHex.Length;

            // equation of strings sizes by adding zero bytes ('0') to the left part
            if (lengthFirstHex > lengthSecondHex)
            {
                secondHex = string.Concat(string.Concat(Enumerable.Repeat('0', lengthFirstHex - lengthSecondHex)), secondHex);
            }
            else if
               (lengthFirstHex < lengthSecondHex) firstHex = string.Concat(string.Concat(Enumerable.Repeat('0', lengthSecondHex - lengthFirstHex)), firstHex);

            var result = "";
            var hexLength = firstHex.Length;
            var accumulatorPreviousIteration = 0;

            for (var i = 1; i < hexLength + 1; i++)
            {
                var valueCurrentIteration = DictionaryValidValues[firstHex.ToLower()[^i]] + DictionaryValidValues[secondHex.ToLower()[^i]] + accumulatorPreviousIteration;
                if (valueCurrentIteration > Radix)
                {
                    accumulatorPreviousIteration = 1;
                    result = DictionaryValidValues.FirstOrDefault(item => item.Value == valueCurrentIteration - Radix).Key + result;
                }
                else
                {
                    accumulatorPreviousIteration = 0;
                    result = DictionaryValidValues.FirstOrDefault(item => item.Value == valueCurrentIteration).Key + result;
                }
            }

            if (accumulatorPreviousIteration == 1) result = accumulatorPreviousIteration + result;

            return result;
        }


        private static string CSharp89FeauturesResolver(string firstHex, string secondHex)
        {
            // based on ByteAdditionResolver
            // https://docs.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-9
            // add using c# 8-9 features: 1) records, 2) init 3) indexes 4) using for vars 5) is not null construction

 
            var twoHex = new RecordTwoHex(firstHex, secondHex);

            // equation of strings sizes by adding zero bytes ('0') to the left part
            if (twoHex.FirstHexLength > twoHex.SecondHexLength)
            {
                twoHex.SecondHex = string.Concat(string.Concat(Enumerable.Repeat('0', twoHex.FirstHexLength - twoHex.SecondHexLength)), secondHex);
            }
            else if
                (twoHex.FirstHexLength < twoHex.SecondHexLength) twoHex.FirstHex = string.Concat(string.Concat(Enumerable.Repeat('0', twoHex.SecondHexLength - twoHex.FirstHexLength)), firstHex);

            var result = "";
            var accumulatorPreviousIteration = 0;

            for (var i = 1; i < twoHex.LengthRecord + 1; i++)
            {
                var valueCurrentIteration = DictionaryValidValues[twoHex.FirstHex.ToLower()[^i]] + DictionaryValidValues[twoHex.SecondHex.ToLower()[^i]] + accumulatorPreviousIteration;
                if (valueCurrentIteration > Radix)
                {
                    accumulatorPreviousIteration = 1;
                    result = DictionaryValidValues.FirstOrDefault(item => item.Value == valueCurrentIteration - Radix).Key + result;
                }
                else
                {
                    accumulatorPreviousIteration = 0;
                    result = DictionaryValidValues.FirstOrDefault(item => item.Value == valueCurrentIteration).Key + result;
                }
            }

            if (accumulatorPreviousIteration == 1) result = accumulatorPreviousIteration + result;

            using (var file = new System.IO.StreamWriter("CSharp89FeauturesWorkResult.txt"))
            {
                //  is not null construction
                if (result is not null)
                {
                    file.WriteLine(result);
                }
            }

            return result;
        }

    }

    # region internal classes
    public record RecordTwoHex
    {
        public string FirstHex { get; set; }
        public string SecondHex { get; set; }
        internal int FirstHexLength { get; init; }
        internal int SecondHexLength { get; init; }
        public int LengthRecord { get; set; }

        public RecordTwoHex(string first, string second)
        {
            FirstHex = first;
            SecondHex = second;
            FirstHexLength = FirstHex.Length;
            SecondHexLength = SecondHex.Length;
            LengthRecord = Math.Max(FirstHexLength, SecondHexLength);
        }
    }
    #endregion

}
