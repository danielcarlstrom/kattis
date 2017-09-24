using System;
using System.Collections.Generic;

namespace FourThought
{
    internal class Program
    {
        private static void Main()
        {
            #region Kattis testdata

            var numberOfTests = Convert.ToInt32(Console.ReadLine());
            var testsList = new List<int>();
            for (var i = 0; i < numberOfTests; i++)
            {
                testsList.Add(Convert.ToInt32(Console.ReadLine()));
            }
            
            #endregion

            //generate all combinations of operators "+ - * /"
            var possibleOperatorCombinations = CalcOperatorCombinations();

            //generate all possible combinations of operators given and "4 4 4 4"
            var possibleCalcCombinations = PrepareCalcCombinatons(possibleOperatorCombinations);

            //calculate sums for all testcases and populate a dictionary with the result and the calculation used
            var possibleSums = CalcPossibleSums(possibleCalcCombinations);

            foreach (var test in testsList)
            {
                if (possibleSums.ContainsKey(test))
                    Console.WriteLine(possibleSums[test] + " = " + test);
                else
                    Console.WriteLine("no solution");
            }
        }

        private static Dictionary<int, string> CalcPossibleSums(List<string> possibleCalcCombinatons)
        {
            var sums = new Dictionary<int, string>();
            foreach (var possibleCalcCombinaton in possibleCalcCombinatons)
            {
                var sum = InfixToPostfixCalculation(possibleCalcCombinaton);

                //no fractions, so "4/4/4/4" would be 0
                var floorDoubleSum = Math.Floor(sum);
                if (sums.ContainsKey((int) floorDoubleSum))
                    continue;
                sums.Add((int) floorDoubleSum, possibleCalcCombinaton);
            }
            return sums;
        }

        private static double InfixToPostfixCalculation(string possibleCalcCombinaton)
        {
            var outputList = new List<string>();
            var operatorStack = new Stack<string>();

            foreach (var current in possibleCalcCombinaton.Replace(" ", string.Empty))
            {
                if (char.IsNumber(current))
                {
                    outputList.Add(current.ToString());
                }
                else
                {
                    if (operatorStack.Count > 0)
                        PeekIsSameOrHigherPrecedence(current, operatorStack.Peek(), outputList, operatorStack);
                    else
                        operatorStack.Push(current.ToString());
                }
            }
            //done with splitting, add the remaining operators
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
                if (char.IsNumber(current))
                    operandStack.Push((int) char.GetNumericValue(current));
                else
                    val = TakeTwoAndCalc(current, operandStack.Pop(), operandStack.Pop(), operandStack);
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
                        val = secondOperand + topOperand;
                    operandStack.Push(secondOperand + topOperand);
                    break;
                }
                case '-':
                {
                    if (operandStack.Count == 0)
                        val = secondOperand - topOperand;
                    operandStack.Push(secondOperand - topOperand);
                    break;
                }
                case '*':
                {
                    if (operandStack.Count == 0)
                        val = secondOperand * topOperand;
                    operandStack.Push(secondOperand * topOperand);
                    break;
                }
                case '/':
                {
                    if (operandStack.Count == 0)
                        val = (double) secondOperand / topOperand;
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
                    //pop the stack and push the new operator
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
                                //pop the stack and push the new operator
                                outputList.Add(operatorStack.Pop());
                                //recursive check if we should add another one
                                PeekIsSameOrHigherPrecedence(current, peek, outputList, operatorStack);
                                return;
                            }
                        }
                            break;
                    }
                }
                    break;
            }

            //the top of the stack is < current in precedence so push it to the stack
            operatorStack.Push(current.ToString());
        }

        private static List<string> PrepareCalcCombinatons(HashSet<string> possibleOperatorCombinations)
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
            var operators = new[] {"+", "-", "/", "*"};
            foreach (var op1 in operators)
            {
                foreach (var op2 in operators)
                {
                    foreach (var op3 in operators)
                    {
                        var opCombo = string.Concat(op1, op2, op3);
                        if (opHashSet.Contains(opCombo))
                            continue;
                        opHashSet.Add(opCombo);
                    }
                }
            }
            return opHashSet;
        }
    }
}
