using System;
using System.Collections.Generic;
using System.Linq;

namespace BuyCoke
{
    internal class Program
    {
        private static void Main()
        {
            #region Kattis testdata

            var tests = Convert.ToInt32(Console.ReadLine());

            var testsList = new List<List<int>>();

            for (var i = 0; i < tests; i++)
            {
                var testLine = Console.ReadLine();
                var stringTests = testLine.Split(' ').ToList();
                var intTests = new List<int>();

                foreach (var test in stringTests)
                    intTests.Add(Convert.ToInt32(test));
                testsList.Add(intTests);
            }

            foreach (var testList in testsList)
                CalcTestCase(testList.ToArray());

            #endregion
        }

        private static void CalcTestCase(int[] testData)
        {
            var numberOfCokes = testData[0];

            //get all permutations of "ones", "fives" and "tens" in order to test all combinations of input to the machine
            var spendRules = GetSpendRules();
            var lowestList = new HashSet<int>();

            foreach (var spendRule in spendRules)
            {
                var wallet = new Wallet(testData[1], testData[2], testData[3]);
                CalculateMinimumCoins(numberOfCokes, wallet, spendRule);
                lowestList.Add(wallet.CoinsSpentThisRound);
                wallet.CoinsSpentThisRound = 0;
            }
            var min = lowestList.Min();
            Console.WriteLine(min);
        }

        private static void CalculateMinimumCoins(int numberOfCokes, Wallet wallet, string[] spendRule)
        {
            //here we decide what coin we want to test first
            while (numberOfCokes > 0)
                foreach (var coinValue in spendRule)
                    switch (coinValue)
                    {
                        case "ones":
                        {
                            while (wallet.Ones >= 8)
                            {
                                wallet.Ones -= 8;
                                wallet.CoinsSpentThisRound += 8;
                                numberOfCokes--;
                                if (numberOfCokes == 0)
                                    return;
                            }
                            break;
                        }
                        case "fives":
                        {
                            while (wallet.Fives >= 2)
                            {
                                wallet.Fives -= 2;
                                wallet.Ones += 2;
                                wallet.CoinsSpentThisRound += 2;
                                numberOfCokes--;
                                if (numberOfCokes == 0)
                                    return;
                            }
                            break;
                        }
                        case "tens":
                        {
                            while (wallet.Tens >= 1)
                            {
                                wallet.Tens -= 1;
                                wallet.Ones += 2;
                                wallet.CoinsSpentThisRound += 1;
                                numberOfCokes--;
                                if (numberOfCokes == 0)
                                    return;
                            }
                            break;
                        }
                        case "onesFives":
                        {
                            while (wallet.Fives >= 1 && wallet.Ones >= 3)
                            {
                                wallet.Ones -= 3;
                                wallet.Fives -= 1;
                                wallet.CoinsSpentThisRound += 4;
                                numberOfCokes--;
                                if (numberOfCokes == 0)
                                    return;
                            }
                            break;
                        }
                    }
        }

        #region save for later

        private static void DistributeFunds(int newFunds, Wallet wallet)
        {
            wallet.Ones = 0;
            wallet.Fives = 0;
            wallet.Tens = 0;

            while (newFunds / 10 >= 1)
            {
                newFunds -= 10;
                wallet.Tens += 1;
            }
            while (newFunds / 5 >= 1)
            {
                newFunds -= 5;
                wallet.Fives += 1;
            }
            wallet.Ones += newFunds;
        }

        #endregion

        #region Help methods

        public static void SwapNumbers(ref string numberOne, ref string numberTwo)
        {
            var temp = numberOne;
            numberOne = numberTwo;
            numberTwo = temp;
        }

        public static void PermuteArray(string[] inputArray, int currentPosition, int endPosition, ref HashSet<string[]> coinOrders)
        {
            int i;
            if (currentPosition == endPosition)
            {
                var coinOrderVariant = new string[4];
                for (i = 0; i <= endPosition; i++)
                    coinOrderVariant[i] = inputArray[i];
                coinOrders.Add(coinOrderVariant);
            }
            else
            {
                for (i = currentPosition; i <= endPosition; i++)
                {
                    SwapNumbers(ref inputArray[currentPosition], ref inputArray[i]);
                    
                    //recursive call with +1 on currentPosition to step through the array
                    PermuteArray(inputArray, currentPosition + 1, endPosition, ref coinOrders);
                    SwapNumbers(ref inputArray[currentPosition], ref inputArray[i]);
                }
            }
        }

        public static HashSet<string[]> GetSpendRules()
        {
            var inputArray = new[] {"ones", "fives", "tens", "onesFives"};

            var coinOrders = new HashSet<string[]>();
            PermuteArray(inputArray, 0, inputArray.Length - 1, ref coinOrders);

            return coinOrders;
        }

        #endregion
    }

    internal class Wallet
    {
        public Wallet(int ones, int fives, int tens)
        {
            Ones = ones;
            Fives = fives;
            Tens = tens;
        }

        public int Ones { get; set; }
        public int Fives { get; set; }
        public int Tens { get; set; }

        public int CoinsSpentThisRound { get; set; }

        public int GetTotalFunds()
        {
            return Ones + Fives * 5 + Tens * 10;
        }
    }
}
