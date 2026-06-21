using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaMoviesApp.Models;

public class MovieRepository
{
    private readonly AppDbContext _context;

    public MovieRepository()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();
    }

    // Получить все фильмы (асинхронно)
    public async Task<List<Movie>> GetAllAsync()
    {
        return await _context.Movies.ToListAsync();
    }

    // Добавить фильм (асинхронно)
    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    // Обновить фильм (асинхронно)
    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }

    // Удалить фильм (асинхронно)
    public async Task DeleteAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }

    // Получить фильм по ID (асинхронно)
    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies.FindAsync(id);
    }
    
    // ========== РАБОТА С ПОЛЬЗОВАТЕЛЯМИ ==========
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    
    public async Task<User> CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    // ========== РАБОТА С ОТЗЫВАМИ ==========

    public async Task<List<Review>> GetReviewsForMovieAsync(int movieId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .AsQueryable()
            .Where(r => r.MovieId == movieId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
    
    public async Task AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateReviewAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int reviewId)
    {
        var review = await _context.Reviews.FindAsync(reviewId);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Review?> GetReviewByIdAsync(int reviewId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == reviewId);
    }
}