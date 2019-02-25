using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            for (int i = 0; i < arr.Length/2; i++)
            {
                Console.WriteLine($"{i} - {arr.Length - i}");
            }
        }
    }
}
