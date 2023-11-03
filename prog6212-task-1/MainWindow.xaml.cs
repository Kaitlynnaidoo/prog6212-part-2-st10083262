using System;
using System.Windows;

namespace prog6212_task_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submit_UI_Click(object sender, RoutedEventArgs e)
        {
           
            bool errorFlag = false;
            // Remove all errors f
            error_UI.Text = "";

            
            var startDate = startDate_UI.SelectedDate;
            var numWeeks = numWeeks_UI.Text;

           
            if (startDate == null)
            {
                errorFlag = true;
                error_UI.Text += "The start date for the semester inputted is wrong! ";
            }

            int numberOfWeeks = 0;
            if (!Int32.TryParse(numWeeks, out numberOfWeeks))
            {
                errorFlag = true;
                error_UI.Text += "The number of weeks inputted for the semester is wrong! ";
            }

          
            string username = "Kaitlyn";
            if (AuthenticateUser(username, /*  */);
            {
                // Open the modules window
                ModulesWindow window = new ModulesWindow(username, numberOfWeeks);
                window.Show();
                this.Close();
            }
            else
            {
                // Display an error message or handle unsuccessful authentication
                MessageBox.Show("Authentication failed. Please check your credentials.");
            }
        }

        
        private bool AuthenticateUser(string username, string password)
        {
        
            bool isAuthenticated = enteredUsername == correctUsername && enteredPassword == correctPassword;

            return isAuthenticated;
        }
    }
}
