using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UserComment> UsersComments { get; set; }
        public DbSet<UserLike> UsersLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationships between Users and Images
            modelBuilder.Entity<UserComment>()
                .HasKey(uc => new { uc.UserId, uc.ImageId });

            modelBuilder.Entity<UserComment>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UsersComments)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserComment>()
                .HasOne(uc => uc.Image)
                .WithMany(i => i.UsersComments)
                .HasForeignKey(uc => uc.ImageId);

            modelBuilder.Entity<UserLike>()
                .HasKey(ul => new { ul.UserId, ul.ImageId });

            modelBuilder.Entity<UserLike>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.UsersLikes)
                .HasForeignKey(ul => ul.UserId);

            modelBuilder.Entity<UserLike>()
                .HasOne(ul => ul.Image)
                .WithMany(i => i.UsersLikes)
                .HasForeignKey(ul => ul.ImageId);

            modelBuilder
                .Entity<ImageFile>()
                .Property(f => f.FileContent)
                .HasColumnType("varbinary(max)");

            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion<string>();
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}