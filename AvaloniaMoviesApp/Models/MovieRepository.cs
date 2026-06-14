using System.Collections.Generic;
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
}