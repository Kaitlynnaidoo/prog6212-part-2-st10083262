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

namespace prog6212_task_1
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

        private void submit_UI_Click(object sender, RoutedEventArgs e)
        {

            // Indicates if one or more of the fields has an invalid value
            bool errorFlag = false;
            // Remove all errors from the previous button press
            error_UI.Text = "";

            // Get the values from the UI
            var startDate = startDate_UI.SelectedDate;
            var numWeeks = numWeeks_UI.Text;


            // Validate all the values

            if (startDate == null) // If the date in the date picker has not been set
            {
                errorFlag = true;
                error_UI.Text += "The start date for the semester inputted is incorrect! ";
            }

            int numberOfWeeks = 0;
            try
            {
                numberOfWeeks = Int32.Parse(numWeeks);
            }
            catch (FormatException)
            {
                errorFlag = true;
                error_UI.Text += "The number of weeks inputted for the semester is incorrect! ";
            }


            if (!errorFlag) // If there are no errors
            {
                // Launch next window
                /*ModulesWindow win = new ModulesWindow(weeks);
                win.Show();
                this.Close();*/
            }
        }
    }
}
