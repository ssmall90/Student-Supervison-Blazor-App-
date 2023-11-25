using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Supervison_App_Library
{
    public static class FilePathConstants
    {
        public static string ConstructPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "database.xml";
            string parentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory)?.FullName)?.FullName)?.FullName)?.FullName)?.FullName;
            string txtFilePath = Path.Combine(parentDirectory, "Student Supervison App Library", "XmlFiles", fileName);
            return txtFilePath;
        }
    }


}
