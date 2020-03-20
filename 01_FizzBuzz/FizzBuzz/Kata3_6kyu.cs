// Solution to kata: https://www.codewars.com/kata/545cedaa9943f7fe7b000048/train/csharp

using System;
using System.Linq;

public static class Kata
{
    public static bool IsPangram(string str)
    {
        return str.ToLower().Where(Char.IsLetter).Distinct().Count() == 26;
    }
}