using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;

namespace prog6212_task_1
{
    public class Module : INotifyPropertyChanged
    {
        
        public string moduleCode { get; private set; }
        
        public string moduleName { get; private set; }
        
        public int moduleNumOfCredits { get; private set; }
        
        public int moduleClassHoursPerWeek { get; private set; }
        
        public int moduleSemesterNumOfWeeks { get; private set; }
        
        public int moduleRecommendedStudyHours { get; private set; }

        
        public Dictionary<DateOnly, int> moduleStudyDates { get; private set; }

        public Module(string code, string name, int num_of_credits, int class_hours_per_week, int semester_num_of_weeks)
        {
            moduleCode = code;
            moduleName = name;
            moduleNumOfCredits = num_of_credits;
            moduleClassHoursPerWeek = class_hours_per_week;
            moduleSemesterNumOfWeeks = semester_num_of_weeks;
            // Calculation from the question paper
            moduleRecommendedStudyHours = num_of_credits * 10 / semester_num_of_weeks - class_hours_per_week;
            
            moduleStudyDates = new Dictionary<DateOnly, int>();
        }

        
        public void setHoursStudiedToday(int hours_studied)
        {
            // Get the date for today
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            /
            if (moduleStudyDates.ContainsKey(today))
            {
                moduleStudyDates[today] = hours_studied;
            }
            else
            {
               
                moduleStudyDates.Add(today, hours_studied);
            }

            
            OnPropertyChanged(nameof(moduleStudyDates));
            OnPropertyChanged(nameof(getRemainingSelfStudyHours));
        }

        // Returns how many hours are left 
        public int getRemainingSelfStudyHours
        {
            get
            {
                int hoursLeft = moduleRecommendedStudyHours - getHoursStudied;
                // Will never return less than 0 
                return hoursLeft < 0 ? 0 : hoursLeft;
            }
        }

        // Returns the number of hours studied
        public int getHoursStudied
        {
            get
            {
                /* Adapted from https://stackoverflow.com/a/46555185
                 * Written on Oct 4, 2017
                 * By User LionDog823
                 */
                DateTime today = DateTime.Today;
                DateOnly monday = DateOnly.FromDateTime(today.AddDays(-(int)today.DayOfWeek + 1));
                DateOnly sunday = monday.AddDays(6);

                var dates = moduleStudyDates.Where(kvp => kvp.Key >= monday && kvp.Key <= sunday)
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                return dates.Values.Sum();
            }
        }

        public override string? ToString()
        {
            return moduleCode;
        }

        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
        public void SaveStudyHoursToDatabase(string username, int hoursStudied)
        {
            
            string connectionString = "Data Source=lab000000\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                
                string query = "INSERT INTO StudyHours (Username, ModuleCode, StudyDate, HoursStudied) " +
                               "VALUES (@Username, @ModuleCode, @StudyDate, @HoursStudied)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@ModuleCode", moduleCode);
                    command.Parameters.AddWithValue("@StudyDate", DateTime.Now.Date);
                    command.Parameters.AddWithValue("@HoursStudied", hoursStudied);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}




