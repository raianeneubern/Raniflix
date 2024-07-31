using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Raniflix.Models;

namespace Raniflix.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {     
    } 
    
    public DbSet<AppUser> AppUsers { get; set; }
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
    
        #region Popular os dados de Usuário
        // Perfil - IdentityRole
        List<IdentityRole> roles = new()
        {
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Moderador",
                NormalizedName = "MODERADOR"
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Usuário",
                NormalizedName = "USUÁRIO"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);

        // Conta de Usuário - IdentityUser
        List<IdentityUser> users = new()
        {
            new IdentityUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "admin@galloflix.com",
                NormalizedEmail = "ADMIN@GALLOFLIX.COM",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                EmailConfirmed = true,
                LockoutEnabled = false
            },
            new IdentityUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
                NormalizedEmail = "USER@GMAIL.COM",
                UserName = "User",
                NormalizedUserName = "USER",
                EmailConfirmed = true,
                LockoutEnabled = true
            }
        };
        foreach (var user in users)
        {
            PasswordHasher<IdentityUser> pass = new();
            pass.HashPassword(user, "@Etec123");
        }
        builder.Entity<IdentityUser>().HasData(users);

        // Dados pessoais do Usuário - AppUser
        List<AppUser> appUsers = new()
        {
            new AppUser()
            {
                AppUserId = users[0].Id,
                Name = "José Antonio Gallo Junior",
                Birthday = DateTime.Parse("05/08/1981"),
                Photo = "/img/users/avatar.png"
            },
            new AppUser()
            {
                AppUserId = users[1].Id,
                Name = "Fulaninho",
                Birthday = DateTime.Parse("25/12/2000")
            }
        };
        builder.Entity<AppUser>().HasData(appUsers);

        // Perfis dos Usuários - IdentityUserRole
        List<IdentityUserRole<string>> userRoles = new()
        {
            new IdentityUserRole<string>()
            {
                UserId = users[0].Id,
                RoleId = roles[0].Id
            },
            new IdentityUserRole<string>()
            {
                UserId = users[0].Id,
                RoleId = roles[1].Id
            },
            new IdentityUserRole<string>()
            {
                UserId = users[0].Id,
                RoleId = roles[2].Id
            },
            new IdentityUserRole<string>()
            {
                UserId = users[1].Id,
                RoleId = roles[2].Id
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        #endregion

    }

}
