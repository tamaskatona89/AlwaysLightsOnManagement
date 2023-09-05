/**
 *  Author:           Katona Tamás
 *  E-mail:           katonatomi@msn.com
 *  Course:           CUBIX - C# és .NET fejlesztés alapok, 2023.June - Sept
 *  Project Name:     MINDIG FÉNYES KFT, Company's Working and Issue Management Software
 *  Project Github:   https://github.com/tamaskatona89/AlwaysLightsOnManagement
 *  Project Duration: 2023.08.23....2023.09.06
 */
using AlwaysLightsOnDataModelsDLL;
using AlwaysLightsOnManagement;
namespace AlwaysLightsOnNUnitTest
{
    public class DBServicesNUnitTest
    {
        public DBServices DBServicesInstance { get; set; } = new DBServices();
        public List<Worker>? workerList { get; set; }

        [SetUp]
        public void Setup()
        {
            workerList = new List<Worker>();
            workerList = DBServicesInstance.Workers.ToList();
        }

        [Test]
        public void Test1_GetWorkersListFromDB()
        {
            Assert.IsNotNull(workerList);
        }

        [Test]
        public void Test2_GetWorkersListFromDB_ExpectedItemCountGreaterThan5()
        {
            Assert.IsTrue(workerList?.Count > 5, "Not greater than 5.");

        }

        [Test]
        public void Test3_AddWorkerToDB_ThenGetWorkerDataBack()
        {
            DBServicesInstance.Workers.Add(new Worker { FullName = "Test Test" });
            DBServicesInstance.SaveChanges();
            Worker lastWorker = new();
            lastWorker = DBServicesInstance.Workers.OrderBy(w => w.WorkerId).Last();
            Assert.IsNotNull(lastWorker);
            Assert.That(lastWorker.FullName, Is.EqualTo("Test Test"));

        }

        [Test]
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

            Assert.That(lastWorker.FullName, Is.EqualTo("Test2 Test2"));

        }

        [Test]
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
            Assert.That(lastWorkerId2, Is.Not.EqualTo(lastWorkerId));
        }
    }
}