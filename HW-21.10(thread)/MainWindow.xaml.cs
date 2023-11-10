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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HW_21._10_thread_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool shouldStopPri = false;
        private bool shouldPausePri = false;
        private bool shouldStopFib = false;
        private bool shouldPauseFib = false;

        private ManualResetEventSlim pauseEventPri = new ManualResetEventSlim(true);
        private ManualResetEventSlim pauseEventFib = new ManualResetEventSlim(true);

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

              
        private void PrimeNumberGeneric()
        {
            int start = 2, end = 1000;
            Dispatcher.Invoke(() =>
            {
                if (Begin.Text != "Begin" && End.Text == "End")
                { start = int.Parse(Begin.Text); }
                else if (Begin.Text == "Begin" && End.Text != "End")
                { end = int.Parse(End.Text); }
                else
                if (Begin.Text != "Begin" && End.Text != "End")
                { start = int.Parse(Begin.Text); end = int.Parse(End.Text); }
                MessageBox.Show($"Range: {start} - {end}");
            });
            for (int number = start; number < end; number++)
            {
                if (shouldStopPri)
                {
                    return;
                }
                
                if (shouldPausePri)
                {
                    pauseEventPri.Wait();
                }

                if (IsPrime(number))
                {
                    Dispatcher.Invoke(() =>
                    {
                        PrimeNumbers.Items.Add(number);
                    });

                    Thread.Sleep(1000);
                }
            }

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
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            shouldStopPri = false;
            PrimeNumbers.Items.Clear();
            Thread thread = new Thread(PrimeNumberGeneric);
            thread.Start();
        }
        private void StopThreadPri()
        {
            shouldStopPri = true;
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopThreadPri();

        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            shouldPausePri = true;
            pauseEventPri.Reset();

        }
        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            shouldPausePri = false;
            pauseEventPri.Set();
        }

        private void GenerateFibonacci()
        { 
            int n = 100;
            {
                int a = 1;
                int b = 1;
                for (int i = 1; i < n; i++)
                {
                    Dispatcher.Invoke(() =>
                        Fibonachi.Items.Add(a));
                    Thread.Sleep(1000);
                    int temp = a;
                    a = b;
                    b = temp + b;
                    if (shouldStopFib)
                    {
                        return;
                    }
                    if (shouldPauseFib)
                    {
                        pauseEventFib.Wait();
                    }
                }
               
            }
         }
        private void Start1_Click(object sender, RoutedEventArgs e)
        {
            shouldStopFib = false;
            Fibonachi.Items.Clear();
            Thread thr = new Thread(GenerateFibonacci);
            thr.Start();
        }
        private void Pause1_Click(object sender, RoutedEventArgs e)
        {
            shouldPauseFib = true;
            pauseEventFib.Reset();
        }
        private void StopThreadFib()
        {
            shouldStopFib = true;
        }
        private void Stop1_Click(object sender, RoutedEventArgs e)
        {
            StopThreadFib();
        }

        private void Stop2_Click(object sender, RoutedEventArgs e)
        {
            StopThreadFib();
            StopThreadPri();
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            StopThreadFib();
            StopThreadPri();
            PrimeNumbers.Items.Clear();
            Fibonachi.Items.Clear();
            Begin.Text = "Begin";
            End.Text = "End";
        }

        private void Resume1_Click(object sender, RoutedEventArgs e)
        {
            shouldPauseFib = false;
            pauseEventFib.Set();
        }
    }
}

