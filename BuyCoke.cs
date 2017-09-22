using System;
using System.Collections.Generic;
using System.Linq;

namespace task3
{
    class Program
    {
        static void Main()
        {
            var tests = Convert.ToInt32(Console.ReadLine());

            var testsList = new List<List<int>>();

            for (var i = 0; i < tests; i++)
            {
                var testLine = Console.ReadLine();
                var stringTests = testLine.Split(' ').ToList();
                var intTests = new List<int>();

                foreach (var test in stringTests)
                {
                    intTests.Add(Convert.ToInt32(test));
                }
                testsList.Add(intTests);
            }

            foreach (var testList in testsList)
            {
                CalcTestCase(testList.ToArray());
            }
        }

        private static void CalcTestCase(int[] testData)
        {
            var numberOfCokes = testData[0];
            var wallet = new Wallet(testData[1], testData[2], testData[3]);
            CalculateMinimumCoins(numberOfCokes, wallet);
        }

        private static void CalculateMinimumCoins(int numberOfCokes, Wallet wallet)
        {
            if (numberOfCokes == 0)
            {
                Console.WriteLine(wallet.CoinsSpent);
                return;
            }

            if (wallet.Ettor + wallet.Femmor * 5 < 8 || numberOfCokes <= wallet.Tior)
            {
                wallet.Tior = wallet.Tior - 1;
                wallet.Ettor = wallet.Ettor + 2;
                wallet.CoinsSpent = wallet.CoinsSpent + 1;
                CalculateMinimumCoins(numberOfCokes - 1, wallet);
            }
            else if (numberOfCokes <= wallet.Femmor * 5 / 10)
            {
                wallet.Femmor = wallet.Femmor - 2;
                wallet.Ettor = wallet.Ettor + 2;
                wallet.CoinsSpent = wallet.CoinsSpent + 2;
                CalculateMinimumCoins(numberOfCokes - 1, wallet);
            }
            else if (wallet.Femmor > 0 && wallet.Ettor >= 3)
            {
                wallet.Femmor = wallet.Femmor - 1;
                wallet.Ettor = wallet.Ettor - 3;
                wallet.CoinsSpent = wallet.CoinsSpent + 4;
                CalculateMinimumCoins(numberOfCokes - 1, wallet);
            }
            else if (wallet.Femmor > 0 && wallet.Tior > 0)
            {
                wallet.Tior = wallet.Tior - 1;
                wallet.Femmor = wallet.Femmor - 1;
                wallet.Ettor = wallet.Ettor + 3;
                wallet.CoinsSpent = wallet.CoinsSpent + 2;
                CalculateMinimumCoins(numberOfCokes - 1, wallet);
            }
            else
            {
                wallet.Ettor = wallet.Ettor - 8;
                wallet.CoinsSpent = wallet.CoinsSpent + 8;
                CalculateMinimumCoins(numberOfCokes - 1, wallet);
            }
        }
    }

    class Wallet
    {
        public int Ettor { get; set; }
        public int Femmor { get; set; }
        public int Tior { get; set; }
        public int CoinsSpent { get; set; }

        public Wallet(int ettor, int femmor, int tior)
        {
            Ettor = ettor;
            Femmor = femmor;
            Tior = tior;
        }
    }
}
