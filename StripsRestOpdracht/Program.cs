using Microsoft.EntityFrameworkCore;
using StripsBL.Services;
using StripsDL;
using StripsDL.Models;
using System.IO;

namespace StripsRest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddDbContext<StripsDbContext>(options =>
                options.UseSqlServer("Data Source=MSI\\SQLEXPRESS01;Initial Catalog=StripsDb;Integrated Security=True;Trust Server Certificate=True"));

            builder.Services.AddScoped<StripService>();
            builder.Services.AddScoped<ReeksenService>();
            builder.Services.AddScoped<UitgeverijenService>();
            builder.Services.AddScoped<AuteursService>();
            builder.Services.AddScoped<DataImportService>();

            builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
     });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var importService = services.GetRequiredService<DataImportService>();

                   
                    string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "StripsDL", "stripsData.txt");
                    logger.LogInformation("Start data-import vanaf: {FilePath}", Path.GetFullPath(filePath));

                    importService.ImportData(filePath);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Er is een fout opgetreden tijdens het importeren van de data.");
                }
            }

            app.Run();
        }
    }
}
