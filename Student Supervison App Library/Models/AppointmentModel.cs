using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Student_Supervisor_Logic.Models
{
    public class AppointmentModel
    {


        private static int _nextId = LoadNextIdNumber();

        private static string ConfigFilePath
        {
            get
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string txtFileName = "LastAppointment.txt";

                string parentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory)?.FullName)?.FullName)?.FullName)?.FullName)?.FullName;

                string txtFilePath = Path.Combine(parentDirectory, "Student Supervisor Data Access", "TextFiles", txtFileName);

                return txtFilePath;
            }
        }

        public static int NextAppointmnetId
        {
            get { return _nextId; }
        }

        public string AppointmentId { get; set; }
        public string Date {  get; set; }
        public string Time { get; set; }
        public string Duration { get; set; }
        public string StudentAttending { get; set; }


        public AppointmentModel()
        {
            
        }

        public AppointmentModel(string date, string time, string duration)
        {
            AppointmentId = $"APP-{_nextId++}";
            Date = date;
            Time = time;
            Duration = duration;
            StudentAttending = string.Empty;
            _nextId = _nextId++;
            SaveLastIdNumber(_nextId);
        }

        public override string ToString()
        {
            return $"\r\n Appointment Id: {AppointmentId} \r\nDate: {Date}\r\nTime: {Time}\r\nDuration: {Duration}\r\nStudent Number Of Attendee: {StudentAttending}\r\n";
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
