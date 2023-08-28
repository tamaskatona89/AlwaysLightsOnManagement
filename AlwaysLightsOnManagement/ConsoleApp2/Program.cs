using AlwaysLightsOnManagement;

namespace ConsoleApp2
{
    public class Program
    {
        public static void Main()
        {
            //DB Handler
            DBServices dBServices = new();


            //List<ReportedIssue>? reportedIssues3333 = dBServices.GetReportedIssuesByZIPCode(3333);
            //List<ReportedIssue>? reportedIssuesBP_VI_District = dBServices.GetBudapestReportedIssuesByDistrict(106);

            //List<ReportedIssue>? issuesOlderThan_2 = dBServices.GetReportedIssuesOlderThan(2);
            //if (issuesOlderThan_2 is null)
            //{
            //    Console.WriteLine("Nincs bejelentés...");
            //}
            //else
            //    foreach (var item in issuesOlderThan_2)
            //    {
            //        Console.WriteLine(item);
            //    }

            PrintProgramTitle();

            //GATHER ID-s from User Input
            int selectedWorkerID = WorkersListInputPrinterAndReader(dBServices);
        SelectNewReportedIssue:
            int selectedIssueID = ReportedIssuesInputPrinterAndReader(dBServices);
        SelectTheWorkType:
            int selectedWorkTypeID = WorkTypesInputPrinterAndReader(dBServices);

            //INSERT INTO DB with the Collected ID-s
            //INSERT INTO WorkList(Issue_ID,WorkType_ID,Worker_ID) VALUES(1,2,2);
            dBServices.AddFinishedWorkToWorkList(selectedIssueID, selectedWorkTypeID, selectedWorkerID);
            dBServices.SaveChanges();
        OtherJobsQuestion:
            Console.Write("\nMUNKAVéGZéS MENTVE!\nVolt még egyéb teendő ugyanitt? (i/n) (...egyéb=Kilép) >");
            string menuSelected = Console.ReadLine();
            if (menuSelected.Equals("i"))
            {
                goto SelectTheWorkType;
            }
            if (menuSelected.Equals("n"))
            {
                //SET CURRENT ReportedIssue to isFixed=TRUE and SELECT NEW ReportedIssue
                //SET CURRENT WORK ReportedIssue IsFixed State --> true
                dBServices.ReportedIssues.First(ri => ri.IssueId == selectedIssueID).IsFixed = true;
                dBServices.SaveChanges();
                Console.WriteLine("Aktuális hibahelyszín lezárva! Javítás befejezett !");

                goto SelectNewReportedIssue;
            }

            //SET CURRENT ReportedIssue to isFixed=TRUE and SELECT NEW ReportedIssue
            //SET CURRENT WORK ReportedIssue IsFixed State --> true
            dBServices.ReportedIssues.First(ri => ri.IssueId == selectedIssueID).IsFixed = true;
            dBServices.SaveChanges();
            Console.WriteLine("Aktuális hibahelyszín lezárva! Javítás befejezett !");
            Console.WriteLine("Program vége.");


        }

        private static int WorkTypesInputPrinterAndReader(DBServices dBServices)
        {
            Console.WriteLine("\nMilyen típusú munkát kellett elvégezni?");
            Console.WriteLine("Munkavégzés-típusok listája:\n");
            List<WorkType> workTypes = dBServices.WorkTypes.ToList();
            Console.WriteLine(String.Format("{0,-13}{1,20}", "Munka kód", "Munkavégzés"));
            Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════");
            foreach (var workType in workTypes)
            {
                Console.WriteLine(String.Format("{0,-13}{1,20}", workType.WorkTypeId, workType.WorkTypeDescription));
            }
            Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════");
            Console.Write("Milyen munkavégzés történt? (Munka kódja) > ");
            List<int> acceptableWorkTypeIDs = dBServices.WorkTypes.Select(wt => wt.WorkTypeId).ToList();
            int selectedWorkTypeID = WorkTypeIDReader(acceptableWorkTypeIDs);
            return selectedWorkTypeID;
        }

        private static int WorkTypeIDReader(List<int> acceptableWorkTypeIDs)
        {
            int selectedWorkTypeID = -1;
            while (!acceptableWorkTypeIDs.Contains(selectedWorkTypeID))
            {
                var input = Console.ReadLine();
                int.TryParse(input, out selectedWorkTypeID);
            }
            Console.WriteLine("Kiválasztott Munkavégzés kódja: {0}", selectedWorkTypeID);
            return selectedWorkTypeID;
        }

        private static int ReportedIssuesInputPrinterAndReader(DBServices dBServices)
        {
            Console.WriteLine("\nHol történt a munkavégzés?");
            Console.WriteLine("Bejelentett lezáratlan hibahelyszínek listája:\n");
            List<ReportedIssue> reportedIssues = dBServices.ReportedIssues.Where(ri => ri.IsFixed == false).ToList();
            Console.WriteLine(String.Format("{0,-13}{1,10}{2,60}", "Helyszín kód", "Ir.szám", "Cím"));
            Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════");
            foreach (var reportedIssue in reportedIssues)
            {
                Console.WriteLine(String.Format("{0,-13}{1,10}{2,60}", reportedIssue.IssueId, reportedIssue.ZipCode, reportedIssue.Address));
            }
            Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════");
            Console.Write("Hol történt? (Helyszín kódja) > ");
            List<int> acceptableIssueIDs = dBServices.ReportedIssues.Select(ri => ri.IssueId).ToList();
            int selectedIssueID = ReportedIssueIDReader(acceptableIssueIDs);
            return selectedIssueID;
        }

        private static int ReportedIssueIDReader(List<int> acceptableIssueIDs)
        {
            int selectedIssueID = -1;
            while (!acceptableIssueIDs.Contains(selectedIssueID))
            {
                var input = Console.ReadLine();
                int.TryParse(input, out selectedIssueID);
            }
            Console.WriteLine("Kiválasztott Helyszín kódja: {0}", selectedIssueID);
            return selectedIssueID;
        }

        private static int WorkerIDReader(List<int> acceptableWorkerIDs)
        {
            int selectedWorkerID = -1;
            while (!acceptableWorkerIDs.Contains(selectedWorkerID))
            {
                var input = Console.ReadLine();
                int.TryParse(input, out selectedWorkerID);
            }
            Console.WriteLine("Kiválasztott Dolgozói kód: {0}", selectedWorkerID);
            return selectedWorkerID;
        }

        private static int WorkersListInputPrinterAndReader(DBServices dBServices)
        {
            Console.WriteLine("Dolgozói lista:\n");
            List<Worker> workers = dBServices.Workers.ToList();
            Console.WriteLine(String.Format("{0,-13}{1,20}", "Dolgozói kód", "Teljes név"));
            Console.WriteLine("═════════════════════════════════");
            foreach (var worker in workers)
            {
                Console.WriteLine(String.Format("{0,-13}{1,20}", worker.WorkerId, worker.FullName));
            }
            Console.WriteLine("═════════════════════════════════");
            Console.Write("Ki végezte? (Dolgozó kódja) > ");
            List<int> acceptableWorkerIDs = dBServices.Workers.Select(w => w.WorkerId).ToList();
            int selectedWorkerID = WorkerIDReader(acceptableWorkerIDs);
            return selectedWorkerID;
        }

        private static void PrintProgramTitle()
        {
            Console.WriteLine("Nap végi munkavégzés adminisztrációja");
            Console.WriteLine("═════════════════════════════════════\n");
        }
    }
}