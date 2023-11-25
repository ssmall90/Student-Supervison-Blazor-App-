using System.Data.Common;
using System.Data;
using Student_Supervisor_Data_Access.DataManager;

namespace Student_Supervison_App_UI
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<IDataManager, DataManager>();
           

        }
    }
}
