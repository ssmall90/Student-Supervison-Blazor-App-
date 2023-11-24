using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Student_Supervisor_Data_Access.DataManager.DataManager;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using Student_Supervisor_Logic.Models;

namespace Student_Supervisor_Data_Access.DataManager
{
    public class DataManager : IDataManager
    {
        public List<SeniorTutorModel>? CurrentData { get; set; }
        public string? filePathForXml { get; set; }


        #region Data Loading
        public List<SeniorTutorModel> DeserializeSeniorTutorModelFromFile(string filePath)
        {

            List<SeniorTutorModel> tutors = new List<SeniorTutorModel>();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SeniorTutorModel));

                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SeniorTutorModel")
                        {

                            SeniorTutorModel seniorTutor = (SeniorTutorModel)serializer.Deserialize(reader.ReadSubtree())!;
                            tutors.Add(seniorTutor);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null!;
            }

            return tutors;
        }
        #endregion



        #region Login Management
        public StudentModel GetStudentAndAssignPersonalSupervisor(string studentNumber, List<SeniorTutorModel> tutors)
        {
            

            PersonalSupervisorModel pSupervisor = null;

            foreach (var seniorTutor in tutors)
            {
                foreach (var personalSupervisor in seniorTutor.PersonalSupervisors)
                {
                    var student = personalSupervisor.Students.Find(s => s.StudentNumber == studentNumber);

                    if (student != null)
                    {
                        student.PersonalSupervisor = personalSupervisor;
                        return student;
                    }

                }
            }

            return null;
        }

        public PersonalSupervisorModel GetPersonalSupervisor(string staffNumber, List<SeniorTutorModel> tutors)
        {
            PersonalSupervisorModel personalSupervisor = null;

            foreach (var seniorTutor in tutors)
            {
                foreach (var pS in seniorTutor.PersonalSupervisors)
                {
                    if (pS.StaffNumber == staffNumber)
                    {
                        personalSupervisor = pS;
                        return personalSupervisor;

                    }

                }
            }

            return null;

        }

        public SeniorTutorModel GetSeniorTutor(string staffNumber, List<SeniorTutorModel> seniorTutors)
        {
            SeniorTutorModel seniorTutor = null;

            foreach (var tutor in seniorTutors)
            {

                if (tutor.StaffNumber == staffNumber)
                {
                    seniorTutor = tutor;
                    break;

                }

            }
            if (seniorTutor != null)
            {
                return seniorTutor;
            }
            else
            {
                return null;
            }
        }
        #endregion



        #region Status Management
        public void AppendStatusToStudentInXml(string studentNumber, StatusModel status, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement studentElement = doc.Root!.Descendants("StudentModel")
                    .FirstOrDefault(s => s.Element("StudentNumber")!.Value == studentNumber)!;

                if (studentElement != null)
                {
                    XElement statusesElement = studentElement.Element("Statuses");

                    if (statusesElement == null)
                    {
                        statusesElement = new XElement("Statuses");
                        studentElement.Add(statusesElement);
                    }

                    XElement statusElement = new XElement("StatusModel",
                        new XElement("StatusId", status.StatusId),
                        new XElement("StatusDate", status.StatusDate.ToString("yyyy-MM-dd")),
                        new XElement("StatusComments", status.StatusComments),
                        new XElement("StatusScore", status.StatusScore));

                    statusesElement.Add(statusElement);

                    doc.Save(filePath);

                    Console.WriteLine("Status Appended To The Student In The XML File Successfully.");
                }
                else
                {
                    Console.WriteLine("Student With The Specified Student Number Was Not Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void UpdateStudentWellBeingScoreInXml(string studentNumber, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement studentElement = doc.Root!.Descendants("StudentModel")
                    .FirstOrDefault(s => s.Element("StudentNumber")!.Value == studentNumber)!;

                if (studentElement != null)
                {
                    XElement statusesElement = studentElement.Element("Statuses");

                    if (statusesElement != null)
                    {
                        XElement lastStatusElement = statusesElement.Elements("StatusModel").LastOrDefault();

                        if (lastStatusElement != null)
                        {
                            int lastStatusScore = int.Parse(lastStatusElement.Element("StatusScore")!.Value);
                            studentElement.Element("WellbeingScore")!.Value = lastStatusScore.ToString();

                            doc.Save(filePath);

                            Console.WriteLine("Student's Well-being Score Updated Successfully.");
                        }
                        else
                        {
                            Console.WriteLine("No Statuses Found For The Student.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Statuses Found For The Student.");
                    }
                }
                else
                {
                    Console.WriteLine("Student With The Specified Student Number Was Not Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void UpdatePersonalSupervisorWellBeingAverageInXml(string supervisorStaffNumber, int newWellBeingAverage, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == supervisorStaffNumber)!;

                if (supervisorElement != null)
                {
                    supervisorElement.Element("StudentWellBeingAverage")!.Value = newWellBeingAverage.ToString();

                    doc.Save(filePath);

                    Console.WriteLine("Personal Supervisor's Well-being Average Updated Successfully.");
                }
                else
                {
                    Console.WriteLine("Personal Supervisor With The Specified Staff Number Was Not Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        #endregion



        #region Appointment Management
        public void ScheduleAppointmentForSupervisor(string supervisorStaffNumber, string appointmentId, string studentNumber, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == supervisorStaffNumber)!;

                if (supervisorElement != null)
                {

                    XElement availableAppointmentsElement = supervisorElement
                        .Element("AvailableAppointments")!;

                    XElement appointmentElement = availableAppointmentsElement
                        .Elements("AppointmentModel")
                        .FirstOrDefault(a => a.Element("AppointmentId")!.Value == appointmentId.ToString())!;


                    XElement studentElement = supervisorElement
                        .Element("Students")!;

                    XElement student = studentElement
                        .Elements("StudentModel")
                        .FirstOrDefault(s => s.Element("StudentNumber")!.Value == studentNumber.ToString())!;



                    if (appointmentElement != null)
                    {
                        appointmentElement.Remove();

                        appointmentElement.Element("StudentAttending")!.Value = studentNumber;

                        XElement scheduledAppointmentsElement = supervisorElement
                            .Element("ScheduledAppointments")!;
                        scheduledAppointmentsElement.Add(appointmentElement);

                        XElement scheduledApointmentsElementStudent = student
                            .Element("ScheduledAppointments")!;
                        scheduledApointmentsElementStudent.Add(appointmentElement);


                        doc.Save(filePath);
                        Console.WriteLine("Appointment Scheduled Successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Appointment with the specified ID not found in available appointments.");
                    }
                }
                else
                {
                    Console.WriteLine("Supervisor with the specified staff number not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void UpdateInteractionScore(string supervisorStaffNumber, int interactionScore, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == supervisorStaffNumber)!;

                XElement interactionScoreElement = supervisorElement.Element("InteractionScore")!;

                if (interactionScoreElement != null)
                {
                    interactionScoreElement.Value = interactionScore.ToString();

                    doc.Save(filePath);

                }
                else
                {
                    Console.WriteLine("Interaction Score Could Not Be Updated.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        public void AppendAppointmentToAvailableAppointmentsXml(string staffNumber, AppointmentModel appointment, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == staffNumber)!;

                if (supervisorElement != null)
                {
                    XElement availableAppointmentsElement = supervisorElement
                        .Element("AvailableAppointments")!;

                    XElement newAppointmentElement = new XElement("AppointmentModel");
                    newAppointmentElement.Add(new XElement("AppointmentId", appointment.AppointmentId));
                    newAppointmentElement.Add(new XElement("Date", appointment.Date));
                    newAppointmentElement.Add(new XElement("Time", appointment.Time));
                    newAppointmentElement.Add(new XElement("Duration", appointment.Duration));

                    newAppointmentElement.Add(new XElement("StudentAttending", appointment.StudentAttending));

                    availableAppointmentsElement.Add(newAppointmentElement);

                    doc.Save(filePath);

                    //Console.WriteLine("Appointment Appended to Available Appointments Successfully.");
                }
                else
                {
                    Console.WriteLine("Supervisor With The Specified Staff Number Was Not Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void RemoveAvailableAppointmentFromXML(string staffNumber, string appointmentId, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == staffNumber)!;

                if (supervisorElement != null)
                {
                    XElement availableAppointmentsElement = supervisorElement.Element("AvailableAppointments")!;

                    if (availableAppointmentsElement != null)
                    {

                        XElement appointmentToRemove = availableAppointmentsElement.Elements("AppointmentModel")
                            .FirstOrDefault(appointment => appointment.Element("AppointmentId")!.Value == appointmentId)!;

                        if (appointmentToRemove != null)
                        {
                            appointmentToRemove.Remove();

                            doc.Save(filePath);

                            //Console.WriteLine("Appointment Removed from Available Appointments Successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Appointment With The Specified Appointment Id Could Not Be Found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Available Appointments Found For The Supervisor.");
                    }
                }
                else
                {
                    Console.WriteLine("Supervisor With The Specified Staff Number Could Not Be Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void AppendAppointmentCompletedAppointmentsXml(string staffNumber, AppointmentModel appointment, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == staffNumber)!;

                if (supervisorElement != null)
                {
                    XElement completedAppointmentsElement = supervisorElement
                        .Element("CompletedAppointments")!;

                    XElement newAppointmentElement = new XElement("AppointmentModel");
                    newAppointmentElement.Add(new XElement("AppointmentId", appointment.AppointmentId));
                    newAppointmentElement.Add(new XElement("Date", appointment.Date));
                    newAppointmentElement.Add(new XElement("Time", appointment.Time));
                    newAppointmentElement.Add(new XElement("Duration", appointment.Duration));

                    newAppointmentElement.Add(new XElement("StudentAttending", appointment.StudentAttending));

                    completedAppointmentsElement.Add(newAppointmentElement);

                    doc.Save(filePath);

                    Console.WriteLine("Appointment Appended to Completed Appointments Successfully.");
                }
                else
                {
                    Console.WriteLine("Supervisor With The Specified Staff Number Was Not Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void AppendAppointmentInCompleteAppointmentsXml(string staffNumber, AppointmentModel appointment, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == staffNumber)!;

                if (supervisorElement != null)
                {
                    XElement incompleteAppointmentsElement = supervisorElement
                        .Element("IncompleteAppointments")!;

                    XElement newAppointmentElement = new XElement("AppointmentModel");
                    newAppointmentElement.Add(new XElement("AppointmentId", appointment.AppointmentId));
                    newAppointmentElement.Add(new XElement("Date", appointment.Date));
                    newAppointmentElement.Add(new XElement("Time", appointment.Time));
                    newAppointmentElement.Add(new XElement("Duration", appointment.Duration));

                    newAppointmentElement.Add(new XElement("StudentAttending", appointment.StudentAttending));

                    incompleteAppointmentsElement.Add(newAppointmentElement);

                    doc.Save(filePath);

                    Console.WriteLine("Appointment Appended to Incomplete Appointments Successfully.");
                }
                else
                {
                    Console.WriteLine("Supervisor With The Specified Staff Number Was Not Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void RemoveScheduledAppointmentFromXML(string staffNumber, string appointmentId, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement supervisorElement = doc.Root!.Descendants("PersonalSupervisorModel")
                    .FirstOrDefault(s => s.Element("StaffNumber")!.Value == staffNumber)!;

                if (supervisorElement != null)
                {
                    XElement scheduledAppointmentsElement = supervisorElement.Element("ScheduledAppointments")!;

                    if (scheduledAppointmentsElement != null)
                    {

                        XElement appointmentToRemove = scheduledAppointmentsElement.Elements("AppointmentModel")
                            .FirstOrDefault(appointment => appointment.Element("AppointmentId")!.Value == appointmentId)!;

                        if (appointmentToRemove != null)
                        {
                            appointmentToRemove.Remove();

                            doc.Save(filePath);

                            //Console.WriteLine("Appointment Removed from Available Appointments Successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Appointment With The Specified Appointment Id Could Not Be Found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Available Appointments Found For The Supervisor.");
                    }
                }
                else
                {
                    Console.WriteLine("Supervisor With The Specified Staff Number Could Not Be Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void RemoveScheduledAppointmentForStudentFromXML(string studentNumber, string appointmentId, string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement studentElement = doc.Root!.Descendants("StudentModel")
                    .FirstOrDefault(s => s.Element("StudentNumber")!.Value == studentNumber)!;

                if (studentElement != null)
                {
                    XElement scheduledAppointmentsElement = studentElement.Element("ScheduledAppointments")!;

                    if (scheduledAppointmentsElement != null)
                    {

                        XElement appointmentToRemove = scheduledAppointmentsElement.Elements("AppointmentModel")
                            .FirstOrDefault(appointment => appointment.Element("AppointmentId")!.Value == appointmentId)!;

                        if (appointmentToRemove != null)
                        {
                            appointmentToRemove.Remove();

                            doc.Save(filePath);

                            //Console.WriteLine("Appointment Removed from Available Appointments Successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Appointment With The Specified Appointment Id Could Not Be Found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Available Appointments Found For The Supervisor.");
                    }
                }
                else
                {
                    Console.WriteLine("Student With The Specified Student Number Could Not Be Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        #endregion


    }
}
