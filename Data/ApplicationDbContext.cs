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

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Favourite> Favorites { get; set; }
        public DbSet<MovieInfo> MovieInfos { get; set; }
        public DbSet<CommentRating> CommentRatings { get; set; }
        public DbSet<DownloadLinks> DownloadLinks { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Actor>().ToTable("Actor").HasMany(a => a.Movies).WithMany(m => m.actors);
            builder.Entity<Movie>().ToTable("Movie").HasMany(m => m.actors).WithMany(a => a.Movies);
            builder.Entity<Favourite>().ToTable("Favourite");
            builder.Entity<MovieInfo>().ToTable("MovieInfo");
            builder.Entity<CommentRating>().ToTable("CommentRating");
            builder.Entity<DownloadLinks>().ToTable("DownloadLinks");
            builder.Entity<UserInfo>().ToTable("UserInfo");
            base.OnModelCreating(builder);
        }
    }
}