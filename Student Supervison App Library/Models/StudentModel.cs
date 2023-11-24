using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Student_Supervisor_Logic.Models
{
    public class StudentModel : IUserModel
    {
        private static int _nextId = LoadNextIdNumber();

        private static string ConfigFilePath
        {
            get
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string txtFileName = "LastStudentNumber.txt";

                string parentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory)?.FullName)?.FullName)?.FullName)?.FullName)?.FullName;

                string txtFilePath = Path.Combine(parentDirectory, "Student Supervison App Library", "TextFiles", txtFileName);

                return txtFilePath;
            }
        }

        public static int NextAppointmnetId
        {
            get { return _nextId; }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string Email { get; set; }
        public int WellbeingScore { get; set; }
        public List<StatusModel> Statuses { get; set; }
        public List<AppointmentModel> ScheduledAppointments { get; set; }
        public PersonalSupervisorModel PersonalSupervisor { get; set; }

        public StudentModel()
        {
            Statuses = new List<StatusModel>();
            ScheduledAppointments = new List<AppointmentModel>();
            PersonalSupervisor = new PersonalSupervisorModel();

        }

        public StudentModel(string firstName, string lastName, string email, PersonalSupervisorModel personalSupervisor)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            StudentNumber = $"ST00-{_nextId++}";
            WellbeingScore = 0;
            Statuses = new List<StatusModel>();
            ScheduledAppointments = new List<AppointmentModel>();
            PersonalSupervisor = personalSupervisor;
            _nextId = _nextId++;
            SaveLastIdNumber(_nextId);

        }

        public int CalculateStatusScore(int score1, int score2, int score3)
        {
            return score1 + score2 + score3;
        }

        public override string ToString()
        {
            return $"\r\nName: {FirstName} {LastName} \r\nStudent Number: {StudentNumber} \r\nWellbeing Score: {WellbeingScore} \r\n";
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
