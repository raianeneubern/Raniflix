using Microsoft.EntityFrameworkCore;
using Raniflix.Models;

namespace Raniflix.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {     
    } 
    
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Configuração do Muitos para Muitos do MovieGenre
        // Definir a chave primária composta
        builder.Entity<MovieGenre>().HasKey(
            mg => new { mg.MovieId, mg.GenreId }
        );
        // Definir a chave estrangeira do Movie
        builder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.Genres)
            .HasForeignKey(mg => mg.MovieId);
        // Definir a chave estrangeira do Genre
        builder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(mg => mg.GenreId);
        #endregion
    }

}
