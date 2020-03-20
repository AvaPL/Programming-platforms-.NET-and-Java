// Solution to kata: https://www.codewars.com/kata/5720a1cb65a504fdff0003e2/train/csharp

using System.Linq;

public class Kata
{
    public static int[] DifferenceInAges(int[] ages)
    {
        int youngest = ages.Min();
        int oldest = ages.Max();
        return new int[]{youngest, oldest, oldest-youngest};
    }
}