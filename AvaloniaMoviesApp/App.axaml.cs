using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaMoviesApp.Views;
using AvaloniaMoviesApp.Models;

namespace AvaloniaMoviesApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Сначала показываем окно входа
            var loginWindow = new LoginWindow();
            
            // Подписываемся на событие закрытия окна
            loginWindow.Closed += (s, e) =>
            {
                if (loginWindow.CurrentUser != null)
                {
                    desktop.MainWindow = new MainWindow(loginWindow.CurrentUser);
                    desktop.MainWindow.Show();
                }
                else
                {
                    desktop.Shutdown();
                }
            };
            
            loginWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }
}