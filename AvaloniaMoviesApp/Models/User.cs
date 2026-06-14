using System.Collections.Generic;

namespace AvaloniaMoviesApp.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    //Навигационное свойство: отзывы пользователя
    //public List<Review> Reviews { get; set; } = new();
}