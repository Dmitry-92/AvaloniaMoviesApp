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
    
    public MainWindow()
    {
        InitializeComponent();
        
        _repository = new MovieRepository();
        _movies = new ObservableCollection<Movie>();
        
        MoviesListBox.ItemsSource = _movies;
        
        LoadMovies();
        
        AddButton.Click += OnAddButtonClick;
        EditButton.Click += OnEditButtonClick;
        DeleteButton.Click += OnDeleteButtonClick;
        RefreshButton.Click += OnRefreshButtonClick;
    }
    
    private void LoadMovies()
    {
        var movies = _repository.GetAll();
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
            _repository.Add(result);
            LoadMovies();
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
            _repository.Update(result);
            LoadMovies();
        }
    }
    
    private async void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
    {
        var selectedMovie = MoviesListBox.SelectedItem as Movie;
        if (selectedMovie == null)
        {
            return;
        }
        
        _repository.Delete(selectedMovie.Id);
        LoadMovies();
    }
    
    private void OnRefreshButtonClick(object? sender, RoutedEventArgs e)
    {
        LoadMovies();
    }
}