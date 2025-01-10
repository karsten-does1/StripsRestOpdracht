using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StripsDL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StripsBL.Services
{
    public class DataImportService
    {
        private readonly StripsDbContext _context;
        private readonly ILogger<DataImportService> _logger;

        public DataImportService(StripsDbContext context, ILogger<DataImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void ImportData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Bestand niet gevonden: {FilePath}", filePath);
                return;
            }

            _logger.LogInformation("Start data-import vanaf: {FilePath}", Path.GetFullPath(filePath));
            var lines = File.ReadAllLines(filePath);
            int importedCount = 0;

            foreach (var line in lines)
            {
                try
                {
                    var fields = line.Split(';');
                    if (fields.Length < 5)
                    {
                        _logger.LogWarning("Ongeldige data: {Line}", line);
                        continue;
                    }

                    
                    int reeksNummer = int.TryParse(fields[0], out var num) ? num : 0;
                    string titel = fields[1];
                    string uitgeverijNaam = fields[2];
                    string reeksNaam = fields[3];
                    string[] auteursNamen = fields[4].Split('|');

                    var uitgeverij = _context.Uitgeverijens.FirstOrDefault(u => u.Naam == uitgeverijNaam)
                                     ?? new Uitgeverijen { Naam = uitgeverijNaam, Adres = "Onbekend" };
                    if (uitgeverij.Id == 0) _context.Uitgeverijens.Add(uitgeverij);

                    
                    var reeks = _context.Reeksens.FirstOrDefault(r => r.Naam == reeksNaam)
                                ?? new Reeksen { Naam = reeksNaam };
                    if (reeks.Id == 0) _context.Reeksens.Add(reeks);


                    var auteurs = new List<Auteur>();
                    foreach (var naam in auteursNamen)
                    {
                        if (string.IsNullOrWhiteSpace(naam)) continue;

                        var email = $"{naam.Trim()}@example.com";

                     
                        var auteur = _context.Auteurs.FirstOrDefault(a => a.Email == email);
                        if (auteur == null)
                        {
                    
                            auteur = new Auteur { Naam = naam.Trim(), Email = email };
                            _context.Auteurs.Add(auteur);
                            _context.SaveChanges(); 
                        }

                        auteurs.Add(auteur);
                    }
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                    {
                     
                        _logger.LogWarning("Duplicate auteur gedetecteerd en overgeslagen.");
                    }

                    int reeksId = reeks?.Id ?? 0;

                    var bestaandeStrip = _context.Strips.FirstOrDefault(s =>
                        s.Titel == titel &&
                        s.Uitgeverij.Id == uitgeverij.Id &&
                        (s.Reeks != null ? s.Reeks.Id : 0) == reeksId &&
                        s.ReeksNummer == reeksNummer);

                    if (bestaandeStrip == null)
                    {
                       
                        var strip = new Strip
                        {
                            Titel = titel,
                            Uitgeverij = uitgeverij,
                            Reeks = reeks,
                            ReeksNummer = reeksNummer,
                            Auteurs = auteurs
                        };

                        _context.Strips.Add(strip);
                        importedCount++;
                    }
                    else
                    {
                        _logger.LogInformation("Duplicate strip overgeslagen: {Titel}", titel);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fout bij verwerken van rij: {Line}", line);
                }
            }

            try
            {
                _context.SaveChanges();
                _logger.LogInformation("Data succesvol geïmporteerd: {Count} rijen", importedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij opslaan van data.");
            }
        }
    }
}