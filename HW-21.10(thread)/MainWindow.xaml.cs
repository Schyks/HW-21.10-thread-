using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Threading;

namespace HW_21._10_thread_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      public MainWindow()
        {
            InitializeComponent();
        }

       private void Begin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Begin.Text == "Begin")
            {
                Begin.Text = string.Empty;
            }
        }

        private void End_GotFocus(object sender, RoutedEventArgs e)
        {
            if (End.Text == "End")
            {
                End.Text = string.Empty;
            }
        }
        private void Begin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Begin.Text))
            {
                Begin.Text = "Begin";
            }
        }

        private void End_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(End.Text))
            {
                End.Text = "End";
            }
        }

        private bool shouldStop = false;
        private void StopThread()
        {
            shouldStop = true;
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            shouldStop = false;
            Thread thread = new Thread(PrimeNumberGeneric);
            thread.Start();
        }

        private void PrimeNumberGeneric()
        {
            Dispatcher.Invoke(() =>// без цього не працює
            {
            List<int> primeNumbers = new List<int>();
            PrimeNumbers.ItemsSource = primeNumbers;
             int start = 2, end = 1000;
            if (Begin.Text != "Begin" && End.Text == "End")
            { start = int.Parse(Begin.Text); }
            else if (Begin.Text == "Begin" && End.Text != "End")
            { end = int.Parse(End.Text); }
            else if (Begin.Text != "Begin" && End.Text != "End")
            { start = int.Parse(Begin.Text); end = int.Parse(End.Text); }

            for (int number = start; number <= end; number++)
            {
                if (IsPrime(number))
                {
                    primeNumbers.Add(number);
                }
            }
                if (shouldStop)
                {
                    return;
                }
                Dispatcher.Invoke(() =>
                {
                    PrimeNumbers.ItemsSource = primeNumbers;
                });
            });
        }
        private bool IsPrime(int number)
        {
            if (number <= 1)
            {
                return false;
            }
            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
 
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopThread();
        }

        private void Start1_Click(object sender, RoutedEventArgs e)
        {
            shouldStop = false;
            Thread thr = new Thread(GenerateFibonacci);
            thr.Start();
        }
        private void GenerateFibonacci()
        { // а тут працює
            int n = 20;
            List<int> fibonacciNumbers = new List<int>();

            {
                int a = 0;
                int b = 1;
                for (int i = 0; i < n; i++)
                {
                    fibonacciNumbers.Add(a);
                    int temp = a;
                    a = b;
                    b = temp + b;
                }
            }
            Dispatcher.Invoke(() =>
            {
                Fibonachi.ItemsSource = fibonacciNumbers;
            });
        }
    }
}

