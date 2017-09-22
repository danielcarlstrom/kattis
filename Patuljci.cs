using System;
using System.Collections.Generic;
using System.Linq;

namespace task1
{
    class Program
    {
        static void Main()
        {
            var testNumbers = new List<int>();
            for (var i = 0; i < 9; i++)
            {
                testNumbers.Add(Int32.Parse(Console.ReadLine()));
            }
            CalcTests(testNumbers.ToArray());
        }

        private static void CalcTests(int[] numbers)
        {
            var diff = numbers.Sum() - 100;

            foreach (var number1 in numbers)
            {
                foreach (var number2 in numbers)
                {
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
                {
                    continue;
                }
                Console.WriteLine(number);
            }
        }
    }
}
