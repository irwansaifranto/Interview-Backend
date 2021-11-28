using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moduit.Utility
{
    public class ProcessAttempHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action">Action you want to reattemp due to exception</param>
        /// <param name="sleep">In milli seconds</param>
        /// <returns></returns>
        public TResult ReattempException<TResult>(
            Func<TResult> action,
            int sleepMin = 500,
            int sleepMax = 300000,
            int maxAttemp = 5,
            Action<Exception, int> actionOnFailure = null)
        {
            int numOfAttemp = 1;

            TResult result = default(TResult);

            while (true)
            {
                try
                {
                    result = action();
                    break;
                }
                catch (Exception exp)
                {
                    actionOnFailure?.Invoke(exp, numOfAttemp);

                    if (numOfAttemp > maxAttemp)
                    {
                        throw;
                    }

                    numOfAttemp++;

                    Random _random = new Random();
                    int randomSleep = _random.Next(sleepMin, sleepMax);

                    Thread.Sleep(randomSleep);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">Action you want to reattemp due to exception</param>
        /// <param name="sleep">In milli seconds</param>
        public void ReattempException(
            Action action,
            int sleepMin = 500,
            int sleepMax = 300000,
            int maxAttemp = 5,
            Action<Exception, int> actionOnFailure = null)
        {
            int numOfAttemp = 1;

            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception exp)
                {
                    actionOnFailure?.Invoke(exp, numOfAttemp);

                    if (numOfAttemp > maxAttemp)
                    {
                        throw;
                    }

                    numOfAttemp++;

                    Random _random = new Random();
                    int randomSleep = _random.Next(sleepMin, sleepMax);

                    Thread.Sleep(randomSleep);
                }
            }
        }
    }
}
