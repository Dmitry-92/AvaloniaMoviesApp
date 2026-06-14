using System;

namespace AvaloniaMoviesApp.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; } // 1-5 звёзд
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Внешние ключи
    public int MovieId { get; set; }
    public int UserId { get; set; }
    
    // Навигационные свойства
    public Movie? Movie { get; set; }
    public User? User { get; set; }
}git