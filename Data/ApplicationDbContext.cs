using DMovies.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMovies.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Favourite> Favorite { get; set; }
        public DbSet<MovieInfo> MoviesInfo { get; set; }
        public DbSet<CommentRating> CommentRating { get; set; }
        public DbSet<DownloadLinks> DownloadLinks { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Actor>().ToTable("Actor");
            builder.Entity<Movie>().ToTable("Movie");
            builder.Entity<Favourite>().ToTable("Favourite");
            builder.Entity<MovieInfo>().ToTable("MovieInfo");
            builder.Entity<CommentRating>().ToTable("CommentRating");
            builder.Entity<DownloadLinks>().ToTable("DownloadLinks");
            builder.Entity<UserInfo>().ToTable("UserInfo");
            base.OnModelCreating(builder);
        }
    }
}
