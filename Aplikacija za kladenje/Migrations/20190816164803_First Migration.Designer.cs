﻿// <auto-generated />
using System;
using Aplikacija_za_kladenje.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aplikacija_za_kladenje.Migrations
{
    [DbContext(typeof(Aplikacija_za_kladenjeContext))]
    [Migration("20190816164803_First Migration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.BetSlip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AwayTeam");

                    b.Property<string>("HomeTeam");

                    b.Property<string>("MatchId");

                    b.Property<decimal>("Odd");

                    b.Property<bool>("TopMatch");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("BetSlip");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Leagues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("SportId");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Matches", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AwayTeamId");

                    b.Property<int?>("HomeTeamId");

                    b.Property<string>("Result");

                    b.Property<int?>("SportId");

                    b.Property<bool>("TopMatch");

                    b.Property<int?>("TypesId");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("SportId");

                    b.HasIndex("TypesId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("SportId");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Sports", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Teams", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LeagueId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.TwoPlayersMatches", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("FirstId");

                    b.Property<int?>("SecondId");

                    b.Property<int?>("SportId");

                    b.Property<bool>("TopMatch");

                    b.Property<decimal>("_1");

                    b.Property<decimal>("_2");

                    b.HasKey("Id");

                    b.HasIndex("FirstId");

                    b.HasIndex("SecondId");

                    b.HasIndex("SportId");

                    b.ToTable("TwoPlayersMatches");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Types", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("_1");

                    b.Property<decimal>("_12");

                    b.Property<decimal>("_1X");

                    b.Property<decimal>("_2");

                    b.Property<decimal>("_X");

                    b.Property<decimal>("_X2");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.UserBetMatches", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AwayTeam");

                    b.Property<string>("HomeTeam");

                    b.Property<string>("MatchId");

                    b.Property<decimal>("Odd");

                    b.Property<bool>("TopMatch");

                    b.Property<string>("Type");

                    b.Property<int?>("UserBetsId");

                    b.HasKey("Id");

                    b.HasIndex("UserBetsId");

                    b.ToTable("UserBetMatches");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.UserBets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BetAmount");

                    b.Property<decimal>("CashOut");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<decimal>("TotalOdd");

                    b.HasKey("Id");

                    b.ToTable("UserBets");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.UserTransactions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Payment");

                    b.Property<string>("Transactions");

                    b.Property<string>("UserID");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("UserTransactions");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Wallet", b =>
                {
                    b.Property<string>("Userid")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Saldo");

                    b.HasKey("Userid");

                    b.ToTable("Wallet");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Leagues", b =>
                {
                    b.HasOne("Aplikacija_za_kladenje.Models.Sports", "Sport")
                        .WithMany("Leagues")
                        .HasForeignKey("SportId");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Matches", b =>
                {
                    b.HasOne("Aplikacija_za_kladenje.Models.Teams", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("Aplikacija_za_kladenje.Models.Teams", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");

                    b.HasOne("Aplikacija_za_kladenje.Models.Sports", "Sport")
                        .WithMany()
                        .HasForeignKey("SportId");

                    b.HasOne("Aplikacija_za_kladenje.Models.Types", "Types")
                        .WithMany()
                        .HasForeignKey("TypesId");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Player", b =>
                {
                    b.HasOne("Aplikacija_za_kladenje.Models.Sports", "Sport")
                        .WithMany()
                        .HasForeignKey("SportId");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.Teams", b =>
                {
                    b.HasOne("Aplikacija_za_kladenje.Models.Leagues", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.TwoPlayersMatches", b =>
                {
                    b.HasOne("Aplikacija_za_kladenje.Models.Player", "First")
                        .WithMany()
                        .HasForeignKey("FirstId");

                    b.HasOne("Aplikacija_za_kladenje.Models.Player", "Second")
                        .WithMany()
                        .HasForeignKey("SecondId");

                    b.HasOne("Aplikacija_za_kladenje.Models.Sports", "Sport")
                        .WithMany()
                        .HasForeignKey("SportId");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.UserBetMatches", b =>
                {
                    b.HasOne("Aplikacija_za_kladenje.Models.UserBets", "UserBets")
                        .WithMany("Matches")
                        .HasForeignKey("UserBetsId");
                });

            modelBuilder.Entity("Aplikacija_za_kladenje.Models.UserTransactions", b =>
                {
                    b.HasOne("Aplikacija_za_kladenje.Models.Wallet", "Wallet")
                        .WithMany("Transactions")
                        .HasForeignKey("UserID");
                });
#pragma warning restore 612, 618
        }
    }
}
