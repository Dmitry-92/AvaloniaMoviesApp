using System.Collections.Generic;
using System.Linq;

namespace AvaloniaMoviesApp.Models;

public class MovieRepository
{
    private readonly AppDbContext _context;

    public MovieRepository()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();
    }
    
    // Получить все фильмы

    public List<Movie> GetAll()
    {
        return _context.Movies.ToList();
    }
    
    // Добавить фильм
    public void Add(Movie movie)
    {
        _context.Movies.Add(movie);
        _context.SaveChanges();
    }
    
    // Обновить фильм
    public void Update(Movie movie)
    {
        _context.Movies.Update(movie);
        _context.SaveChanges();
    }
    
    // Удалить фильм
    public void Delete(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }
    }
    // Получить фильм по ID
    public Movie? GetById(int id)
    {
        return _context.Movies.Find(id);
    }
}

