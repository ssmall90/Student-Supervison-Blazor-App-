using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Student_Supervisor_Logic.Models
{
    [Serializable]
    public class PersonalSupervisorModel : IUserModel
    {

        private static int _nextId = LoadNextIdNumber();

        private static string ConfigFilePath
        {
            get
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string txtFileName = "LastSupervisorNumber.txt";

                // Navigating up multiple levels to reach the desired folder
                string parentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory)?.FullName)?.FullName)?.FullName)?.FullName)?.FullName;

                string txtFilePath = Path.Combine(parentDirectory, "Student Supervisor Data Access", "TextFiles", txtFileName);

                return txtFilePath;
            }
        }


        public static int NextSupervisorNumber
        {
            get { return _nextId; }
        }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string StaffNumber { get; set; }
        public string Email { get; set; }
        public int InteractionScore { get; set; }
        public int StudentWellBeingAverage { get; set; }
        public List<StudentModel> Students { get; set; }
        public List<AppointmentModel> ScheduledAppointments { get; set; }
        public List<AppointmentModel> AvailableAppointments { get; set; }
        public List<AppointmentModel> CompletedAppointments { get; set; }
        public List<AppointmentModel> IncompleteAppointments { get; set; }


        public PersonalSupervisorModel()
        {
            Students = new List<StudentModel>();
            ScheduledAppointments = new List<AppointmentModel> { };
            AvailableAppointments = new List<AppointmentModel> { };
            CompletedAppointments = new List<AppointmentModel> { };
            IncompleteAppointments = new List<AppointmentModel> { };
        }

        public PersonalSupervisorModel(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            StaffNumber = $"PS-00{_nextId++}";
            InteractionScore = 0;
            StudentWellBeingAverage = 0;
            Students = new List<StudentModel>();
            ScheduledAppointments = new List<AppointmentModel>();
            AvailableAppointments = new List<AppointmentModel>();
            CompletedAppointments = new List<AppointmentModel>();
            IncompleteAppointments = new List<AppointmentModel>();
            _nextId = _nextId++;
            SaveLastIdNumber(_nextId);
        }

        public void UpdateInteractionScore(int points)
        {
            InteractionScore += points;
        }

        public void UpdateAverageWellbeingScore()
        {
            int overall = 0;

            foreach (var student in Students)
            {
                overall += student.WellbeingScore;
            }

            overall = overall / Students.Count;

             StudentWellBeingAverage = overall;
        }

        public override string ToString()
        {
            return $"\r\nStaff Number: {StaffNumber} \r\nName: {FirstName} {LastName} \r\nInteraction Score: {InteractionScore} \r\nStudent Wellbeing Average: {StudentWellBeingAverage} \r\n";
        }

        private static int LoadNextIdNumber()
        {
            if (File.Exists(ConfigFilePath))
            {
                string content = File.ReadAllText(ConfigFilePath);

                if (int.TryParse(content, out int number))
                {
                    return number;
                }
            }
            return 0;
        }

        private static void SaveLastIdNumber(int number)
        {
            File.WriteAllText(ConfigFilePath, number.ToString());
        }


    }
}
