using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ATMProblem
{
    public class MoneyMachine
    {
        public Dictionary<DollarType, int> DollarStorage;

        public MoneyMachine() => RestockStorage(false);

        public void RestockStorage(bool print = true)
        {
            DollarStorage = new Dictionary<DollarType, int>()
            {
                [DollarType.OneHundred] = 10,
                [DollarType.Fifty] = 10,
                [DollarType.Twenty] = 10,
                [DollarType.Ten] = 10,
                [DollarType.Five] = 10,
                [DollarType.One] = 10
            };
            if(print)
                DisplayBalance();
        }

        public void Withdrawal(string amtString)
        {
            amtString = Regex.Replace(amtString, "[^0-9]", "");
            var amount = int.Parse(amtString);

            var neededDollars = new Dictionary<DollarType, int>();
            var toProcess = amount;
            foreach (var dollarType in DollarStorage.Keys)
            {
                var denomination = (int) dollarType;

                neededDollars[dollarType] = toProcess / denomination;
                toProcess = toProcess % denomination;
                
                if (DollarStorage[dollarType] < neededDollars[dollarType])
                {
                    Console.WriteLine("Failure: insufficient funds");
                    return;
                }
            }

            foreach (var neededType in neededDollars.Keys)
            {
                DollarStorage[neededType] -= neededDollars[neededType];
            }

            Console.WriteLine($"Success: Dispensed ${amount}");
            DisplayBalance();
        }

        public void ListAmounts(string[] strings)
        {
            foreach (var denomination in strings)
            {
                var clean = Regex.Replace(denomination, "[^0-9]", "");
                var enumValue = (DollarType) int.Parse(clean);
                Console.WriteLine($"${clean} - {DollarStorage[enumValue]}");
            }
        }

        public void DisplayBalance()
        {
            Console.WriteLine("Machine balance:");
            foreach (var denomination in DollarStorage.Keys)
            {
                Console.WriteLine($"${(int) denomination} - {DollarStorage[denomination]}");
            }
        }
    }
}