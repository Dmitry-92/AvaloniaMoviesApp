namespace AvaloniaMoviesApp.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public int Year { get; set; } = 2024; // значение по умолчанию
    public string Genre { get; set; } = string.Empty;
    public double Rating { get; set; } // 0-10
}