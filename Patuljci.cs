using System;
using System.Collections.Generic;
using System.Linq;

namespace task1
{
    internal class Program
    {
        private static void Main()
        {
            var testNumbers = new List<int>();
            for (var i = 0; i < 9; i++)
                testNumbers.Add(int.Parse(Console.ReadLine()));
            CalcTests(testNumbers.ToArray());
        }

        private static void CalcTests(int[] numbers)
        {
            var diff = numbers.Sum() - 100;

            for (var i = 0; i < numbers.Length; i++)
            {
                var number1 = numbers[i];
                for (var j = 1; j < numbers.Length; j++)
                {
                    var number2 = numbers[j];
                    if (number1 + number2 == diff)
                    {
                        NumbersToOutput(number1, number2, numbers);
                        return;
                    }
                }
            }
        }

        private static void NumbersToOutput(int i, int j, int[] numbers)
        {
            foreach (var number in numbers)
            {
                if (number == i || number == j)
                    continue;
                Console.WriteLine(number);
            }
        }
    }
}
