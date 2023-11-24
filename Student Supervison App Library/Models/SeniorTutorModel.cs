using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Student_Supervisor_Logic.Models
{
    [XmlRoot("SeniorTutorModel")]
    public class SeniorTutorModel : IUserModel
    {
        private static int _nextId = LoadNextIdNumber();

        private static string ConfigFilePath
        {
            get
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string txtFileName = "LastSeniorTutorNumber.txt";

                // Navigating up multiple levels to reach the desired folder
                string parentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory)?.FullName)?.FullName)?.FullName)?.FullName)?.FullName;

                string txtFilePath = Path.Combine(parentDirectory, "Student Supervisor Data Access", "TextFiles", txtFileName);

                return txtFilePath;
            }
        }

        public static int NextAppointmnetId
        {
            get { return _nextId; }
        }

        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string StaffNumber { get; set; }
        public string Email { get; set; }

        public List<PersonalSupervisorModel> PersonalSupervisors { get; set; }


        public SeniorTutorModel()
        {
            PersonalSupervisors = new List<PersonalSupervisorModel>();
        }

        public SeniorTutorModel(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PersonalSupervisors = new List<PersonalSupervisorModel>();
            _nextId = _nextId++;
            SaveLastIdNumber(_nextId);
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
