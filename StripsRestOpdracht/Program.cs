using Microsoft.EntityFrameworkCore;
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

            // Add services to the container.

            builder.Services.AddDbContext<StripsDbContext>(options =>
               options.UseSqlServer("Data Source=MSI\\SQLEXPRESS01;Initial Catalog=StripsDb;Integrated Security=True;Trust Server Certificate=True"));


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
                try
                {
                    var context = services.GetRequiredService<StripsDbContext>();
                    if (!context.Strips.Any())
                    {
                        var importer = new DataImporter(context);

                        // Geef het pad naar je TXT-bestand
                        string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "StripsDL", "stripsData.txt");
                        Console.WriteLine($"Importeren vanaf: {Path.GetFullPath(filePath)}");

                        if (File.Exists(filePath))
                        {
                            importer.ImportData(filePath);
                         Console.WriteLine("Data succesvol geïmporteerd!");
                        }
                        else
                        {
                            Console.WriteLine("Bestand niet gevonden!");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Data is al aanwezig, importeren overgeslagen.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Data importeren mislukt: {ex.Message}");
                }
            }

            app.Run();
        }
    }
}
