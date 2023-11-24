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

            //builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
            //builder.Services.AddMemoryCache();
            //builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
            //builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            //.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Admin", policy =>
            //    {
            //        policy.RequireClaim("jobTitle", "Admin");
            //    });
            //});


        }
    }
}
