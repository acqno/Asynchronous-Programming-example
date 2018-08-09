using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * 
 * Name: Alvin Quijano 
 * Ver: 1.2 - Refactored ansynchronous facotrial method 
 * Description: Code behind file for window form application
 * 
 */
namespace AlvinQuijano_Lab06_Ex1
{
    // Delegates
    public delegate bool NumberPredicate(int number);

    public partial class Form1 : Form
    {
        NumberPredicate evenPredicate = IsEven;

        // arrays
        int[] intList = new int[10];
        double[] doubleList = new double[10];
        char[] charList = { 'A', 'B', 'C', 'X', 'Y', 'Z', '!', '@', '?', '/' };

        public Form1()
        {
            InitializeComponent();
        }

        // Task #1 - Calculate Factorial asynchronously
        private async void calcButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve input number from user 
                ulong num = ulong.Parse(factTextBox.Text);             

                // Task to perform factorial calculation
                Task<ulong> factorialTask = Task.Run(() => Factorial(num));
                
                resultLabel.Text = "Calculating...";

                await Task.Delay(1999);
                await factorialTask;
                
                resultLabel.Text = factorialTask.Result.ToString();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                resultLabel.Text = "Please enter a number";
            }
        }

        // Recursive factorial method 
        public ulong Factorial(ulong num)
        {
            if (num == 1)
                return 1;
            else
                return num * Factorial(num - 1);
        }

        // Task #2 - Check for Even/Odd
        private void checkButton_Click(object sender, EventArgs e)
        {
            try
            {
                int num = int.Parse(inputTextBox.Text);

                if (evenPredicate(num))
                {
                    boolLabel.Text = "Even";
                }
                else
                {
                    boolLabel.Text = "Odd";
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                boolLabel.Text = "Enter a valid number";
            }
        }

        // Method to check if input value is even
        private static bool IsEven(int i)
        {
            if (i % 2 == 0)
                return true;
            else
                return false;
        }

        // Task #3 - Display list of Values, search values
        private void generateButton_Click(object sender, EventArgs e)
        {
           
            Random rand = new Random();

            if (intRButton.Checked)
            {
                for (int i = 0; i < intList.Length; i++)
                {
                    intList[i] = rand.Next(10, 99);
                }

                valListBox.DataSource = intList;
            }
            else if (doubleRButton.Checked)
            {
                for (int i = 0; i < doubleList.Length; i++)
                {
                    doubleList[i] = Math.Round(rand.NextDouble() * (99 - 10) + 10, 2);
                }

                valListBox.DataSource = doubleList;
            }
            else
            {
                valListBox.DataSource = charList;
            }
        }

        // Task #3a - Search generated list of values for a value
        private void searchButton_Click(object sender, EventArgs e)
        {

            if (intRButton.Checked)
            {
                int search = int.Parse(searchValTextBox.Text);

                foundValLabel.Text = ($"{SearchValue(search)}");
            }
            else if (doubleRButton.Checked)
            {
                double search = double.Parse(searchValTextBox.Text);

                foundValLabel.Text = ($"{SearchValue(search)}");
            }
            else if (charRButton.Checked)
            {
                char search = Convert.ToChar(searchValTextBox.Text);

                foundValLabel.Text = ($"{SearchValue(search)}");
            }
            
        }

        public bool SearchValue<T>(T value) where T : IComparable
        {
            for (int i = 0; i < valListBox.Items.Count; i++)
            {
                if (valListBox.Items.Contains(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // Task #3b - Display list of values between high index and low index
        private void displayButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (intRButton.Checked)
                {
                    int low = int.Parse(lowTextBox.Text);
                    int high = int.Parse(highTextBox.Text);

                    int[] inputIntArray = valListBox.Items.OfType<int>().ToArray();

                    NewList(inputIntArray, low, high);
                }
                else if (doubleRButton.Checked)
                {
                    double low = double.Parse(lowTextBox.Text);
                    double high = double.Parse(highTextBox.Text);

                    double[] inputDblArray = valListBox.Items.OfType<double>().ToArray();
                    NewList(inputDblArray, low, high);
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            
            
        }

        // Method that takes 3 arguments - array, lowindex and highindex and checks each element in array if it is inbetween low and high
        public void NewList<T>(T[] genArray, T lowIndex, T highIndex) 
            where T : IComparable
        {
            List<T> compareList = new List<T>();

            foreach (var item in genArray)
            {
                if((item.CompareTo(lowIndex)) == 1 && (item.CompareTo(highIndex)) == -1)
                {
                    compareList.Add(item);
                }
            }
            lowHighListBox.DataSource = compareList;                
        }
    }
}
