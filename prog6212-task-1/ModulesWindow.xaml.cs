using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace prog6212_task_1
{
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
                /
                bool errorFlag = false;
                hoursError_UI.Text = "";


                
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

               
                if (!errorFlag)
                {
                    
                    Module matching = modules.FirstOrDefault(u => u.moduleCode == selectedModule);
                    if (matching != null) 
                    {
                        matching.setHoursStudiedToday(hoursStudiedToday);
                    }
                }
            }
        }

        private void addModule_UI_Click(object sender, RoutedEventArgs e)
        {
           
                // Flag indicated if there was an error 
                bool errorFlag = false;
                moduleError_UI.Text = "";

                
                string code = moduleCode_UI.Text;
                string classHours_ui = moduleClassHours_UI.Text;
                string name = moduleName_UI.Text;
                string credits_ui = moduleCredits_UI.Text;

                // Validate fields

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

                // Check for errors
                if (!errorFlag)
                {
                    Module module = new Module(code, name, numberOfCredits, classHoursPerWeek, semesterWeeks);
                    modules.Add(module);
                }

                if (!errorFlag)
            {
                
                Module module = new Module(moduleCode_UI.Text, moduleName_UI.Text, Convert.ToInt32(moduleCredits_UI.Text), Convert.ToInt32(moduleClassHours_UI.Text), semesterWeeks);
                module.SaveToDatabase(username);
                modules.Add(module);
            }
        }

        private void LoadModulesFromDatabase(string username)
        {
            
            string connectionString = "Data Source=lab000000\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                
                string query = "SELECT Code, Name, Credits, ClassHoursPerWeek, SemesterWeeks FROM Modules WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Module module = new Module(
                                reader["Code"].ToString(),
                                reader["Name"].ToString(),
                                Convert.ToInt32(reader["Credits"]),
                                Convert.ToInt32(reader["ClassHoursPerWeek"]),
                                Convert.ToInt32(reader["SemesterWeeks"])
                            );
                            modules.Add(module);
                        }
                    }
                }
            }
        }
    }
}

