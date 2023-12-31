﻿/**
 *  Author:           Katona Tamás
 *  E-mail:           katonatomi@msn.com
 *  Course:           CUBIX - C# és .NET fejlesztés alapok, 2023.June - Sept
 *  Project Name:     MINDIG FÉNYES KFT, Company's Working and Issue Management Software
 *  Project Github:   https://github.com/tamaskatona89/AlwaysLightsOnManagement
 *  Project Duration: 2023.08.23....2023.09.06
 */
using System.Xml.Serialization;
using AlwaysLightsOnDataModelsDLL;
using Microsoft.EntityFrameworkCore;

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
        /// Returns a list of ReportedIssues in a specified Town/City/Village by a given ZIPCode - With Status of IsFixed = False, so It's Open
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
                return dbServices.ReportedIssues.Where(ri => ri.ZipCode == paramZIPCode && ri.IsFixed == false).OrderBy(ri => ri.ReportedDateTime).ToList();
            }
        }

        /// <summary>
        /// Returns a list of ReportedIssues in a specified Budapest district, involved all sub-regions - With Status of IsFixed = False, so It's Open
        /// </summary>
        /// <param name="districtNumber">Must be a 3 digit number. Ex.: 106 for VI.district</param>
        /// <returns>List of ReportedIssue objects</returns>
        /// <exception cref="ArgumentException">If districtNumber is not a 3 digit number</exception>
        public List<ReportedIssue>? GetBudapestReportedOPENIssuesByDistrict(int districtNumber)
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
                    resultSubList = dbServices.ReportedIssues.Where(ri => ri.ZipCode == currentDistrictRegion && ri.IsFixed == false).OrderBy(ri => ri.ReportedDateTime).ToList();
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

        /// <summary>
        /// Returns ReportedIssues from all Hungary - With Status of IsFixed = False, so It's Open && Older than CurrentDate-dayCount
        /// </summary>
        /// <param name="dayCount">Days to subtract from CurrentDate</param>
        /// <returns>List of ReportedIssue objects</returns>
        public List<ReportedIssue>? GetReportedIssuesOlderThan(int dayCount)
        {
            using (var dbServices = new DBServices())
            {
                //DateTime beforeWhen = DateTime.Today.AddDays(-Double.Parse(dayCount.ToString()));
                DateTime beforeWhen = DateTime.Today.AddDays(-dayCount);

                //SELECT * FROM dbo.ReportedIssues WHERE Reported_DateTime < '2023.08.26';
                //var toReturn = dbServices.Database.ExecuteSqlRaw("SELECT * FROM dbo.ReportedIssues WHERE Reported_DateTime < '{0}'",beforeWhen);
                //var toReturn = dbServices.ReportedIssues.Where(ri => ri.ReportedDateTime < beforeWhen && ri.IsFixed == false).ToList();
                return dbServices.ReportedIssues.Where(ri => ri.ReportedDateTime.HasValue && ri.ReportedDateTime < beforeWhen && ri.IsFixed == false).OrderBy(ri => ri.ReportedDateTime).ToList();
            }
        }

        /// <summary>
        /// Creates a WorkList entry
        /// </summary>
        /// <param name="issueID">Existing ReportedIssues.Issue_ID</param>
        /// <param name="workTypeID">Existing WorkTypes.WorkType_ID</param>
        /// <param name="workerID">Existing Worker.Worker_ID</param>
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

        /// <summary>
        /// GetReportedIssues Switcher. Calls other GetReportedIssue methods depends on inputNumber digits. 1-2-3-or 4
        /// </summary>
        /// <param name="number">1-2-3-or 4 digit only int number</param>
        /// <returns>Call a GetReportedIssue... method with inputNumber</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<ReportedIssue>? ReportedIssuesQuery_byNumber_Switch(int number)
        {
            //Check number has 1, 2, 3, or 4 accepted Digits
            if (4 < Math.Floor(Math.Log10(number) + 1) || 1 > Math.Floor(Math.Log10(number) + 1))
            {
                throw new ArgumentException("ReportedIssuesQuery_byNumber_Switch(): Argument number must be a 1 or 2 or 3 or 4 digit number!");
            }

            if (4 == Math.Floor(Math.Log10(number) + 1))
            {
                // 4 DIGIT Number
                return GetReportedIssuesByZIPCode(number);
            }
            else if (3 == Math.Floor(Math.Log10(number) + 1))
            {
                // 3 DIGIT Number
                return GetBudapestReportedOPENIssuesByDistrict(number);
            }
            else if (2 == Math.Floor(Math.Log10(number) + 1) || 1 == Math.Floor(Math.Log10(number) + 1))
            {
                // 2 DIGIT Number
                return GetReportedIssuesOlderThan(number);
            }
            return null;

        }

        /// <summary>
        /// Get finished WorkList completed by Workers based on given Year and Month
        /// </summary>
        /// <param name="yearNumber">int Year, ex.: 2023</param>
        /// <param name="monthNumber">int Month, ex.: 8</param>
        /// <returns>List of ExportableWorkList objects</returns>
        public List<ExportableWorkList> GetWorkListByTime(int yearNumber, int monthNumber)
        {
            using (var dbServices = new DBServices())
            {
                var resultList_interface = from wl in dbServices.WorkLists
                                           join ri in dbServices.ReportedIssues on wl.IssueId equals ri.IssueId
                                           join wt in dbServices.WorkTypes on wl.WorkTypeId equals wt.WorkTypeId
                                           join wker in dbServices.Workers on wl.WorkerId equals wker.WorkerId
                                           where wl.FixingDateTime.HasValue && wl.FixingDateTime.Value.Year == yearNumber && wl.FixingDateTime.Value.Month == monthNumber
                                           select new ExportableWorkList(Int32.Parse(wl.WorkListId.ToString()),
                                                                         ri.ZipCode.ToString() + " " + ri.Address.ToString(),
                                                                          wt.WorkTypeDescription.ToString(),
                                                                         wker.FullName.ToString(),
                                                                        wl.FixingDateTime!.Value);

                // COPY resultList_interface --> (ExportableWorkList) resultList what WPF DataGrid can only handle, or put ZERO result message to it.
                List<ExportableWorkList> resultList = new List<ExportableWorkList>();
                if (resultList_interface.Any())
                    foreach (var item in resultList_interface)
                        resultList.Add(item);
                else
                    resultList.Add(new ExportableWorkList(0, "A lekérdezés nem hozott eredményt...", "-", "-", DateTime.Now));

                return resultList;
            }
        }

        /// <summary>
        /// Get finished WorkList completed by Workers based on given Year and Month, Group by WorkType
        /// </summary>
        /// <param name="yearNumber">int Year, ex.: 2023</param>
        /// <param name="monthNumber">int Month, ex.: 8</param>
        /// <returns>List of ExportableWorkList objects</returns>
        public List<ExportableWorkList> GetWorkListByMonth_GroupByWorkTypes(int yearNumber, int monthNumber)
        {
            using (var dbServices = new DBServices())
            {
                List<WorkType> workTypesList = dbServices.WorkTypes.ToList();
                List<ExportableWorkList> resultList = new();
                foreach (var workType in workTypesList)
                {
                    //ADD Current WorkType Header To resultList
                    resultList.Add(new ExportableWorkList(0, "", "▼" + workType.WorkTypeDescription + "▼", "", DateTime.Now));

                    //Select Current WorkType key, and get WorkList related to it.
                    List<ExportableWorkList> currentWorkTypeResultList = GetWorkListByWorkTypeIDAndTime(workType.WorkTypeId, yearNumber, monthNumber);
                    foreach (var currentWorkTypeCurrentWork in currentWorkTypeResultList)
                    {
                        resultList.Add(new ExportableWorkList(currentWorkTypeCurrentWork.WorkListID, currentWorkTypeCurrentWork.ReportedIssue, workType.WorkTypeDescription, currentWorkTypeCurrentWork.Worker, currentWorkTypeCurrentWork.FixingDateTime));
                    }

                    //ADD 2xEmpty Delimiter Entries between Types (if not last WorkType...)
                    if (workType != workTypesList.Last())
                    {
                        resultList.Add(new ExportableWorkList(0, "", "", "", DateTime.Now)); resultList.Add(new ExportableWorkList(0, "", "", "", DateTime.Now));
                    }
                }
                return resultList;
            }
        }

        public List<ExportableWorkList> GetWorkListByWorkerIDAndTime(int workerID, int yearNumber, int monthNumber)
        {
            using (var dbServices = new DBServices())
            {
                var resultList_interface = from wl in dbServices.WorkLists
                                           join ri in dbServices.ReportedIssues on wl.IssueId equals ri.IssueId
                                           join wt in dbServices.WorkTypes on wl.WorkTypeId equals wt.WorkTypeId
                                           join wker in dbServices.Workers on wl.WorkerId equals wker.WorkerId
                                           where wl.FixingDateTime.HasValue && wl.FixingDateTime.Value.Year == yearNumber && wl.FixingDateTime.Value.Month == monthNumber && wker.WorkerId == workerID
                                           select new ExportableWorkList(Int32.Parse(wl.WorkListId.ToString()),
                                                                         ri.ZipCode.ToString() + " " + ri.Address.ToString(),
                                                                          wt.WorkTypeDescription.ToString(),
                                                                         wker.FullName.ToString(),
                                                                        wl.FixingDateTime!.Value);

                // COPY resultList_interface --> (ExportableWorkList) resultList what WPF DataGrid can only handle, or put ZERO result message to it.
                List<ExportableWorkList> resultList = new List<ExportableWorkList>();
                if (resultList_interface.Any())
                    foreach (var item in resultList_interface)
                        resultList.Add(item);
                else
                    resultList.Add(new ExportableWorkList(0, "A lekérdezés nem hozott eredményt...", "-", "-", DateTime.Now));

                return resultList;
            }
        }

        /// <summary>
        /// Get finished WorkList completed by Workers based on given existing WorkTypeID, Year, and Month
        /// </summary>
        /// <param name="workTypeID">int existing WorkTypeID, ex.: 1</param>
        /// <param name="yearNumber">int Year, ex.: 2023</param>
        /// <param name="monthNumber">int Month, ex.: 8</param>
        /// <returns>List of ExportableWorkList objects</returns>
        public List<ExportableWorkList> GetWorkListByWorkTypeIDAndTime(int workTypeID, int yearNumber, int monthNumber)
        {
            using (var dbServices = new DBServices())
            {
                var resultList_interface = from wl in dbServices.WorkLists
                                           join ri in dbServices.ReportedIssues on wl.IssueId equals ri.IssueId
                                           join wt in dbServices.WorkTypes on wl.WorkTypeId equals wt.WorkTypeId
                                           join wker in dbServices.Workers on wl.WorkerId equals wker.WorkerId
                                           where wl.FixingDateTime.HasValue && wl.FixingDateTime.Value.Year == yearNumber && wl.FixingDateTime.Value.Month == monthNumber && wl.WorkTypeId == workTypeID
                                           select new ExportableWorkList(wl.WorkListId,
                                                                         ri.ZipCode.ToString() + " " + ri.Address,
                                                                          wt.WorkTypeDescription,
                                                                         wker.FullName,
                                                                        wl.FixingDateTime!.Value);

                // COPY resultList_interface --> (ExportableWorkList) resultList what WPF DataGrid can only handle, or put ZERO result message to it.
                List<ExportableWorkList> resultList = new List<ExportableWorkList>();
                if (resultList_interface.Any())
                    foreach (var item in resultList_interface)
                        resultList.Add(item);
                else
                    resultList.Add(new ExportableWorkList(0, "A lekérdezés nem hozott eredményt...", "-", "-", DateTime.Now));

                return resultList;
            }
        }

        /// <summary>
        /// Get Workers from Workers DB-Table 
        /// </summary>
        /// <returns>List of Worker object</returns>
        public List<Worker>? GetWorkers()
        {
            using (var dbServices = new DBServices())
            {
                return dbServices.Workers.ToList();
            }
        }

        /// <summary>
        /// Create XML file from List<ExportableWorkList>
        /// </summary>
        /// <param name="xmlFileName">Filename to serialize into</param>
        /// <param name="listToSerialize">List<ExportableWorkList> to Serialize</param>
        public void CreateXML(string xmlFileName, List<ExportableWorkList> listToSerialize)
        {

            // Create an instance of XmlSerializer class
            XmlSerializer serializer = new XmlSerializer(typeof(List<ExportableWorkList>));

            // Create a FileStream to write the serialized XML to a file
            using (FileStream stream = new FileStream(xmlFileName, FileMode.Create))
            {
                // Serialize WorkList to the XML file
                serializer.Serialize(stream, listToSerialize);
                stream.Close();
            }
        }


    }
}
