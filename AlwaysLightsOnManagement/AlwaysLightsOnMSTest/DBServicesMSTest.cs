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
            DBServicesInstance.Workers.Add (new Worker { FullName = "Test Test"});
            DBServicesInstance.SaveChanges();
            Worker lastWorker = new();
            lastWorker = DBServicesInstance.Workers.OrderBy(w => w.WorkerId).Last();
            Assert.IsNotNull(lastWorker);
            Assert.AreEqual("Test Test",lastWorker.FullName);

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
            Assert.AreNotEqual(lastWorkerId,lastWorkerId2);
        }


    }
}