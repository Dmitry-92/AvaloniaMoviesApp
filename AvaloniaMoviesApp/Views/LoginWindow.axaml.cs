using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaMoviesApp.Models;
using System.Threading.Tasks;

namespace AvaloniaMoviesApp.Views;

public partial class LoginWindow : Window
{
    private readonly MovieRepository _repository;
    public User? CurrentUser { get; private set; }
    
    public LoginWindow()
    {
        InitializeComponent();
        _repository = new MovieRepository();
        
        LoginButton.Click += OnLoginClick;
    }
    
    private async void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        var username = UsernameBox.Text?.Trim();
        var email = EmailBox.Text?.Trim();
        
        if (string.IsNullOrWhiteSpace(username))
        {
            StatusText.Text = "⚠️ Введите имя пользователя";
            return;
        }
        
        // Ищем пользователя
        var user = await _repository.GetUserByUsernameAsync(username);
        
        if (user == null)
        {
            // Создаём нового
            user = await _repository.CreateUserAsync(username, email ?? $"{username}@example.com");
            StatusText.Text = "✅ Новый пользователь создан!";
        }
        else
        {
            StatusText.Text = $"✅ Добро пожаловать, {user.Username}!";
        }
        
        CurrentUser = user;
        await Task.Delay(300); // небольшая задержка для отображения статуса
        Close(user);
    }
}