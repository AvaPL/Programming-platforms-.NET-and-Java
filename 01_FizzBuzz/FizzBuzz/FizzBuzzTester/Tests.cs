using System;
using System.Collections.Generic;
using FizzBuzz;
using NUnit.Framework;

namespace FizzBuzzTester
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ShouldReturnBuzzFor10()
        {
            List<int> numbers = new List<int>(){10};
            List<string> expectedResults = new List<string>(){"Buzz"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }


        [Test]
        public void ShouldReturnCorrectResultForListOfElements()
        {
            List<int> numbers = new List<int>(){10, 3, 15, 2};
            List<string> expectedResults = new List<string>(){"Buzz", "Fizz", "FizzBuzz", "2"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults, actualResults);
        }

        [Test]
        public void ShouldReturnCorrectResultsForNegativeNumbers()
        {
            List<int> numbers = new List<int>(){-2, -3, -5, -15};
            List<string> expectedResults = new List<string>(){"-2", "Fizz", "Buzz", "FizzBuzz"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults, actualResults);
        }

        [Test]
        public void ShouldReturnEmptyListForEmptyList()
        {
            List<int> numbers = new List<int>(){};
            List<string> expectedResults = new List<string>(){};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults, actualResults);
        }

        [Test]
        public void ShouldReturnEmptyListForNull()
        {
            List<int> numbers = null;
            List<string> expectedResults = new List<string>(){};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults, actualResults);
        }

        [Test]
        public void ShouldReturnFizzBuzzFor15()
        {
            List<int> numbers = new List<int>(){15};
            List<string> expectedResults = new List<string>(){"FizzBuzz"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }

        [Test]
        public void ShouldReturnFizzFor6()
        {
            List<int> numbers = new List<int>(){6};
            List<string> expectedResults = new List<string>(){"Fizz"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }

        [Test]
        public void ShouldReturnStringRepresentationFor2()
        {
            List<int> numbers = new List<int>(){2};
            List<string> expectedResults = new List<string>(){"2"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }
        
        [Test]
        public void ShouldReturnBuzzingaFor7()
        {
            List<int> numbers = new List<int>(){7};
            List<string> expectedResults = new List<string>(){"Buzzinga"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }
        
        [Test]
        public void ShouldReturnFizzBuzzFor352()
        {
            List<int> numbers = new List<int>(){352};
            List<string> expectedResults = new List<string>(){"FizzBuzz"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }
        
        [Test]
        public void ShouldReturnFizzBuzzFor253()
        {
            List<int> numbers = new List<int>(){253};
            List<string> expectedResults = new List<string>(){"FizzBuzz"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }
        
        [Test]
        public void ShouldReturnBuzzFor256()
        {
            List<int> numbers = new List<int>(){256};
            List<string> expectedResults = new List<string>(){"Buzz"};
            List<string> actualResults = FizzBuzzer.MillNumbers(numbers);
            Assert.AreEqual(expectedResults[0], actualResults[0]);
        }
    }
}