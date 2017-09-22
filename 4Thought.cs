using System;
using System.Collections.Generic;

namespace task2
{
    class Program
    {
        static void Main()
        {
            var tests = Int32.Parse(Console.ReadLine());
            var testsList = new List<int>();
            for (var i = 0; i < tests; i++)
            {
                testsList.Add(Int32.Parse(Console.ReadLine()));
            }

            //generera upp alla kombinationer av operatorerna "+ - * /"
            var possibleOperatorCombinations = CalcOperatorCombinations();

            //ta fram alla tänkbara strängar utifrån kombinationer av operatorer och givna "4 4 4 4"
            var possibleCalcCombinations = PrepareCalcCombinatons(possibleOperatorCombinations);

            //räkna summorna för alla strängkombinationer och lägg dem i en dictionary tillsammans med strängen, för output
            var possibleSums = CalcPossibleSums(possibleCalcCombinations);

            foreach (var test in testsList)
            {
                if (possibleSums.ContainsKey(test))
                {
                    //yay!
                    Console.WriteLine(possibleSums[test] + " = " + test);
                }
                else
                {
                    //oh no :/
                    Console.WriteLine("no solution");
                }
            }
        }

        private static Dictionary<int, string> CalcPossibleSums(List<string> possibleCalcCombinatons)
        {
            var sums = new Dictionary<int, string>(); ;
            foreach (var possibleCalcCombinaton in possibleCalcCombinatons)
            {
                var sum = InfixToPostfixCalculation(possibleCalcCombinaton);

                //vi vill ha heltal, alltså måste "4/4/4/4" bli 0
                var floorDoubleSum = Math.Floor(sum);
                if (sums.ContainsKey((int)floorDoubleSum))
                    continue;
                sums.Add((int)floorDoubleSum, possibleCalcCombinaton);
            }
            return sums;
        }

        private static double InfixToPostfixCalculation(string possibleCalcCombinaton)
        {
            var outputList = new List<string>();
            var operatorStack = new Stack<string>();

            foreach (var current in possibleCalcCombinaton.Replace(" ", String.Empty))
            {
                if (Char.IsNumber(current))
                {
                    outputList.Add(current.ToString());
                }
                else
                {
                    //kolla om stacken är tom
                    if (operatorStack.Count > 0)
                    {
                        PeekIsSameOrHigherPrecedence(current, operatorStack.Peek(), outputList, operatorStack);
                    }
                    else
                    {
                        //stacken är tom så lägg till en operator
                        operatorStack.Push(current.ToString());
                    }
                }
            }
            //klar med uppdelningen, lägg på resterande operatorer
            foreach (var op in operatorStack)
            {
                outputList.Add(op);
            }
            var outputListString = "";
            foreach (var s in outputList)
            {
                outputListString += s;
            }

            return CalcPostfix(outputListString);
        }

        private static double CalcPostfix(string outputList)
        {
            double val = 0;
            var operandStack = new Stack<int>();

            foreach (var current in outputList)
            {
                if (Char.IsNumber(current))
                {
                    operandStack.Push((int)char.GetNumericValue(current));
                }
                else
                {
                    //beräkna top två i stacken med nuvarande operator
                    val = TakeTwoAndCalc(current, operandStack.Pop(), operandStack.Pop(), operandStack);
                }
            }
            return val;
        }

        private static double TakeTwoAndCalc(char current, int topOperand, int secondOperand, Stack<int> operandStack)
        {
            double val = 0;
            switch (current)
            {
                case '+':
                    {
                        if (operandStack.Count == 0)
                        {
                            val = secondOperand + topOperand;
                        }
                        operandStack.Push(secondOperand + topOperand);
                        break;
                    }
                case '-':
                    {
                        if (operandStack.Count == 0)
                        {
                            val = secondOperand - topOperand;
                        }
                        operandStack.Push(secondOperand - topOperand);
                        break;
                    }
                case '*':
                    {
                        if (operandStack.Count == 0)
                        {
                            val = secondOperand * topOperand;
                        }
                        operandStack.Push(secondOperand * topOperand);
                        break;
                    }
                case '/':
                    {
                        if (operandStack.Count == 0)
                        {
                            val = (double)secondOperand / topOperand;
                        }
                        operandStack.Push(secondOperand / topOperand);
                        break;
                    }
            }
            return val;
        }

        private static void PeekIsSameOrHigherPrecedence(char current, string peek, List<string> outputList, Stack<string> operatorStack)
        {
            switch (peek)
            {
                case "*":
                case "/":
                    {
                        //nu poppar vi stacken och pushar på nya operatorn
                        outputList.Add(operatorStack.Pop());
                        operatorStack.Push(current.ToString());
                        return;
                    }
                case "-":
                case "+":
                    {
                        switch (current)
                        {
                            case '-':
                            case '+':
                                {
                                    if (operatorStack.Count > 0)
                                    {
                                        //nu poppar vi stacken och pushar nya operatorn
                                        outputList.Add(operatorStack.Pop());
                                        //rekursivt, kolla om vi ska adda en till stacken eller inte
                                        PeekIsSameOrHigherPrecedence(current, peek, outputList, operatorStack);
                                        return;
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }

            //toppen av stacken < current i precedence så lägg den på stacken
            operatorStack.Push(current.ToString());
        }

        static private List<string> PrepareCalcCombinatons(HashSet<string> possibleOperatorCombinations)
        {
            var combinations = new List<string>();
            foreach (var opc in possibleOperatorCombinations)
            {
                var calc = "4";
                foreach (var c in opc)
                {
                    switch (c)
                    {
                        case '+':
                            calc += " + 4";
                            break;
                        case '-':
                            calc += " - 4";
                            break;
                        case '*':
                            calc += " * 4";
                            break;
                        case '/':
                            calc += " / 4";
                            break;
                    }
                }

                combinations.Add(calc);
            }
            return combinations;
        }

        private static HashSet<string> CalcOperatorCombinations()
        {
            var opHashSet = new HashSet<string>();
            var operators = new string[] { "+", "-", "/", "*" };
            foreach (string op1 in operators)
            {
                foreach (string op2 in operators)
                {
                    foreach (string op3 in operators)
                    {
                        var opCombo = string.Concat(op1, op2, op3);
                        if (opHashSet.Contains(opCombo))
                        {
                            continue;
                        }
                        opHashSet.Add(opCombo);
                    }
                }
            }
            return opHashSet;
        }
    }
}
