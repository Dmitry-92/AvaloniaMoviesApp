using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaMoviesApp.Models;

namespace AvaloniaMoviesApp.Views;

public partial class ReviewsWindow : Window
{
    private readonly MovieRepository _repository;
    private readonly Movie _movie;
    private readonly User _currentUser;
    private ObservableCollection<Review> _reviews;
    private Review? _selectedReview;
    
    public ReviewsWindow(Movie movie, User currentUser)
    {
        InitializeComponent();
        
        _repository = new MovieRepository();
        _movie = movie;
        _currentUser = currentUser;
        _reviews = new ObservableCollection<Review>();
        
        ReviewsListBox.ItemsSource = _reviews;
        ReviewsListBox.SelectionChanged += OnReviewSelectionChanged;
        
        // Заполняем информацию о фильме
        MovieTitleText.Text = _movie.Title;
        MovieYearText.Text = _movie.Year.ToString();
        MovieDirectorText.Text = _movie.Director;
        MovieGenreText.Text = _movie.Genre;
        
        AddReviewButton.Click += OnAddReviewClick;
        EditReviewButton.Click += OnEditReviewClick;
        DeleteReviewButton.Click += OnDeleteReviewClick;
        
        // Загружаем отзывы
        _ = LoadReviewsAsync();
    }
    
    private async Task LoadReviewsAsync()
    {
        var reviews = await _repository.GetReviewsForMovieAsync(_movie.Id);
        _reviews.Clear();
        foreach (var review in reviews)
        {
            _reviews.Add(review);
        }
    }
    
    private void OnReviewSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedReview = ReviewsListBox.SelectedItem as Review;
        
        // Показываем кнопки редактирования/удаления только если выбран свой отзыв
        if (_selectedReview != null && _selectedReview.UserId == _currentUser.Id)
        {
            EditReviewButton.IsVisible = true;
            DeleteReviewButton.IsVisible = true;
        }
        else
        {
            EditReviewButton.IsVisible = false;
            DeleteReviewButton.IsVisible = false;
        }
    }
    
    private async void OnAddReviewClick(object? sender, RoutedEventArgs e)
    {
        var rating = (int)(RatingBox.Value ?? 5);
        var comment = CommentBox.Text?.Trim();
        
        if (string.IsNullOrWhiteSpace(comment))
        {
            CommentBox.PlaceholderText = "⚠️ Введите текст комментария";
            return;
        }
        
        var review = new Review
        {
            MovieId = _movie.Id,
            UserId = _currentUser.Id,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now
        };
        
        await _repository.AddReviewAsync(review);
        CommentBox.Text = "";
        RatingBox.Value = 5;
        await LoadReviewsAsync();
    }
    
    private async void OnEditReviewClick(object? sender, RoutedEventArgs e)
    {
        //if (_selectedReview == null) return;
        
        //var dialog = new ReviewEditDialog(_selectedReview);
        //var result = await dialog.ShowDialog<Review?>(this);
        
        //if (result != null)
        //{
          //  await _repository.UpdateReviewAsync(result);
           // await LoadReviewsAsync();
        //}
    }
    
    private async void OnDeleteReviewClick(object? sender, RoutedEventArgs e)
    {
        if (_selectedReview == null) return;
        
        await _repository.DeleteReviewAsync(_selectedReview.Id);
        await LoadReviewsAsync();
        EditReviewButton.IsVisible = false;
        DeleteReviewButton.IsVisible = false;
    }
}