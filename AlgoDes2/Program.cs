using System;

namespace AlgoDes2
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Який алгоритм? 1 - IDS, 2 - A*");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    IDSAlgorithm.Solve();
                    break;
                case 2:
                    AStarAlgorithm.Solve();
                    break;
            }
        }
    }
}