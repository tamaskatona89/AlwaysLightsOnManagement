using AlwaysLightsOnDataModelsDLL;
using AlwaysLightsOnManagement;

namespace AlwaysLightsOnMSTest
{
    [TestClass]
    public class DBServicesMSTest
    {
        public DBServices DBServicesInstance { get; set; } = new DBServices();
        public List<Worker>? workerList { get; set; }

        [TestInitialize]
        public void Init()
        {
            workerList = new List<Worker>();
            workerList = DBServicesInstance.Workers.ToList();
        }


        [TestMethod]
        public void Test1_GetWorkersListFromDB()
        {
            Assert.IsNotNull(workerList);

        }

        [TestMethod]
        public void Test2_GetWorkersListFromDB_ExpectedItemCountGreaterThan5()
        {
            Assert.IsTrue(workerList?.Count > 5, "Not greater than 5.");

        }

        [TestMethod]
        public void Test3_AddWorkerToDB_ThenGetWorkerDataBack()
        {
            DBServicesInstance.Workers.Add(new Worker { FullName = "Test Test" });
            DBServicesInstance.SaveChanges();
            Worker lastWorker = new();
            lastWorker = DBServicesInstance.Workers.OrderBy(w => w.WorkerId).Last();
            Assert.IsNotNull(lastWorker);
            Assert.AreEqual("Test Test", lastWorker.FullName);

        }

        [TestMethod]
        public void Test4_ModifyWorkerInDB_ThenGetWorkerDataBack()
        {
            Worker lastWorker = new();
            lastWorker = DBServicesInstance.Workers.OrderBy(w => w.WorkerId).Last();
            Assert.IsNotNull(lastWorker);

            //MODIFY
            lastWorker.FullName = "Test2 Test2";
            DBServicesInstance.SaveChanges();

            //GET DATA BACK
            lastWorker = DBServicesInstance.Workers.OrderBy(w => w.WorkerId).Last();
            Assert.IsNotNull(lastWorker);

            Assert.AreEqual("Test2 Test2", lastWorker.FullName);

        }

        [TestMethod]
        public void Test5_DeleteWorkerFromDB()
        {
            Worker lastWorker = new();
            lastWorker = DBServicesInstance.Workers.OrderBy(w => w.WorkerId).Last();
            Assert.IsNotNull(lastWorker);

            //GET LASTWORKER ID
            int lastWorkerId = lastWorker.WorkerId;

            //DELETE
            DBServicesInstance.Remove(lastWorker);
            DBServicesInstance.SaveChanges();

            //GET LASTWORKER ID AGAIN
            lastWorker = DBServicesInstance.Workers.OrderBy(w => w.WorkerId).Last();
            int lastWorkerId2 = lastWorker.WorkerId;

            //CHECK ID-s BEFORE AND AFTER DELETION
            Assert.AreNotEqual(lastWorkerId, lastWorkerId2);
        }

        [TestMethod]
        public void Test_GetReportedIssuesByZIPCode()
        {
            List<ReportedIssue>? resultList = DBServicesInstance.GetReportedIssuesByZIPCode(4400);
            Assert.IsNotNull(resultList);
            resultList = DBServicesInstance.GetReportedIssuesByZIPCode(3700);
            Assert.IsNotNull(resultList);
        }

        [TestMethod]
        public void Test_GetBudapestReportedOPENIssuesByDistrict()
        {
            List<ReportedIssue>? resultList = DBServicesInstance.GetBudapestReportedOPENIssuesByDistrict(106);
            Assert.IsNotNull(resultList);
        }
        
        [TestMethod]
        public void Test_GetReportedIssuesOlderThan()
        {
            List<ReportedIssue>? resultList = DBServicesInstance.GetReportedIssuesOlderThan(1);
            Assert.IsNotNull(resultList);
        }

        [TestMethod]
        public void Test_AddFinishedWorkToWorkList_AndReadBackData()
        {
            DBServicesInstance.AddFinishedWorkToWorkList(51, 1, 1);
            DBServicesInstance.SaveChanges();

            //GET DATA BACK
            WorkList lastWork = DBServicesInstance.WorkLists.OrderBy(wl => wl.WorkListId).Last();
            Assert.AreEqual(51, lastWork.IssueId);
            
        }

        [TestMethod]
        public void Test_ReportedIssuesQuery_byNumber_Switch()
        {
            List<ReportedIssue>? resultList = DBServicesInstance.ReportedIssuesQuery_byNumber_Switch(106);
            Assert.IsNotNull(resultList);

        }

        [TestMethod]
        public void Test_GetWorkListByTime()
        {
            List<ExportableWorkList>? resultList = DBServicesInstance.GetWorkListByTime(2023,8);
            Assert.IsNotNull(resultList);

        }

        [TestMethod]
        public void Test_GetWorkListByMonth_GroupByWorkTypes()
        {
            List<ExportableWorkList>? resultList = DBServicesInstance.GetWorkListByMonth_GroupByWorkTypes(2023, 8);
            Assert.IsNotNull(resultList);

        }

        [TestMethod]
        public void Test_GetWorkListByWorkerIDAndTime()
        {
            List<ExportableWorkList>? resultList = DBServicesInstance.GetWorkListByWorkerIDAndTime(6,2023,8);
            Assert.IsNotNull(resultList);

        }

        [TestMethod]
        public void Test_GetWorkListByWorkTypeIDAndTime()
        {
            List<ExportableWorkList>? resultList = DBServicesInstance.GetWorkListByWorkTypeIDAndTime(1, 2023, 8);
            Assert.IsNotNull(resultList);

        }

        [TestMethod]
        public void Test_GetWorkers()
        {
            List<Worker>? resultList = DBServicesInstance.GetWorkers();
            Assert.IsNotNull(resultList);

        }

        [TestMethod]
        public void Test_CreateXML()
        {
            List<ExportableWorkList> resultList = new List<ExportableWorkList>();
            resultList.Add(new ExportableWorkList { WorkListID = 1, ReportedIssue = "ReportedIssue", WorkType = "WorkType",Worker="Worker", FixingDateTime = DateTime.Now});
            DBServicesInstance.CreateXML("file.xml",resultList);

            if (File.Exists("file.xml"))
            {
                Assert.IsNotNull(resultList);
            }
            

        }

    }
}