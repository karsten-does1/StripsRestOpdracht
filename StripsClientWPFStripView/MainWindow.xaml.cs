using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.Mvc;
using StripsBL.Services;
using StripsDL.Models;
using StripsRest.DTOs;


namespace StripsClientWPFStripView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7257/api/") };
        }

        private async void GetStripButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IdTextBox.Text, out int id))
            {
                try
                {
                    var strip = await GetStripByIdAsync(id);
                    if (strip != null)
                    {
                        // Update UI
                        TitelTextBox.Text = strip.Titel;
                        NrTextBox.Text = strip.Nr.ToString();
                        ReeksTextBox.Text = strip.Reeks ?? "N/A";
                        UitgeverijTextBox.Text = strip.Uitgeverij ?? "N/A";

                        AuteursListBox.Items.Clear();
                        foreach (var auteur in strip.Auteurs)
                        {
                            AuteursListBox.Items.Add(auteur.Auteur);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Strip not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching strip: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task<StripDto?> GetStripByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Strips/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StripDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }
    }
}