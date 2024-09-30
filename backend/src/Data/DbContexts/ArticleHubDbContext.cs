using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DbContexts
{
    public class ArticleHubDbContext : DbContext
    {
        public ArticleHubDbContext(DbContextOptions<ArticleHubDbContext> options)
          : base(options)
        {
        }

        public DbSet<Article> article { get; set; }
        public DbSet<ArticleTag> article_tag { get; set; }
        public DbSet<ArticleComment> article_comment { get; set; }
        public DbSet<ArticleLike> article_like { get; set; }
        public DbSet<Tag> tag { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<UserFollower> user_follower { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(a =>
            {
                a.HasKey(a => a.id);

                a.Property(a => a.slug).IsRequired();
                a.Property(a => a.title).IsRequired();
                a.Property(a => a.body).IsRequired();
                a.Property(a => a.image).IsRequired();

                a.Property(a => a.created).IsRequired();
                a.Property(a => a.updated).IsRequired();
            });

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.id);

                u.HasIndex(u => u.email).IsUnique();
                u.HasIndex(u => u.username).IsUnique();

                u.Property(a => a.first_name).IsRequired();
                u.Property(a => a.last_name).IsRequired();
                u.Property(a => a.email).IsRequired();
                u.Property(a => a.password).IsRequired();
                u.Property(a => a.username).IsRequired();

                u.Property(a => a.created).IsRequired();
                u.Property(a => a.updated).IsRequired();

                u.HasMany<Article>(a => a.user_articles)
                .WithOne(u => u.user)
                .HasForeignKey(s => s.user_id)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Tag>(t =>
            {
                t.HasKey(t => t.id);

                t.Property(a => a.name).IsRequired();
                t.Property(a => a.created).IsRequired();
                t.Property(a => a.updated).IsRequired();
            });

            modelBuilder.Entity<ArticleLike>(a =>
            {
                a.HasKey(af => new {af.user_id, af.article_id});

                a.Property(a => a.created).IsRequired();
                a.Property(a => a.updated).IsRequired();

                a.HasOne(ua => ua.article)
                .WithMany(u => u.article_likes)
                .HasForeignKey(ua => ua.article_id)
                .OnDelete(DeleteBehavior.Cascade);

                a.HasOne(u => u.user)
                .WithMany(u => u.user_likes)
                .HasForeignKey(u => u.user_id)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ArticleTag>(a =>
            {
                a.HasKey(at => new { at.article_id, at.tag_id });

                a.Property(a => a.created).IsRequired();
                a.Property(a => a.updated).IsRequired();

                a.HasOne(ua => ua.article)
                .WithMany(u => u.article_tags)
                .HasForeignKey(ua => ua.article_id)
                .OnDelete(DeleteBehavior.Cascade);

                a.HasOne(u => u.tag)
                .WithMany(u => u.articles)
                .HasForeignKey(u => u.tag_id)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ArticleComment>(c =>
            {
                c.HasKey(c => c.id);

                c.Property(c => c.body).IsRequired();

                c.Property(c => c.created).IsRequired();
                c.Property(c => c.updated).IsRequired();

                c.HasOne(ua => ua.article)
               .WithMany(u => u.article_comments)
               .HasForeignKey(ua => ua.article_id)
               .OnDelete(DeleteBehavior.Cascade);

                c.HasOne(u => u.user)
                .WithMany(u => u.user_comments)
                .HasForeignKey(u => u.user_id)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserFollower>(u =>
            {
                u.HasKey(u => new { u.User_follower_id, u.user_followeing_id });

                u.HasOne(ua => ua.follower)
                .WithMany(u => u.user_followings)
                .HasForeignKey(ua => ua.User_follower_id)
                .OnDelete(DeleteBehavior.NoAction);

                u.HasOne(u => u.followeing)
                .WithMany(u => u.user_followers)
                .HasForeignKey(u => u.user_followeing_id)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}