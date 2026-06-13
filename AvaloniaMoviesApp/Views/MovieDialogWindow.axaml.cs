using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaMoviesApp.Models;

namespace AvaloniaMoviesApp.Views;

public partial class MovieDialogWindow : Window
{
    private Movie _movie;
    
    public MovieDialogWindow(Movie? movie = null)
    {
        InitializeComponent();
        
        if (movie != null)
        {
            _movie = movie;
            TitleBox.Text = _movie.Title;
            DirectorBox.Text = _movie.Director;
            YearBox.Value = Convert.ToDecimal(_movie.Year);
            GenreBox.Text = _movie.Genre;
            RatingSlider.Value = _movie.Rating;
            RatingTextBox.Text = _movie.Rating.ToString("F1");
        }
        else
        {
            _movie = new Movie();
        }
    
        // Синхронизация Slider -> TextBox
        RatingSlider.ValueChanged += (s, e) =>
        {
            RatingTextBox.Text = RatingSlider.Value.ToString("F1");
            RatingValue.Text = RatingSlider.Value.ToString("F1");
        };
    
        // Синхронизация TextBox -> Slider
        RatingTextBox.TextChanged += (s, e) =>
        {
            if (double.TryParse(RatingTextBox.Text, out double value))
            {
                value = Math.Max(0, Math.Min(10, value));
                RatingSlider.Value = value;
                RatingValue.Text = value.ToString("F1");
            }
        };
    
        RatingValue.Text = RatingSlider.Value.ToString("F1");
    
        SaveButton.Click += OnSaveClick;
        CancelButton.Click += (s, e) => Close(null);
    }
    
    private void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text))
        {
            return;
        }
        
        _movie.Title = TitleBox.Text;
        _movie.Director = DirectorBox.Text ?? "";
        _movie.Year = (int)(YearBox.Value ?? 2024m);
        _movie.Genre = GenreBox.Text ?? "";
        _movie.Rating = RatingSlider.Value;
        
        Close(_movie);
    }
}