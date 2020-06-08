using System;
using System.Collections.Generic;

namespace test3
{

    class MainClass
    {
        public static void Main(string[] args)
        {

            string input;
            string[] inputs;

            input = Console.ReadLine().Trim();
            int tc = Convert.ToInt32(input);

            for (int t = 1; t <= tc; t++)
            {
                Console.Write("Case #" + t + ": ");

                input = Console.ReadLine();
                inputs = input.Split(' ');

                int r = Convert.ToInt32(inputs[0]);
                int c = Convert.ToInt32(inputs[1]);

                char[,] arr = new char[r, c];

                for (int i = 0; i < r; i++)
                {
                    input = Console.ReadLine().Trim();
                    for (int j = 0; j < input.Length; ++j)
                    {
                        arr[i, j] = input[j];
                    }
                }

                SpragueGrundy sg = new SpragueGrundy();
                sg.GetGrundyNumber(arr, r, c, 1, 0);
                Console.Write(sg.countFirstMoves);


                Console.WriteLine(string.Empty);
            }
        }
    }

    public class SpragueGrundy
    {

        public Dictionary<string, int> mem = new Dictionary<string, int>();

        public int countFirstMoves = 0;

        public string GetUniqueString(char[,] arr, int r, int c)
        {
            char[] chArr = new char[r * c];

            int k = -1;
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    chArr[++k] = arr[i, j];
                }
            }

            return new string(chArr);
        }

        public bool CheckEmptyRow(char[,] arr, int row, int a, int b)
        {
            bool isEmpty = true;

            for (int i = a; i <= b; ++i)
            {
                if (arr[row, i] != '.')
                {
                    isEmpty = false;
                }
            }

            return isEmpty;
        }

        public bool CheckEmptyCol(char[,] arr, int col, int a, int b)
        {
            bool isEmpty = true;

            for (int i = a; i <= b; ++i)
            {
                if (arr[i, col] != '.')
                {
                    isEmpty = false;
                }
            }

            return isEmpty;
        }

        public int GetMex(HashSet<int> set)
        {
            int x = -1;
            while (set.Contains(++x))
            { }

            return x;
        }

        public int GetXor(HashSet<int> set)
        {
            int x = 0;
            foreach (var y in set)
            {
                x = x ^ y;
            }
            return x;
        }

        public int GetGrundyNumber(char[,] arr, int r, int c, int moveNumber, int length)
        {
            string str = GetUniqueString(arr, r, c);

            if (mem.ContainsKey(str))
            {
                int mexInMem = mem[str];

                if (moveNumber == 2)
                {
                    if (mexInMem == 0)
                    {
                        countFirstMoves += length;
                    }
                }
                return mexInMem;
            }

            HashSet<int> set = new HashSet<int>();

            for (int row = 0; row < r; ++row)
            {
                for (int i = 0; i < c; ++i)
                {
                    if (i > 0 && arr[row, i - 1] == '#')
                        continue;

                    if (i == 0 || arr[row, i - 1] == 'x')
                    {
                        for (int j = i; j < c; ++j)
                        {
                            if (j < c - 1 && arr[row, j + 1] == '#')
                                continue;

                            if (j == c - 1 || arr[row, j + 1] == 'x')
                            {
                                if (CheckEmptyRow(arr, row, i, j))
                                {
                                    FillRows(arr, row, i, j);
                                    set.Add(GetGrundyNumber(arr, r, c, moveNumber + 1, j - i + 1));
                                    BackOutRows(arr, row, i, j);
                                }
                            }
                        }
                    }
                }
            }

            for (int col = 0; col < c; ++col)
            {
                for (int i = 0; i < r; ++i)
                {
                    if (i > 0 && arr[i - 1, col] == '#')
                        continue;

                    if (i == 0 || arr[i - 1, col] == 'x')
                    {
                        for (int j = i; j < r; ++j)
                        {
                            if (j < r - 1 && arr[j + 1, col] == '#')
                                continue;

                            if (j == r - 1 || arr[j + 1, col] == 'x')
                            {
                                if (CheckEmptyCol(arr, col, i, j))
                                {
                                    FillCols(arr, col, i, j);
                                    set.Add(GetGrundyNumber(arr, r, c, moveNumber + 1, j - i + 1));
                                    BackOutCols(arr, col, i, j);
                                }
                            }
                        }
                    }
                }
            }

            int mex = GetMex(set);


            if (moveNumber == 2)
            {
                if (mex == 0)
                {
                    countFirstMoves += length;
                }
            }

            mem.Add(str, mex);

            return mex;
        }

        public void FillRows(char[,] arr, int row, int a, int b)
        {
            for (int i = a; i <= b; ++i)
            {
                arr[row, i] = 'x';
            }
        }
        public void BackOutRows(char[,] arr, int row, int a, int b)
        {
            for (int i = a; i <= b; ++i)
            {
                arr[row, i] = '.';
            }
        }
        public void FillCols(char[,] arr, int col, int a, int b)
        {
            for (int i = a; i <= b; ++i)
            {
                arr[i, col] = 'x';
            }
        }
        public void BackOutCols(char[,] arr, int col, int a, int b)
        {
            for (int i = a; i <= b; ++i)
            {
                arr[i, col] = '.';
            }
        }
    }
}
