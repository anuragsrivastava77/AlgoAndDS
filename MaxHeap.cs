using System;
namespace test4
{
       public struct Tuple
    {
        public int x, y, r;

        public void Set(int a, int b, int c)
        {
            x = a;y = b;r = c;
        }
    }

    public class MaxHeap
    {
        Tuple[] arr;
        int index = -1;

        public Tuple defaultTuple = new Tuple();
       
        public MaxHeap(int n)
        {
            arr = new Tuple[n];
            defaultTuple.Set(-1, -1, -1);
        }

        public void Push(Tuple x)
        {
            arr[++index] = x;
            BubbleUp(index);

        }

        public Tuple Pop()
        {
            if (index >= 0)
            {
                Tuple x = arr[0];
                arr[0] = arr[index];
                --index;
                BubbleDown(0);

                return x;
            }

            return defaultTuple;
        }

        public Tuple Peek()
        {
            if (index >= 0)
                return arr[0];

            return defaultTuple;
        }

        void BubbleDown(int i)
        {
            int lci, rci;
            Tuple lc, rc, max;

            while (i < index)
            {
                lci = (2 * i) + 1;
                rci = (2 * i) + 2;

                if (lci <= index)
                    lc = arr[lci];
                else
                    lc = this.defaultTuple;

                if (rci <= index)
                    rc = arr[rci];
                else
                    rc =  this.defaultTuple;

                max = GetMax(lc, rc);
                if (max.r > arr[i].r)
                {
                    if (max.x == lc.x && max.y== lc.y)
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

                if (arr[pi].r >= arr[i].r)
                    break;

                Swap(pi, i);
                i = pi;
            }
        }

        void Swap(int i, int j)
        {
            Tuple temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        Tuple GetMax(Tuple x, Tuple y)
        {
            if (x.r > y.r)
                return x;

            return y;
        }

    }
}
