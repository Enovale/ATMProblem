using System;
using System.IO;
using NUnit.Framework;

namespace ATMProblem.Tests
{
    public class Tests
    {
        private StringWriter _consoleOut;

        private int _stdoutPosition;
        
        [SetUp]
        public void Setup()
        {
            Program.AtmMachine = new MoneyMachine();
            _stdoutPosition = 0;
            _consoleOut = new StringWriter();
            Console.SetOut(_consoleOut);
        }

        private string ReadConsoleOut()
        {
            var sout = _consoleOut.ToString().Substring(_stdoutPosition);
            _stdoutPosition += sout.Length;
            return sout;
        }

        [Test]
        public void TestSanity()
        {
            Program.ProcessCommand("l");
            Assert.AreEqual("Failure: Invalid Command" + Environment.NewLine, ReadConsoleOut());
            _consoleOut.Flush();
            Program.ProcessCommand("W ");
            Assert.AreEqual("Failure: Invalid Command" + Environment.NewLine, ReadConsoleOut());
        }

        [Test]
        public void TestWithdrawal()
        {
            Program.ProcessCommand("W $1111");
            Assert.AreEqual("Failure: insufficient funds" + Environment.NewLine, ReadConsoleOut());
            Program.ProcessCommand("W $748");
            Assert.IsTrue(Program.AtmMachine.DollarStorage[DollarType.OneHundred] == 3);
            Assert.IsTrue(Program.AtmMachine.DollarStorage[DollarType.Fifty] == 10);
            Assert.IsTrue(Program.AtmMachine.DollarStorage[DollarType.Twenty] == 8);
            Assert.IsTrue(Program.AtmMachine.DollarStorage[DollarType.Ten] == 10);
            Assert.IsTrue(Program.AtmMachine.DollarStorage[DollarType.Five] == 9);
            Assert.IsTrue(Program.AtmMachine.DollarStorage[DollarType.One] == 7);
            Program.ProcessCommand("R");
            ReadConsoleOut();
            Program.ProcessCommand("W 9");
            ReadConsoleOut();
            Program.ProcessCommand("W 9");
            ReadConsoleOut();
            Program.ProcessCommand("W 9");
            Assert.AreEqual("Failure: insufficient funds" + Environment.NewLine, ReadConsoleOut());
        }

        [Test]
        public void TestRestock()
        {
            Program.ProcessCommand("R");
            Assert.AreEqual(@"Machine balance:
$100 - 10
$50 - 10
$20 - 10
$10 - 10
$5 - 10
$1 - 10" + Environment.NewLine, ReadConsoleOut());
        }

        [Test]
        public void TestList()
        {
            Program.ProcessCommand("I 10 20 50");
            Assert.AreEqual(@"$10 - 10
$20 - 10
$50 - 10" + Environment.NewLine, ReadConsoleOut());
            Program.ProcessCommand("I ksadjfd");
            Assert.AreEqual("Failure: Invalid Command" + Environment.NewLine, ReadConsoleOut());
        }
    }
}