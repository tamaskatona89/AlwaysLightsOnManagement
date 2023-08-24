using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AlwaysLightsOnManagement
{
    public partial class DBServices : DbContext
    {
        public DBServices()
        {
        }

        public DBServices(DbContextOptions<DBServices> options)
            : base(options)
        {
        }

        public virtual DbSet<ReportedIssue> ReportedIssues { get; set; } = null!;
        public virtual DbSet<WorkList> WorkLists { get; set; } = null!;
        public virtual DbSet<WorkType> WorkTypes { get; set; } = null!;
        public virtual DbSet<Worker> Workers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\katon\\source\\repos\\CUBIX\\CS_dotNET_DEV_BASICS\\11th_Week\\AlwaysLightsOnManagement\\DB\\database.mdf;Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportedIssue>(entity =>
            {
                entity.HasKey(e => e.IssueId)
                    .HasName("PK__tmp_ms_x__B29F2BD8DE597CE9");

                entity.Property(e => e.IssueId).HasColumnName("Issue_ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.IsFixed)
                    .HasColumnName("Is_Fixed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReportedDateTime)
                    .HasColumnName("Reported_DateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ZipCode).HasColumnName("ZIP_CODE");
            });

            modelBuilder.Entity<WorkList>(entity =>
            {
                entity.ToTable("WorkList");

                entity.Property(e => e.WorkListId).HasColumnName("WorkList_ID");

                entity.Property(e => e.FixingDateTime)
                    .HasColumnName("Fixing_DateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IssueId).HasColumnName("Issue_ID");

                entity.Property(e => e.WorkTypeId).HasColumnName("WorkType_ID");

                entity.Property(e => e.WorkerId).HasColumnName("Worker_ID");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.WorkLists)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkList_ToReportedIssues");

                entity.HasOne(d => d.WorkType)
                    .WithMany(p => p.WorkLists)
                    .HasForeignKey(d => d.WorkTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkList_ToWorkTypes");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.WorkLists)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkList_ToWorker");
            });

            modelBuilder.Entity<WorkType>(entity =>
            {
                entity.Property(e => e.WorkTypeId).HasColumnName("WorkType_ID");

                entity.Property(e => e.WorkTypeDescription)
                    .HasMaxLength(100)
                    .HasColumnName("WorkType_Description");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.ToTable("Worker");

                entity.Property(e => e.WorkerId).HasColumnName("Worker_ID");

                entity.Property(e => e.FullName).HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
