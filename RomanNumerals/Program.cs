using System;
using System.Collections.Generic;
using System.Linq;

namespace RomanNumerals
{
    /// <summary>
    /// NOTE: I only wrote the Main method and the IsRomanNumeral methods in this program.
    /// The ToArabicNumber and ToRomanNumber methods were designed by Wutz and can be found here:
    /// https://stackoverflow.com/questions/14900228/roman-numerals-to-integers
    /// </summary>
    class Program
    {
        private static bool stop = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter either an Arabic number or a Roman number and I will convert it.");
            Console.WriteLine("NOTE: The Arabic value cannot exceed 3999 and cannot be negative.");
            Console.WriteLine("Enter \"Q\" to quit.");

            while (!stop)
            {
                Console.WriteLine();
                Console.Write("Enter a value to convert: ");
                string input = Console.ReadLine();
                int arabicValue = 0;

                if (input.ToUpper().Trim() != "Q")
                {
                    Int32.TryParse(input, out arabicValue);

                    try
                    {
                        if (arabicValue > 0)
                        {
                            if (arabicValue > 3999)
                            {
                                throw new Exception(String.Format("{0} exceeds 3999.", arabicValue));
                            }

                            // Convert from Arabic to Roman
                            Console.WriteLine(String.Format("{0} = {1}", arabicValue, ToRomanNumber(arabicValue)));
                        }
                        else
                        {
                            if (input != String.Empty)
                            {
                                // Convert from Roman to Arabic
                                string romanValue = input.ToUpper().Trim();

                                if (IsRomanNumeral(romanValue))
                                {
                                    Console.WriteLine(String.Format("{0} = {1}", romanValue, ToArabicNumber(romanValue)));
                                }
                                else
                                {
                                    throw new Exception(String.Format("{0} is not a valid roman number.", input));
                                }
                            }
                            else
                            {
                                throw new Exception("A blank line cannot be converted to a value.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(String.Format("{0}", ex.Message.ToString()));
                    }
                }
                else
                {
                    stop = true;
                }
            }
        }

        /// <summary>
        /// Converts a Roman number string into a Arabic number
        /// </summary>
        /// <param name="romanNumber">The Roman number string</param>
        /// <returns>The Arabic number (0 if the given string is not convertible to a Roman number)</returns>
        private static int ToArabicNumber(string romanNumber)
        {
            string[] replaceRom = { "CM", "CD", "XC", "XL", "IX", "IV" };
            string[] replaceNum = { "DCCCC", "CCCC", "LXXXX", "XXXX", "VIIII", "IIII" };
            string[] roman = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            int[] arabic = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            
            return Enumerable.Range(0, replaceRom.Length)
                .Aggregate
                (
                    romanNumber,
                    (agg, cur) => agg.Replace(replaceRom[cur], replaceNum[cur]),
                    agg => agg.ToArray()
                )
                .Aggregate
                (
                    0,
                    (agg, cur) =>
                    {
                        int idx = Array.IndexOf(roman, cur.ToString());
                        return idx < 0 ? 0 : agg + arabic[idx];
                    },
                    agg => agg
                );
        }

        /// <summary>
        /// Converts a Arabic number into a Roman number string
        /// </summary>
        /// <param name="arabicNumber">the Arabic number</param>
        /// <returns>the Roman number string</returns>
        private static string ToRomanNumber(int arabicNumber)
        {
            string[] roman = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            int[] arabic = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            return Enumerable.Range(0, arabic.Length)
                .Aggregate
                (
                    Tuple.Create(arabicNumber, string.Empty),
                    (agg, cur) =>
                    {
                        int remainder = agg.Item1 % arabic[cur];
                        string concat = agg.Item2 + string.Concat(Enumerable.Range(0, agg.Item1 / arabic[cur]).Select(num => roman[cur]));
                        return Tuple.Create(remainder, concat);
                    },
                    agg => agg.Item2
                );
        }

        /// <summary>
        /// Loops through all of the characters in the input string to see if it contains values
        /// that are not in the seven roman numeral characters allowed.
        /// </summary>
        /// <param name="romanNumber">The roman number string</param>
        /// <returns>true if it is a roman number, otherwise false</returns>
        private static bool IsRomanNumeral(string romanNumber)
        {
            bool isRoman = true;

            HashSet<char> roman = new HashSet<char> { 'M', 'D', 'C', 'L', 'X', 'V', 'I' };

            foreach (char c in romanNumber)
            { 
                if(!roman.Contains(c))
                {
                    isRoman = false;
                    break;
                }
            }

            return isRoman;
        }
    }
}