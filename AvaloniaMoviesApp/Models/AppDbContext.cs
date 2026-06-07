using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaMoviesApp.Models;

public class AppDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    
    private readonly string _databasePath;

    public AppDbContext()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _databasePath = Path.Combine(folder, "AvaloniaMoviesApp", "movies.db");
        
        var directory = Path.GetDirectoryName(_databasePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_databasePath}");
    }
}