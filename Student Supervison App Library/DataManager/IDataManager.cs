using Student_Supervisor_Logic.Models;

namespace Student_Supervisor_Data_Access.DataManager
{
    public interface IDataManager
    {
        List<SeniorTutorModel>? CurrentData { get; set; }
        string? filePathForXml { get; set; }

        void AppendAppointmentCompletedAppointmentsXml(string staffNumber, AppointmentModel appointment, string filePath);
        void AppendAppointmentInCompleteAppointmentsXml(string staffNumber, AppointmentModel appointment, string filePath);
        void AppendAppointmentToAvailableAppointmentsXml(string staffNumber, AppointmentModel appointment, string filePath);
        void AppendStatusToStudentInXml(string studentNumber, StatusModel status, string filePath);
        List<SeniorTutorModel> DeserializeSeniorTutorModelFromFile(string filePath);
        PersonalSupervisorModel GetPersonalSupervisor(string staffNumber, List<SeniorTutorModel> tutors);
        SeniorTutorModel GetSeniorTutor(string staffNumber, List<SeniorTutorModel> seniorTutors);
        StudentModel GetStudentAndAssignPersonalSupervisor(string studentNumber, List<SeniorTutorModel> tutors);
        void RemoveAvailableAppointmentFromXML(string staffNumber, string appointmentId, string filePath);
        void RemoveScheduledAppointmentForStudentFromXML(string studentNumber, string appointmentId, string filePath);
        void RemoveScheduledAppointmentFromXML(string staffNumber, string appointmentId, string filePath);
        void ScheduleAppointmentForSupervisor(string supervisorStaffNumber, string appointmentId, string studentNumber, string filePath);
        void UpdateInteractionScore(string supervisorStaffNumber, int interactionScore, string filePath);
        void UpdatePersonalSupervisorWellBeingAverageInXml(string supervisorStaffNumber, int newWellBeingAverage, string filePath);
        void UpdateStudentWellBeingScoreInXml(string studentNumber, string filePath);
    }
}