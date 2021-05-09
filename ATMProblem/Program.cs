using System;

namespace ATMProblem
{
    public class Program
    {
        public static MoneyMachine AtmMachine;
        
        public static void Main(string[] args)
        {
            Console.WriteLine("CTW ATM Machine");
            AtmMachine = new MoneyMachine();
            while (true)
            {
                var cmd = Console.ReadLine();
                ProcessCommand(cmd);
            }
        }

        public static void ProcessCommand(string cmd)
        {
            var args = cmd.Split(' ');

            try
            {
                switch (args[0])
                {
                    case "Q":
                        Environment.Exit(0);
                        return;
                    case "R":
                        AtmMachine.RestockStorage();
                        return;
                    case "W":
                        if (args.Length < 1)
                            throw new ArgumentNullException();
                        AtmMachine.Withdrawal(args[1]);
                        return;
                    case "I":
                        if (args.Length < 1)
                            throw new ArgumentNullException();
                        AtmMachine.ListAmounts(args[1..args.Length]);
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                Console.WriteLine("Failure: Invalid Command");
            }
        }
    }
}