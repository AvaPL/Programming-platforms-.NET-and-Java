using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FizzBuzz
{
    public class FizzBuzzer
    {
        public static List<string> MillNumbers(List<int> numbers)
        {
            if (numbers==null)
                return new List<string>();
            List<string> result = new List<string>(numbers.Count);
            foreach (var number in numbers)
            {
                if (number % 7 == 0)
                    result.Add("Buzzinga");
                else if (number % 15 == 0 || Regex.IsMatch(number.ToString(), "35|53"))
                    result.Add("FizzBuzz");
                else if (number % 3 == 0)
                    result.Add("Fizz");
                else if (number % 5 == 0 || number.ToString().Contains("5")) 
                    result.Add("Buzz");
                else
                    result.Add(number.ToString());
            }

            return result;
        }
    }
}