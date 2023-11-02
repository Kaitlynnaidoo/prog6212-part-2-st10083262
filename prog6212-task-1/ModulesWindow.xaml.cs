using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace prog6212_task_1
{
    /// <summary>
    /// Interaction logic for ModulesWindow.xaml
    /// </summary>
    public partial class ModulesWindow : Window
    {

        private ObservableCollection<Module> modules = new ObservableCollection<Module>();
        private int semesterWeeks;
        public ModulesWindow(int numberOfWeeks)
        {
            InitializeComponent();

            semesterWeeks = numberOfWeeks;

            Module module1 = new Module("prog6221", "Programming 2A", 40, 5, semesterWeeks);
            Module module2 = new Module("cldv7311", "Cloud Development 2B", 30, 15, semesterWeeks);

            modules.Add(module1);
            modules.Add(module2);

            modulesList_UI.ItemsSource = modules;
            moduleComboBox_UI.ItemsSource = modules;

        }

        private void addHoursStudied_UI_Click(object sender, RoutedEventArgs e)
        {
            // Error flag to indicate if validations failed
            bool errorFlag = false;
            hoursError_UI.Text = "";


            // Get fields from the UI and check them
            string selectedModule = "";
            if (moduleComboBox_UI.SelectedItem == null) // If no module was selected
            {
                errorFlag = true;
                hoursError_UI.Text += "No Module was selected. ";
            }
            else
            {
                selectedModule = moduleComboBox_UI.SelectedItem.ToString();
            }


            // Get hours studied
            string hoursStudied_ui = hoursStudied_UI.Text;

            int hoursStudiedToday = 0;
            if (!Int32.TryParse(hoursStudied_ui, out hoursStudiedToday))
            {
                errorFlag = true;
                hoursError_UI.Text += "The number of hours studied is not correct. ";
            }

            // If no errors were found
            if (!errorFlag)
            {
                // Search and find the modules
                Module matching = modules.FirstOrDefault(u => u.moduleCode == selectedModule);
                if (matching != null) // If it was found
                {
                    matching.setHoursStudiedToday(hoursStudiedToday);
                }
            }
        }

        private void addModule_UI_Click(object sender, RoutedEventArgs e)
        {
            // Flag indicated if there was an error in any of the fields entered
            bool errorFlag = false;
            moduleError_UI.Text = "";

            // Get the fields from the UI
            string code = moduleCode_UI.Text;
            string classHours_ui = moduleClassHours_UI.Text;
            string name = moduleName_UI.Text;
            string credits_ui = moduleCredits_UI.Text;

            // Validate all the above fields

            int numberOfCredits = 0;
            if (!Int32.TryParse(credits_ui, out numberOfCredits))
            {
                errorFlag = true;
                moduleError_UI.Text += "The number of credits inputted is an invalid number of credits. ";
            }

            int classHoursPerWeek = 0;
            if (!Int32.TryParse(classHours_ui, out classHoursPerWeek))
            {
                errorFlag = true;
                moduleError_UI.Text += "The class hours inputted is an incorrect number of hours. ";
            }

            if (String.IsNullOrEmpty(code))
            {
                errorFlag = true;
                moduleError_UI.Text += "The module code entered is empty. ";
            }

            if (String.IsNullOrEmpty(name))
            {
                errorFlag = true;
                moduleError_UI.Text += "The module name entered is empty. ";
            }

            // Check if there was any errors
            if (!errorFlag)
            {
                Module module = new Module(code, name, numberOfCredits, classHoursPerWeek, semesterWeeks);
                modules.Add(module);
            }
        }
    }
}
