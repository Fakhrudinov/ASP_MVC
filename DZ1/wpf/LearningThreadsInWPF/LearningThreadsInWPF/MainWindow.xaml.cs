using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace LearningThreadsInWPF
{
    /// Tasks:
    /// 1.	Создайте форму WPF или WinForms, разместите на ней текстовое поле и в другом потоке последовательно добавляйте на него числа Фибоначчи.
    /// 2.	В этой же форме добавьте регулятор, который будет отсчитывать, сколько секунд должно пройти, прежде чем появится следующее число.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Thread f = new Thread(ShowFibonachiNumber);
            f.Start();
        }

        public void ShowFibonachiNumber()
        {
            int counter = -138;
            bool direction = true;//increment, false = decrement
            
            while (Application.Current != null)
            {
                //Максимальное расчитываемое число фибоначчи от -139 до 139, иначе выйдем за пределы decimal
                //проверяю, надо ли менять направление инкремента счетчика
                if (counter > 138)
                {
                    direction = false;
                }
                else if (counter < -138)
                {
                    direction = true;
                }

                //берем значение задержки
                double timerValue = 1;//единица тут просто так, иначе с переменной без значения работать не получается.      
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            timerValue = Double.Parse(seconds.Text) * 1000;
                        });
                }

                Thread.Sleep((int)timerValue);

                //выводим данные числа фибоначчи
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {                            
                            textBlock.Text = GetFibonacci(counter).ToString();
                            indexFibonacci.Content = counter;
                        }));
                }

                //меняю значение счетчика
                if (direction)
                {
                    counter++;
                }
                else
                {
                    counter--;
                }
            }
        }

        private static decimal GetFibonacci(int index)
        {
            int startNumber = CalculateStartNumber(index);

            decimal resultLoop = CalculateFibonacciWithLoop(Math.Abs(index), startNumber);

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

        private void Button_Click_Increment(object sender, RoutedEventArgs e)
        {
            double secondsValue = 1;// единица тут просто так, иначе с переменной без значения работать не получается.

            // взять значение текстового поля
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (ThreadStart)delegate
                    {
                        secondsValue = Double.Parse(seconds.Text);
                    });
            }
            // увеличить
            if (secondsValue >= 1 && secondsValue <= 100)
            {
                secondsValue++;
            }
            else if(secondsValue >= 0.1)
            {
                secondsValue = secondsValue + 0.1;
            }

            // изменить текстовое поле
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (ThreadStart)delegate
                    {
                        seconds.Text = Math.Round(secondsValue, 1).ToString();
                    });
            }
        }

        private void Button_Click_Decrement(object sender, RoutedEventArgs e)
        {
            double secondsValue = 1;// единица тут просто так, иначе с переменной без значения работать не получается.

            // взять значение текстового поля
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (ThreadStart)delegate
                    {
                        secondsValue = Double.Parse(seconds.Text);
                    });
            }
            // уменьшить
            if (secondsValue > 1)
            {
                secondsValue--;
            }
            else if (secondsValue <= 1 && secondsValue > 0.1)
            {
                secondsValue = secondsValue - 0.1;
            }
            else 
            {
                secondsValue = 0.1;
            }

            // изменить текстовое поле
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (ThreadStart)delegate
                    {
                        seconds.Text = Math.Round(secondsValue, 1).ToString();
                    });
            }
        }
    }
}
