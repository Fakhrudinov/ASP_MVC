using System;
using System.Threading;

namespace MiniThreadPool.MyAction
{
    public static class FibonacciCalculation
    {
        public static decimal GetFibonacci(int index)
        {
            int startNumber = CalculateStartNumber(index);

            decimal resultLoop = CalculateFibonacciWithLoop(Math.Abs(index), startNumber);
            Thread.Sleep(1000);
            return resultLoop;
        }

        private static int CalculateStartNumber(int index)
        {
            int startNumber = 0;
            if (index < 0) // вычисление отрицательных чисел Фибоначчи
            {
                startNumber = -1;
            }
            else if (index > 0)// вычисление положительных
            {
                startNumber = 1;
            }
            else // index == 0
            {
                startNumber = 0;
            }

            return startNumber;
        }
        private static decimal CalculateFibonacciWithLoop(int index, decimal startNumber)
        {
            if (startNumber == 0)// для вычисления индекса 0
                return 0;

            decimal prev = 0; // предыдущее число
            decimal result = 1;
            int i = 1; // начинаем с 1 т.к. первый шаг у нас всегда есть, 0 отсекается.
            try
            {
                for (; i <= index; i++)
                {
                    result = prev + startNumber;
                    startNumber = prev;
                    prev = result;
                }
            }
            catch (OverflowException)
            {
                return 0;
            }
            return result;
        }
    }
}
