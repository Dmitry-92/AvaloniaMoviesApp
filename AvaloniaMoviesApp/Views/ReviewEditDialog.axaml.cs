using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaMoviesApp.Models;

namespace AvaloniaMoviesApp.Views;

public partial class ReviewEditDialog : Window
{
    private Review _review;

    public ReviewEditDialog(Review review)
    {
        InitializeComponent();
        
        _review = review;
        
        RatingBox.Value = _review.Rating;
        CommentBox.Text = _review.Comment;

        SaveButton.Click += OnSaveClick;
        CancelButton.Click += (s, e) => Close(null);
    }

    private void OnSaveClick(object sender, RoutedEventArgs e)
    {
        var rating = (int)(RatingBox.Value ?? 5);
        var comment = CommentBox.Text?.Trim();

        if (string.IsNullOrWhiteSpace(comment))
        {
            return;
        }
        
        _review.Rating = rating;
        _review.Comment = comment;
        
        Close(_review);
    }
}