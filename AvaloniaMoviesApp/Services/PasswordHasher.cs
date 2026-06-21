namespace AvaloniaMoviesApp.Services;

public static class PasswordHasher
{
    // Хешируем пароль
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Проверяем пароль
    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}