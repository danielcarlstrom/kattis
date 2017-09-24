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
                testNumbers.Add(Convert.ToInt32(Console.ReadLine()));
            CalcTests(testNumbers.ToArray());
        }

        private static void CalcTests(int[] numbers)
        {
            var diff = numbers.Sum() - 100;

            for (var i = 0; i < numbers.Length; i++)
            {
                for (var j = 0; j < numbers.Length; j++)
                {
                    if (numbers[i] == numbers[j])
                    {
                        continue;
                    }
                    if (numbers[i] + numbers[j] == diff)
                    {
                        NumbersToOutput(numbers[i], numbers[j], numbers);
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
