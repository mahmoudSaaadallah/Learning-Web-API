using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApiLearning.DataAccess.Data;
using WebApiLearning.DataAccess.Repository;
using WebApiLearning.Model.IRepository;
namespace WebApiLearning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Now let's make this application could be used with other client side applications after we
            //    adding the pipeline app.UseCors();
            // We will add this client side applications using builder.
            builder.Services.AddCors(CorsOptions =>
            {
                CorsOptions.AddPolicy("MyCors", CorsPolicybuilder =>
                {
                    //CorsPolicybuilder.WithOrigins("Adding Url For Client Side Here"); // This will give access for specific Client applications.

                    // Also I could make the access open for any Client Side applications using 
                    CorsPolicybuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();// This will allow any client side applications to access my api application.

                    // also we could make the access open for any client side applications to use GET methods
                    //   only, so the client application will not be able to add data to data base, but he
                    //   will be able to retrieve data from data base depend on the get methods in controllers.
                    //CorsPolicybuilder.AllowAnyOrigin().WithMethods("Get");


                });
            }); // Adds cross-origin resource sharing services to the specified

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // If I want to make this web application apply to work with static files like HTML, CSS,
            //   JavaScript, images and so on, then we have to allow this in pipeline.
            app.UseStaticFiles();// This pipeline allows static files to be used with this api.

            // Now also If I want to make the application could be used in different client like make this
            //   API server work with anther frontEnd client, then we have to add this in pipeline here.

            app.UseCors("MyCors"); // UseCors allows CORS to be used in the pipeline and allows clients to use this API
            // We have to know that I could give the ability to access this api for specific client side
            //   applications, This must happen after we add pipeline we have to back again to builder and
            //   add client side application that could access this api.

            app.UseAuthorization();
          


            app.MapControllers();

            app.Run();
        }
    }
}
