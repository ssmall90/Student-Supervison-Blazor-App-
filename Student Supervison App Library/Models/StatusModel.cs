using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Student_Supervisor_Logic.Models
{
    public class StatusModel
    {

        private static string ConfigFilePath
        {
            get
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string txtFileName = "LastStatusNumber.txt";

                string parentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory)?.FullName)?.FullName)?.FullName)?.FullName)?.FullName;

                string txtFilePath = Path.Combine(parentDirectory, "Student Supervison App Library", "TextFiles", txtFileName);

                return txtFilePath;
            }
        }

        private static int _nextId = LoadNextIdNumber();



        public static int NextStatusId
        {
            get { return _nextId; }
        }

        public string StatusId { get; set; }
        public string StatusDate { get; set; }
        public string StatusComments { get; set; }
        public int StatusScore { get; set; }
        public StatusModel()
        {
            
        }

        public StatusModel(string statusComments, int statusScore)
        {
            StatusComments = statusComments;
            StatusScore = statusScore;
            StatusDate = DateOnly.FromDateTime(DateTime.Now).ToString();
            StatusId = $"STAT-00{_nextId++}";
            _nextId = _nextId++;
            SaveLastIdNumber(_nextId);
        }

        public override string ToString()
        {
            return $"\r\nID: {StatusId} \r\nDate: {StatusDate.ToString()} \r\nComments: {StatusComments} \r\nWellbeing Score: {StatusScore}\r\n";
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
