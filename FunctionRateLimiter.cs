using System;
namespace Application
{
    public class FunctionRateLimiter
    {

        private DateTime lastTime;
        bool isFirstCall;
        int n;
        int numOfCalls;
        Queue<DateTime> queue;
        DateTime lastTimeTillSeonds;

        

        public FunctionRateLimiter(int valueOfn)
        {
            isfirstCall = true;
            n = valueOfn;

            // Part 1
            lastTime = DateTime.Now;

            DateTime.MinValue;
           
       

            // Part 3
            queue = new Queue<DateTime>();

            //Part 2
            DateTime currTime = DateTime.Now;
            var lastTimeTillSeonds = currTime.AddMilliseconds(-currTime.Millisecond);
            numOfCalls = 0;


        }

        // Part 1 -> One call in N milliseconds.
        public bool Apply1()
        {
            if (n == 0)
                return false;

            if (isFirstCall)
            {
                isFirstCall = false;
                lastTime = DateTime.Now;
                ExpensiveFuntion();
                return true;
            }
            else
            {
                DateTime currTime = DateTime.Now;
                TimeSpan timespan = currTime - lastTime;
                if(timespan.TotalMilliseconds > n)
                {
                    ExpensiveFuntion();
                    lastTime = DateTime.Now;
                    return true;
                }
            }

            return false;
        }

        // PART 2 -> n calls in any one second period
        public bool Apply2()
        {
            if (n == 0)
                return false;

            DateTime currTime = DateTime.Now;
            var currTimeTillSeonds = currTime.AddMilliseconds(-currTime.Millisecond);

            if (isFirstCall)
            {
                isFirstCall = false;
                lastTimeTillSeonds = currTimeTillSeonds;
                ExpensiveFuntion();
                return true;
            }
            else
            {
                if (currTimeTillSeonds > lastTimeTillSeonds)
                {
                    ExpensiveFuntion();
                    numOfCalls = 0;
                    return true;
                }
                else if(numOfCalls < n)
                {
                    ExpensiveFuntion();
                    ++numOfCalls;
                    return true;
                }
            }

            return false;
        }


        // PART 3 -> n calls in any rolling one second period
        public bool Apply3()
        {
            if (n == 0)
                return false;

            while (queue.Count > 0 && (DateTime.Now - queue.Peek()).TotalMilliseconds > 1000)
            {
                queue.Dequeue();
            }

            if (queue.Count<n)
            {
                queue.Enqueue(DateTime.Now);
                ExpensiveFuntion();
                return true;
            }

            return false;
        }

        private void ExpensiveFuntion()
        {
            //Something expensive will happen here.
        }
    }
}
