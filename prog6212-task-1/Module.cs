using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace prog6212_task_1
{
    class Module : INotifyPropertyChanged
    {
        // Module Code
        public string moduleCode { get; private set; }
        // Name of module
        public string moduleName { get; private set; }
        // Number of credits module is worth
        public int moduleNumOfCredits { get; private set; }
        // Number of class hours per week this module has
        public int moduleClassHoursPerWeek { get; private set; }
        // Number of weeks in the semester
        public int moduleSemesterNumOfWeeks { get; private set; }
        // Number of hours recommended to study for this module
        public int moduleRecommendedStudyHours { get; private set; }

        // A mapping of dates to the number of hours studies on that date
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
            // Initialize the mapping of study dates
            moduleStudyDates = new Dictionary<DateOnly, int>();
        }


        // Set the hours studied for a specific date
        public void setHoursStudiedToday(int hours_studied)
        {
            // Get the date for today
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // If it exists in the mapping
            if (moduleStudyDates.ContainsKey(today))
            {
                moduleStudyDates[today] = hours_studied;
            }
            else
            {
                // Create a new mapping
                moduleStudyDates.Add(today, hours_studied);
            }

            // Notify the UI that these properties have changed
            OnPropertyChanged(nameof(moduleStudyDates));
            OnPropertyChanged(nameof(getRemainingSelfStudyHours));
        }


        // Returns how many hours are left to be studied from the recommended amount this week
        public int getRemainingSelfStudyHours
        {

            get
            {
                int hoursLeft = moduleRecommendedStudyHours - getHoursStudied;
                // Will never return less than 0 (Goal met)
                return hoursLeft < 0 ? 0 : hoursLeft;
            }
        }


        // Returns the number of hours studied this week
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

                // Use linq to get all the dates from the dictionary
                var dates = moduleStudyDates.Where(kvp => kvp.Key >= monday && kvp.Key <= sunday)
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


                // Return the sum of the values (hours)
                return dates.Values.Sum();
            }
        }


        // Represents this object (Module) as a string for the combobox display
        public override string? ToString()
        {
            return moduleCode;
        }


        // Add for realtime UI updates
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
