using System;
using System.Collections.Generic;
using System.Linq;

namespace task3
{
    class Program
    {
        static void Main()
        {
            int tests = Int32.Parse(Console.ReadLine());

            var testsList = new List<List<int>>();

            for (var i = 0; i < tests; i++)
            {
                var testLine = Console.ReadLine();
                var stringTests = testLine.Split(' ').ToList();
                var intTests = new List<int>();

                foreach (var test in stringTests)
                {
                    intTests.Add(Int32.Parse(test));
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
            var wallet = new Wallet(testData[1], testData[2], testData[3]); //skapa en plånbok och ladda den med cash
            var minimumCoinsSpent = new List<int>(); //lista med alla olika output givet coinOrder
            var coinOrder = CalcCoinOrderCombinations();
            
            foreach (var position in coinOrder) //alla tänkbara ordningsföljder för betalningar
            {
                wallet.Reset();
                minimumCoinsSpent.Add(CalculateMinimumCoins(numberOfCokes, wallet, position));
            }
            Console.WriteLine(minimumCoinsSpent.ToArray().Min());
        }

        private static int CalculateMinimumCoins(int numberOfCokes, Wallet wallet, string position)
        {
            while (numberOfCokes > 0) //loopa bakåt över antal cokes vi köper
            {
                var vendor = new VendorMachine();
                vendor.BuyOneCoke(wallet, position); //köp en cola
                numberOfCokes--;
            }
            wallet.ComboUsed = position;
            return wallet.CoinsSpent;
        }

        private static HashSet<string> CalcCoinOrderCombinations()
        {
            var coinOrder = new HashSet<string>
            {
                "1234","1243","1324","1342","1423","1432","2134","2143","2314","2341","2413","2431",
                "3124","3142","3214","3241","3412","3421","4123","4132","4213","4231","4312","4321"
            };
            return coinOrder;
        }
    }
        
    internal class VendorMachine
    {
        public void BuyOneCoke(Wallet wallet, string position)
        {
            var stopSpending = false; //to prevent entering more than one purchase path
            foreach (char pos in position)
            {
                switch (pos)
                {
                    case '1':
                    {
                        if (wallet.Tior >= 1 && !stopSpending)
                        {
                            //vi har tillräckligt med tior, så vi kommer spendera en tia denna körning
                            wallet.CoinsSpent = wallet.CoinsSpent + 1;
                            wallet.Tior = wallet.Tior - 1;
                            wallet.Ettor = wallet.Ettor + 2;
                            stopSpending = true;
                        }
                        break;
                    }
                    case '2':
                    {
                        if (wallet.Femmor >= 2 && !stopSpending)
                        {
                            //vi har tillräckligt med femmor, så vi kommer spendera två femmor denna körning
                            wallet.CoinsSpent = wallet.CoinsSpent + 2;
                            wallet.Femmor = wallet.Femmor - 2;
                            wallet.Ettor = wallet.Ettor + 2;
                            stopSpending = true;
                        }
                        break;
                    }
                    case '3':
                    {
                        if (wallet.Femmor >= 1 && wallet.Ettor >= 3 && !stopSpending)
                        {
                            //vi har tillräckligt med femmor tillsammans med minst tre ettor, så vi kommer spendera en femma och tre ettor denna körning
                            wallet.CoinsSpent = wallet.CoinsSpent + 4;
                            wallet.Femmor = wallet.Femmor - 1;
                            wallet.Ettor = wallet.Ettor - 3;
                            stopSpending = true;
                        }
                        break;
                    }
                    case '4':
                    {
                        if (wallet.Ettor >= 8 && !stopSpending)
                        {
                            //vi har tillräckligt ettor, så vi kommer spendera åtta ettor denna körning
                            wallet.CoinsSpent = wallet.CoinsSpent + 8;
                            wallet.Ettor = wallet.Ettor - 8;
                            stopSpending = true;
                        }
                        break;
                    }
                }
            }
        }
    }

    class Wallet
    {
        public int StartingEttor { get; set; }
        public int StartingFemmor { get; set; }
        public int StartingTior { get; set; }
        public int Ettor { get; set; }
        public int Femmor { get; set; }
        public int Tior { get; set; }
        public int CoinsSpent { get; set; }
        public string ComboUsed { get; set; }

        public Wallet(int ettor, int femmor, int tior)
        {
            Ettor = ettor;
            StartingEttor = ettor;
            Femmor = femmor;
            StartingFemmor = femmor;
            Tior = tior;
            StartingTior = tior;
        }
        
        public void Reset()
        {
            Ettor = StartingEttor;
            Femmor = StartingFemmor;
            Tior = StartingTior;
            CoinsSpent = 0;
        }
    }
}
