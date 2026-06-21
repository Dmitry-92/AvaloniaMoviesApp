using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaMoviesApp.Models;
using AvaloniaMoviesApp.Services;

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
        var password = PasswordBox.Text;
        var confirmPassword = ConfirmPasswordBox.Text;
        
        // Проверка обязательных полей
        if (string.IsNullOrWhiteSpace(username))
        {
            StatusText.Text = "⚠️ Введите имя пользователя";
            return;
        }
        
        if (string.IsNullOrWhiteSpace(password))
        {
            StatusText.Text = "⚠️ Введите пароль";
            return;
        }
        
        // Ищем пользователя в БД
        var existingUser = await _repository.GetUserByUsernameAsync(username);
        
        if (existingUser != null)
        {
            // ===== ВХОД СУЩЕСТВУЮЩЕГО ПОЛЬЗОВАТЕЛЯ =====
            if (PasswordHasher.VerifyPassword(password, existingUser.PasswordHash))
            {
                CurrentUser = existingUser;
                StatusText.Text = $"✅ Добро пожаловать, {existingUser.Username}!";
                await Task.Delay(300);
                Close(existingUser);
            }
            else
            {
                StatusText.Text = "❌ Неверный пароль!";
            }
        }
        else
        {
            // ===== РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ =====
            if (password != confirmPassword)
            {
                StatusText.Text = "❌ Пароли не совпадают!";
                return;
            }
            
            if (password.Length < 4)
            {
                StatusText.Text = "❌ Пароль должен содержать минимум 4 символа";
                return;
            }
            
            // Хешируем пароль и создаём пользователя
            var passwordHash = PasswordHasher.HashPassword(password);
            var newUser = new User
            {
                Username = username,
                Email = email ?? $"{username}@example.com",
                PasswordHash = passwordHash
            };
            
            await _repository.CreateUserAsync(newUser);
            CurrentUser = newUser;
            StatusText.Text = $"✅ Новый пользователь {username} создан!";
            await Task.Delay(300);
            Close(newUser);
        }
    }
}