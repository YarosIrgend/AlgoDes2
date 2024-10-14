using System;
using System.Collections.Generic;

namespace AlgoDes2
{
    static class AStarAlgorithm
    {
        private const int boardSize = 8;
        static char[,] board = new char[boardSize, boardSize];
        static char[,] goalBoard = {
            { '.', '.', '.', 'Q', '.', '.', '.', '.' },
            { 'Q', '.', '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', 'Q', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.', '.', 'Q' },
            { '.', 'Q', '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.', 'Q', '.' },
            { '.', '.', 'Q', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', 'Q', '.', '.' }
        };
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

        private static void PrintBoard(char[,] board)
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
            PrintBoard(board);

            var result = AStarSearch(board);
            if (result != null)
            {
                Console.WriteLine("\nРезультат: ");
                PrintBoard(result);
                Console.WriteLine($"К-ть ітерацій: {iterations}");
                Console.WriteLine($"К-ть згенерованих станів: {states}");
                Console.WriteLine($"К-ть глухих кутів: {blindCorners}");
                Console.WriteLine($"К-ть вузлів у пам'яті: {nodesInMemory}");
            }
            else
            {
                Console.WriteLine("Рішення не знайдено.");
            }
        }

        static char[,] AStarSearch(char[,] initialBoard)
        {
            var openSet = new SortedSet<State>(new StateComparer());
            var closedSet = new HashSet<string>();
            openSet.Add(new State(initialBoard, 0));

            while (openSet.Count > 0)
            {
                iterations++;
                var current = openSet.Min;
                openSet.Remove(current);

                nodesInMemory = openSet.Count + closedSet.Count;

                if (current.H == 0)
                {
                    return current.Board;
                }

                closedSet.Add(BoardToString(current.Board));
        
                var neighbors = GetNeighbors(current);

                if (neighbors.Count == 0)
                {
                    blindCorners++;
                }

                bool allNeighborsInClosedSet = true;

                foreach (var neighbor in neighbors)
                {
                    if (!closedSet.Contains(BoardToString(neighbor.Board)))
                    {
                        openSet.Add(neighbor);
                        states++;
                        allNeighborsInClosedSet = false;
                    }

                    nodesInMemory = openSet.Count + closedSet.Count;
                }

                if (allNeighborsInClosedSet)
                {
                    blindCorners++;
                }
            }
            return null;
        }

        static int CalculateHeuristic(char[,] board)
        {
            int misplaced = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] != goalBoard[i, j])
                    {
                        misplaced++;
                    }
                }
            }
            return misplaced;
        }

        static List<State> GetNeighbors(State current)
        {
            var neighbors = new List<State>();
            for (int row = 0; row < boardSize; row++)
            {
                int currentCol = -1;
                for (int col = 0; col < boardSize; col++)
                {
                    iterations++;
                    if (current.Board[row, col] == 'Q')
                    {
                        currentCol = col;
                        break;
                    }
                }

                if (currentCol == -1) continue;

                for (int col = 0; col < boardSize; col++)
                {
                    iterations++;
                    if (col != currentCol && IsSafe(current.Board, row, col))
                    {
                        var newBoard = (char[,])current.Board.Clone();
                        newBoard[row, currentCol] = '.';
                        newBoard[row, col] = 'Q';
                        neighbors.Add(new State(newBoard, current.G + 1));
                        states++;
                    }
                }
            }
            return neighbors;
        }

        static bool IsSafe(char[,] board, int row, int col)
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

        static string BoardToString(char[,] board)
        {
            string result = "";
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    result += board[i, j];
                }
            }
            return result;
        }

        class State
        {
            public char[,] Board { get; }
            public int G { get; } // Кількість кроків
            public int H { get; } // Евристика

            public int F => G + H; // f(n) = g(n) + h(n)

            public State(char[,] board, int g)
            {
                Board = (char[,])board.Clone();
                G = g;
                H = CalculateHeuristic(Board);
            }
        }

        class StateComparer : IComparer<State>
        {
            public int Compare(State stateA, State stateB)
            {
                return stateA.F.CompareTo(stateB.F);
            }
        }
    }
}