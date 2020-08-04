using System;
namespace test1
{
    public class MinHeap
    {
        int[] arr;
        int index = -1;
        public MinHeap(int n)
        {
            arr = new int[n];
        }

        public void Add(int x)
        {
            arr[++index] = x;
            BubbleUp(index);

        }

        public int Pop()
        {
            int x = -1;
            if (index >= 0)
            {
                x = arr[0];
                arr[0] = arr[index];
                --index;
                BubbleDown(0);
            }

            return x;
        }

        public int Peek()
        {
            if (index >= 0)
                return arr[0];

            return -1;
        }

        void BubbleDown(int i)
        {
            int lci, rci, lc, rc,min;
            while(i<index)
            { 
                lci = (2 * i) + 1;
                rci = (2 * i) + 2;
               
                if (lci <= index)
                    lc = arr[lci];
                else
                    lc = Int32.MaxValue;

                if (rci <= index)
                    rc = arr[rci];
                else
                    rc = Int32.MaxValue;

                min = GetMin(lc, rc);
                if (min < arr[i])
                {
                    if (min == lc)
                    {
                        Swap(i, lci);
                        i = lci;
                    }
                    else
                    {
                        Swap(i, rci);
                        i = rci;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        void BubbleUp(int i)
        {
            int pi;
            while (true)
            {
                pi = (i - 1) / 2;
                if (pi < 0)
                    break;

                if (arr[pi] < arr[i])
                    break;

                Swap(pi, i);
                i = pi;
            }
        }

        void Swap(int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        int GetMin(int x, int y)
        {
            if (x < y)
                return x;

            return y;
        }

    }
}
