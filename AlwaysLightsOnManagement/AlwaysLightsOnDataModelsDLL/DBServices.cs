﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
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

        /// <summary>
        /// Returns a list of ReportedIssues in a specified Town/City/Village by a given ZIPCode
        /// </summary>
        /// <param name="paramZIPCode">Must be a 4 digit number. Ex.: 3333 for Terpes or 4400 for Nyíregyháza</param>
        /// <returns>List of ReportedIssue objects</returns>
        /// <exception cref="ArgumentException">If paramZIPCode is not a 4 digit number</exception>
        public List<ReportedIssue>? GetReportedIssuesByZIPCode(int paramZIPCode)
        {
            //Check paramZIPCode has 4 Digits
            if (4 != Math.Floor(Math.Log10(paramZIPCode) + 1))
            {
                throw new ArgumentException("GetReportedIssuesByZIPCode(): Argument paramZIPCode must be a 4 digit number!");
            }
            using (var dbServices = new DBServices())
            {
                //FETCH Matched ReportedIssues Data
                return dbServices.ReportedIssues.Where(ri => ri.ZipCode == paramZIPCode && ri.IsFixed == false).ToList();
            }
        }

        /// <summary>
        /// Returns a list of ReportedIssues in a specified Budapest district, involved all sub-regions.
        /// </summary>
        /// <param name="districtNumber">Must be a 3 digit number. Ex.: 106 for VI.district</param>
        /// <returns>List of ReportedIssue objects</returns>
        /// <exception cref="ArgumentException">If districtNumber is not a 3 digit number</exception>
        public List<ReportedIssue>? GetBudapestReportedIssuesByDistrict(int districtNumber)
        {
            //Check paramZIPCode has 3 Digits
            if (3 != Math.Floor(Math.Log10(districtNumber) + 1))
            {
                throw new ArgumentException("GetBudapestReportedIssuesByDistrict(): Argument districtNumber must be a 3 digit number!");
            }
            using (var dbServices = new DBServices())
            {
                List<ReportedIssue>? resultList = new();
                List<ReportedIssue>? resultSubList = new();

                //FETCH All District Region [1..9] inside Given District ReportedIssues Data
                for (int districtRegionStepper = 1; districtRegionStepper <= 9; districtRegionStepper++)
                {
                    resultSubList.Clear();

                    // 1069 = 106 + [1..9]
                    int currentDistrictRegion = int.Parse(districtNumber.ToString() + districtRegionStepper.ToString());
                    resultSubList = dbServices.ReportedIssues.Where(ri => ri.ZipCode == currentDistrictRegion && ri.IsFixed == false).ToList();
                    if (0 < resultSubList.Count)
                    {
                        foreach (var item in resultSubList)
                        {
                            resultList.Add(item);
                        }
                    }
                }

                return resultList;
            }

        }

        public List<ReportedIssue>? GetReportedIssuesOlderThan(int dayCount)
        {
            using (var dbServices = new DBServices())
            {
                //DateTime beforeWhen = DateTime.Today.AddDays(-Double.Parse(dayCount.ToString()));
                DateTime beforeWhen = DateTime.Today.AddDays(-dayCount);

                //SELECT * FROM dbo.ReportedIssues WHERE Reported_DateTime < '2023.08.26';
                //var toReturn = dbServices.Database.ExecuteSqlRaw("SELECT * FROM dbo.ReportedIssues WHERE Reported_DateTime < '{0}'",beforeWhen);
                //var toReturn = dbServices.ReportedIssues.Where(ri => ri.ReportedDateTime < beforeWhen && ri.IsFixed == false).ToList();
                return dbServices.ReportedIssues.Where(ri => ri.ReportedDateTime.HasValue && ri.ReportedDateTime < beforeWhen && ri.IsFixed == false).ToList();
            }
        }

        public void AddFinishedWorkToWorkList(int issueID, int workTypeID, int workerID)
        {
            // INSERT INTO WorkList(Issue_ID,WorkType_ID,Worker_ID) VALUES(1,2,2);
            using (var dbServices = new DBServices())
            {
                //dbServices.Database.ExecuteSqlRaw("INSERT INTO WorkList(Issue_ID,WorkType_ID,Worker_ID) VALUES(@issueID,@workTypeID,@workerID)",
                //    new SqlParameter("Issue_ID", @issueID),
                //    new SqlParameter("WorkType_ID", @workTypeID),
                //    new SqlParameter("Worker_ID", @workerID)
                //);

                dbServices.Database.ExecuteSqlRaw("INSERT INTO WorkList(Issue_ID,WorkType_ID,Worker_ID) Values({0},{1},{2})", issueID, workTypeID, workerID);


            }
        }

      
    }
}
