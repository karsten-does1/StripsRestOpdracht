using Microsoft.EntityFrameworkCore;
using StripsDL.Models;
namespace StripsDL
{
    
        public class DataImporter
        {
            private readonly StripsDbContext _context;

            public DataImporter(StripsDbContext context)
            {
                _context = context;
            }

            public void ImportData(string filePath)
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Bestand niet gevonden!");
                    return;
                }

                var lines = File.ReadAllLines(filePath);
                int importedCount = 0;

                var auteurCache = new Dictionary<string, Auteur>();
                var reeksCache = new Dictionary<string, Reeksen>();

                foreach (var line in lines)
                {
                    try
                    {
                        Console.WriteLine($"Verwerken: {line}");

                        var fields = line.Split(';');
                        if (fields.Length < 5)
                        {
                            Console.WriteLine($"Ongeldige data: {line}");
                            continue;
                        }

                        int reeksNummer = 0;
                        if (!int.TryParse(fields[0], out reeksNummer))
                        {
                            Console.WriteLine($"Ongeldig ReeksNummer: {fields[0]}, standaardwaarde gebruikt.");
                            reeksNummer = 0;
                        }

                        string titel = fields[1];
                        string uitgeverijNaam = fields[2];
                        string reeksNaam = fields[3];
                        string[] auteursNamen = fields[4].Split('|');

                        var uitgeverij = _context.Uitgeverijens.FirstOrDefault(u => u.Naam == uitgeverijNaam)
                                         ?? new Uitgeverijen { Naam = uitgeverijNaam, Adres = "Onbekend" };
                        if (uitgeverij.Id == 0) _context.Uitgeverijens.Add(uitgeverij);

                        Reeksen? reeks = null;
                        if (!reeksCache.TryGetValue(reeksNaam, out reeks)) 
                        {
                            reeks = _context.Reeksens.FirstOrDefault(r => r.Naam == reeksNaam);
                            if (reeks == null)
                            {
                                reeks = new Reeksen { Naam = reeksNaam };
                                _context.Reeksens.Add(reeks);
                            }

                            reeksCache[reeksNaam] = reeks;
                        }

                        var auteurs = new List<Auteur>();
                        foreach (var naam in auteursNamen)
                        {
                            if (string.IsNullOrWhiteSpace(naam)) continue;

                            var email = $"{naam.Trim()}@example.com";
                            var auteur = auteurCache.TryGetValue(email, out var cachedAuteur)
                                ? cachedAuteur
                                : _context.Auteurs.FirstOrDefault(a => a.Email == email) ?? new Auteur { Naam = naam.Trim(), Email = email };

                            if (auteur.Id == 0)
                            {
                                _context.Auteurs.Add(auteur);
                            }
                            auteurCache[email] = auteur;
                            auteurs.Add(auteur);
                        }

                        var bestaandeStrip = _context.Strips.FirstOrDefault(s =>
                            s.Titel == titel &&
                            s.Uitgeverij.Id == uitgeverij.Id &&
                            (s.Reeks == null && reeks == null || s.Reeks != null && reeks != null && s.Reeks.Id == reeks.Id) &&
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

                            try
                            {
                                _context.SaveChanges();
                                importedCount++;
                                Console.WriteLine($"Strip '{titel}' succesvol geïmporteerd.");
                            }
                            catch (DbUpdateException ex)
                            {
                                Console.WriteLine($"Duplicate detected: Strip '{titel}' wordt overgeslagen.");
                                if (ex.InnerException != null)
                                {
                                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                                }
                                _context.ChangeTracker.Clear(); }
                        }
                        else
                        {
                            Console.WriteLine($"Duplicate detected: Strip '{titel}' met ReeksNummer '{reeksNummer}' wordt overgeslagen.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Fout bij verwerken van rij: {line}");
                        Console.WriteLine($"Foutmelding: {ex.Message}");
                    }
                }

                Console.WriteLine($"Data import voltooid. Totaal aantal geïmporteerde rijen: {importedCount}");
            }
        }
    }

