using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }


        public FootballBettingContext(DbContextOptions options) : base(options)
        {
        }

        public FootballBettingContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string Config = @"Server=DESKTOP-VNP1D7N\SQLEXPRESS;Database=Betting;Integrated Security = true;";
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBetEntity(modelBuilder);

            ConfigureColorEntity(modelBuilder);

            ConfigureCountryEntity(modelBuilder);

            ConfigureGameEntity(modelBuilder);

            ConfigurePlayerEntity(modelBuilder);

            ConfigurePlayerStatisticEntity(modelBuilder);

            ConfigurePositionEntity(modelBuilder);

            ConfigureTeamEntity(modelBuilder);

            ConfigureTownEntity(modelBuilder);

            ConfigureUserEntity(modelBuilder);
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasKey(u => u.UserId);
        }

        private void ConfigureTownEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Town>()
                .HasKey(t => t.TownId);

            modelBuilder
                .Entity<Town>()
                .HasOne(t => t.Country)
                .WithMany(t => t.Towns)
                .HasForeignKey(t => t.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        private void ConfigureTeamEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Team>()
                .HasKey(t => t.TeamId);

            modelBuilder
                .Entity<Team>()
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(t => t.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);
                

            modelBuilder
                .Entity<Team>()
                .HasOne(t => t.SecondaryKitColor)
                .WithMany(t => t.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<Team>()
                .HasOne(t => t.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.TownId);

        }

        private void ConfigurePositionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Position>()
                .HasKey(p => p.PositionId);
        }

        private void ConfigurePlayerStatisticEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.PlayerId, ps.GameId });
        }

        private void ConfigurePlayerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
             .Entity<Player>()
             .HasKey(p => p.PlayerId);

            modelBuilder
                .Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(p => p.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<Player>()
                .HasOne(p => p.Position)
                .WithMany(p => p.Players)
                .HasForeignKey(p => p.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        private void ConfigureGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Game>()
                .HasKey(g => g.GameId);

            modelBuilder
                .Entity<Game>()
                .HasOne(g => g.HomeTeam)
                .WithMany(g => g.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<Game>()
                .HasOne(g => g.AwayTeam)
                .WithMany(g => g.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        private void ConfigureCountryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Country>()
                .HasKey(c => c.CountryId);
        }

        private void ConfigureColorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Color>()
                .HasKey(c => c.ColorId);
        }

        private void ConfigureBetEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Bet>()
                .HasKey(b => b.BetId);

            modelBuilder
                .Entity<Bet>()
                .Property(b => b.Prediction)
                .IsRequired();

            modelBuilder
                .Entity<Bet>()
                .HasOne(b => b.Game)
                .WithMany(b => b.Bets)
                .HasForeignKey(b => b.GameId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany(b => b.Bets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
