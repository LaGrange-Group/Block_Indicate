﻿// <auto-generated />
using System;
using Block_Indicate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Block_Indicate.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Block_Indicate.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AccountActive");

                    b.Property<bool>("AccountTrial");

                    b.Property<string>("BinanceApiKey");

                    b.Property<string>("BinanceApiSecret");

                    b.Property<bool>("CompletedSignUp");

                    b.Property<bool?>("ConnectedBinance");

                    b.Property<bool?>("ConnectedHuobi");

                    b.Property<string>("FirstName");

                    b.Property<string>("HuobiApiKey");

                    b.Property<string>("HuobiApiSecret");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("PaidStartDate");

                    b.Property<string>("PhoneNumber");

                    b.Property<DateTime>("TrialStartDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Block_Indicate.Models.DoubleVolumeBinance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BitcoinVolume")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<double>("PercentageChange");

                    b.Property<DateTime>("RealTime");

                    b.Property<string>("Symbol");

                    b.HasKey("Id");

                    b.ToTable("DoubleVolumeBinance");
                });

            modelBuilder.Entity("Block_Indicate.Models.FourHourDoji", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BitcoinVolume")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("BitcoinVolumeToMatch")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<bool>("Logged");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("PriceToMatch")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<DateTime>("RealTime");

                    b.Property<string>("Symbol");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(28, 18)");

                    b.HasKey("Id");

                    b.ToTable("FourHourDojis");
                });

            modelBuilder.Entity("Block_Indicate.Models.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BitcoinVolumeAF")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("BitcoinVolumeOriginal")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<string>("Exchange");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("MaxPercentDiff24Hr")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<bool?>("NoTrade");

                    b.Property<bool>("Open");

                    b.Property<double>("PercentageChangeAF");

                    b.Property<int>("PrevRowId");

                    b.Property<DateTime>("RealTime");

                    b.Property<bool?>("ResultOfTrade");

                    b.Property<string>("Symbol");

                    b.Property<string>("TimeToResult");

                    b.Property<string>("Type");

                    b.Property<decimal>("VolumeAF")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("Vwap")
                        .HasColumnType("decimal(28, 18)");

                    b.HasKey("Id");

                    b.HasIndex("PrevRowId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("Block_Indicate.Models.Trade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("BuyPrice")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<bool>("Closed");

                    b.Property<decimal>("CurrentPercentageResult")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime?>("EndDate");

                    b.Property<decimal>("EndingBitcoinAmount")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<string>("Exchange");

                    b.Property<decimal>("FinalPercentageResult")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<DateTime>("RealTime");

                    b.Property<decimal>("SellPrice")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<DateTime>("StartDate");

                    b.Property<decimal>("StartingBitcoinAmount")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("StopLoss")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<string>("Symbol");

                    b.Property<decimal>("TakeProfit")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<string>("TimeActive");

                    b.Property<string>("TimeToCompletion");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("Block_Indicate.Models.TradeBot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AllMarkets");

                    b.Property<decimal>("AllocatedBitcoin")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<string>("BaseCurrency");

                    b.Property<int>("CustomerId");

                    b.Property<string>("Exchange");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfActiveTrades");

                    b.Property<int>("NumberOfTrades");

                    b.Property<decimal>("PercentStopLoss")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("PercentTakeProfit")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<bool>("Status");

                    b.Property<string>("Type");

                    b.Property<int>("UniqueSetId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("TradeBots");
                });

            modelBuilder.Entity("Block_Indicate.Models.TradePerformance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DVBAvgTime");

                    b.Property<double>("DVBFail");

                    b.Property<double>("DVBNoTrade");

                    b.Property<double>("DVBNoTradeAvgSettle");

                    b.Property<double>("DVBSuccess");

                    b.Property<string>("FourDBAvgTime");

                    b.Property<double>("FourDBFail");

                    b.Property<double>("FourDBNoTrade");

                    b.Property<double>("FourDBNoTradeAvgSettle");

                    b.Property<double>("FourDBSuccess");

                    b.HasKey("Id");

                    b.ToTable("TradePerformances");
                });

            modelBuilder.Entity("Block_Indicate.Models.TriggeredDojiFourHour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BitcoinVolume")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<bool>("Logged");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<double>("PercentageChange");

                    b.Property<DateTime>("RealTime");

                    b.Property<string>("Symbol");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("Vwap")
                        .HasColumnType("decimal(28, 18)");

                    b.HasKey("Id");

                    b.ToTable("TriggeredDojiFourHours");
                });

            modelBuilder.Entity("Block_Indicate.Models.ValidDoubleVolumeBinance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BitcoinVolume")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<bool>("Logged");

                    b.Property<double>("PercentageChange");

                    b.Property<int>("PrevRowId");

                    b.Property<DateTime>("RealTime");

                    b.Property<string>("Symbol");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(28, 18)");

                    b.Property<decimal>("Vwap")
                        .HasColumnType("decimal(28, 18)");

                    b.HasKey("Id");

                    b.ToTable("ValidDoubleVolumeBinance");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Block_Indicate.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");


                    b.ToTable("ApplicationUser");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Block_Indicate.Models.Customer", b =>
                {
                    b.HasOne("Block_Indicate.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Block_Indicate.Models.Result", b =>
                {
                    b.HasOne("Block_Indicate.Models.DoubleVolumeBinance", "DoubleVolumeBinance")
                        .WithMany()
                        .HasForeignKey("PrevRowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Block_Indicate.Models.Trade", b =>
                {
                    b.HasOne("Block_Indicate.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Block_Indicate.Models.TradeBot", b =>
                {
                    b.HasOne("Block_Indicate.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
