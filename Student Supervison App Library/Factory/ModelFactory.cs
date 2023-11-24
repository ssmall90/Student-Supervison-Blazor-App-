using Student_Supervisor_Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Supervisor_Logic.Factory
{
    public static class ModelFactory
    {
        public static StatusModel CreateStatus(string statusComment, int statusScore)
        {

            return new StatusModel(statusComment,statusScore);
        }
        public static AppointmentModel CreateAppointment(string date, string time, string duration)
        {

            return new AppointmentModel(date, time, duration);
        }



    }
}
