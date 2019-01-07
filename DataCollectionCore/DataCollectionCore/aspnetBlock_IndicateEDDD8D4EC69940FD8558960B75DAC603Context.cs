using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataCollectionCore
{
    public partial class aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context : DbContext
    {
        public aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context()
        {
        }

        public aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context(DbContextOptions<aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context> options)
            : base(options)
        {
        }

        public virtual DbSet<DoubleVolumeBinance> DoubleVolumeBinance { get; set; }
        public virtual DbSet<FourHourDojis> FourHourDojis { get; set; }
        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<TradePerformances> TradePerformances { get; set; }
        public virtual DbSet<TriggeredDojiFourHours> TriggeredDojiFourHours { get; set; }
        public virtual DbSet<ValidDoubleVolumeBinance> ValidDoubleVolumeBinance { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Block_Indicate-EDDD8D4E-C699-40FD-8558-960B75DAC603;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<DoubleVolumeBinance>(entity =>
            {
                entity.Property(e => e.BitcoinVolume).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.LastPrice).HasColumnType("decimal(28, 18)");
            });

            modelBuilder.Entity<FourHourDojis>(entity =>
            {
                entity.Property(e => e.BitcoinVolume).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.BitcoinVolumeToMatch).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Close).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.High).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Low).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Open).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.PriceToMatch).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Volume).HasColumnType("decimal(28, 18)");
            });

            modelBuilder.Entity<Results>(entity =>
            {
                entity.Property(e => e.BitcoinVolumeAf)
                    .HasColumnName("BitcoinVolumeAF")
                    .HasColumnType("decimal(28, 18)");

                entity.Property(e => e.BitcoinVolumeOriginal).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.LastPrice).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.MaxPercentDiff24Hr).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.PercentageChangeAf).HasColumnName("PercentageChangeAF");

                entity.Property(e => e.VolumeAf)
                    .HasColumnName("VolumeAF")
                    .HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Vwap).HasColumnType("decimal(28, 18)");
            });

            modelBuilder.Entity<TradePerformances>(entity =>
            {
                entity.Property(e => e.DvbavgTime).HasColumnName("DVBAvgTime");

                entity.Property(e => e.Dvbfail).HasColumnName("DVBFail");

                entity.Property(e => e.DvbnoTrade).HasColumnName("DVBNoTrade");

                entity.Property(e => e.DvbnoTradeAvgSettle).HasColumnName("DVBNoTradeAvgSettle");

                entity.Property(e => e.Dvbsuccess).HasColumnName("DVBSuccess");

                entity.Property(e => e.FourDbavgTime).HasColumnName("FourDBAvgTime");

                entity.Property(e => e.FourDbfail).HasColumnName("FourDBFail");

                entity.Property(e => e.FourDbnoTrade).HasColumnName("FourDBNoTrade");

                entity.Property(e => e.FourDbnoTradeAvgSettle).HasColumnName("FourDBNoTradeAvgSettle");

                entity.Property(e => e.FourDbsuccess).HasColumnName("FourDBSuccess");
            });

            modelBuilder.Entity<TriggeredDojiFourHours>(entity =>
            {
                entity.Property(e => e.BitcoinVolume).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Close).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.High).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.LastPrice).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Low).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Open).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Volume).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Vwap).HasColumnType("decimal(28, 18)");
            });

            modelBuilder.Entity<ValidDoubleVolumeBinance>(entity =>
            {
                entity.Property(e => e.BitcoinVolume).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.LastPrice).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Volume).HasColumnType("decimal(28, 18)");

                entity.Property(e => e.Vwap).HasColumnType("decimal(28, 18)");
            });
        }
    }
}
