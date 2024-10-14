using System;

namespace AlgoDes2
{
    static class IDSAlgorithm
    {
        private const int boardSize = 8;
        static char[,] board = new char[boardSize, boardSize];
        static Random random = new Random();
        private static int iterations;
        private static int states;
        private static int blindCorners;
        private static int nodesInMemory;

        private static void SetRandomBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = '.';
                }

                int row = random.Next(boardSize);
                board[i, row] = 'Q';
            }
        }

        private static void PrintBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Console.Write($"{board[i, j]} ");
                }

                Console.WriteLine();
            }
        }

        public static void Solve()
        {
            Console.WriteLine("Початкова дошка");
            SetRandomBoard();
            PrintBoard();

            for (int depth = 1; depth <= boardSize; depth++)
            {
                iterations++;
                if (DLS(0, depth))
                {
                    Console.WriteLine("\nРезультат: ");
                    PrintBoard();
                    Console.WriteLine($"К-ть ітерацій: {iterations}");
                    Console.WriteLine($"К-ть згенерованих станів: {states}");
                    Console.WriteLine($"К-ть глухих кутів: {blindCorners}");
                    Console.WriteLine($"К-ть вузлів у пам'яті: {nodesInMemory}");
                    return;
                }
            }

            Console.WriteLine("Рішення не знайдено.");
        }

        static bool DLS(int row, int depth)
        {
            if (row == boardSize)
                return true;
            if (depth == 0)
                return false;

            // Знаходимо поточну колонку, де стоїть ферзь у цьому ряду
            int currentCol = -1;
            for (int col = 0; col < boardSize; col++)
            {
                iterations++;
                if (board[row, col] == 'Q')
                {
                    currentCol = col;
                    break;
                }
            }

            if (currentCol == -1)
                return false;

            for (int col = 0; col < boardSize; col++)
            {
                iterations++;
                if (col != currentCol && IsSafe(row, col))
                {
                    board[row, currentCol] = '.';
                    board[row, col] = 'Q';
                    states++;
                    nodesInMemory++;

                    if (DLS(row + 1, depth - 1))
                        return true;

                    board[row, col] = '.';
                    board[row, currentCol] = 'Q';
                    nodesInMemory--;
                }
            }

            blindCorners++;
            return false;
        }

        static bool IsSafe(int row, int col)
        {
            for (int i = 0; i < row; i++)
                if (board[i, col] == 'Q')
                    return false;

            for (int i = 1; row - i >= 0 && col - i >= 0; i++)
                if (board[row - i, col - i] == 'Q')
                    return false;

            for (int i = 1; row - i >= 0 && col + i < boardSize; i++)
                if (board[row - i, col + i] == 'Q')
                    return false;

            return true;
        }
    }
}