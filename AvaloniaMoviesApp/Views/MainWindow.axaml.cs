using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaMoviesApp.Models;

namespace AvaloniaMoviesApp.Views;

public partial class MainWindow : Window
{
    private readonly MovieRepository _repository;
    private ObservableCollection<Movie> _movies;
    
    private readonly User _currentUser;

    public MainWindow(User currentUser)
    {
        InitializeComponent();
        _currentUser = currentUser;
    
        _repository = new MovieRepository();
        _movies = new ObservableCollection<Movie>();
    
        MoviesListBox.ItemsSource = _movies;
    
        _ = LoadMoviesAsync();
    
        AddButton.Click += OnAddButtonClick;
        EditButton.Click += OnEditButtonClick;
        DeleteButton.Click += OnDeleteButtonClick;
        RefreshButton.Click += OnRefreshButtonClick;
        MoviesListBox.DoubleTapped += OnMovieDoubleClick;
    }
    
    private async Task LoadMoviesAsync()
    {
        var movies = await _repository.GetAllAsync();
        _movies.Clear();
        foreach (var movie in movies)
        {
            _movies.Add(movie);
        }
    }
    
    private async void OnAddButtonClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new MovieDialogWindow();
        dialog.Title = "Добавление фильма";
        var result = await dialog.ShowDialog<Movie?>(this);
        
        if (result != null)
        {
            await _repository.AddAsync(result);
            await LoadMoviesAsync();
        }
    }
    
    private async void OnEditButtonClick(object? sender, RoutedEventArgs e)
    {
        var selectedMovie = MoviesListBox.SelectedItem as Movie;
        if (selectedMovie == null)
        {
            return;
        }
        
        var dialog = new MovieDialogWindow(selectedMovie);
        dialog.Title = "Редактирование фильма";
        var result = await dialog.ShowDialog<Movie?>(this);
        
        if (result != null)
        {
            await _repository.UpdateAsync(result);
            await LoadMoviesAsync();
        }
    }
    
    private async void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
    {
        var selectedMovie = MoviesListBox.SelectedItem as Movie;
        if (selectedMovie == null)
        {
            return;
        }
        
        await _repository.DeleteAsync(selectedMovie.Id);
        await LoadMoviesAsync();
    }
    
    private async void OnRefreshButtonClick(object? sender, RoutedEventArgs e)
    {
        await LoadMoviesAsync();
    }
    
    
    private async void OnMovieDoubleClick(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        var selectedMovie = MoviesListBox.SelectedItem as Movie;
        if (selectedMovie == null) return;
    
        var reviewsWindow = new ReviewsWindow(selectedMovie, _currentUser);
        await reviewsWindow.ShowDialog(this);
    }
    
}