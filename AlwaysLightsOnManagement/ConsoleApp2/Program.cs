﻿/**
 *  Author:           Katona Tamás
 *  E-mail:           katonatomi@msn.com
 *  Course:           CUBIX - C# és .NET fejlesztés alapok, 2023.June - Sept
 *  Project Name:     MINDIG FÉNYES KFT, Company's Working and Issue Management Software
 *  Project Github:   https://github.com/tamaskatona89/AlwaysLightsOnManagement
 *  Project Duration: 2023.08.23....2023.09.06
 */
using AlwaysLightsOnManagement;
using ConsoleApp1;

namespace ConsoleApp2
{
    public class Program
    {
        public static void Main()
        {
            //DB Handler
            DBServices dBServices = new();

            PrintProgramTitle();

            //GATHER ID-s from User Input
            int selectedWorkerID = WorkersListInputPrinterAndReader(dBServices);
            bool reportedIssue_MENU_ExitFlag = false;
            bool workType_MENU_ExitFlag = false;

            while (!reportedIssue_MENU_ExitFlag)
            {
                int selectedIssueID = ReportedIssuesInputPrinterAndReader(dBServices);

                while (!workType_MENU_ExitFlag)
                {
                    int selectedWorkTypeID = WorkTypesInputPrinterAndReader(dBServices);

                    //INSERT INTO DB with the Collected ID-s
                    //INSERT INTO WorkList(Issue_ID,WorkType_ID,Worker_ID) VALUES(1,2,2);
                    dBServices.AddFinishedWorkToWorkList(selectedIssueID, selectedWorkTypeID, selectedWorkerID);
                    dBServices.SaveChanges();

                    // OtherJobsQuestion for the Same Issue
                    Console.Write("\nMUNKAVéGZéS MENTVE!\nVolt még egyéb teendő ugyanitt? (i/n) >");
                    string workType_menuSelected = GetUserInput_YES_NO();
                    if (workType_menuSelected.Equals("i"))
                        continue;
                    if (workType_menuSelected.Equals("n"))
                    {
                        //SET CURRENT ReportedIssue to isFixed=TRUE and SELECT NEW ReportedIssue
                        //SET CURRENT WORK ReportedIssue IsFixed State --> true
                        dBServices.ReportedIssues.First(ri => ri.IssueId == selectedIssueID).IsFixed = true;
                        dBServices.SaveChanges();
                        Console.WriteLine("Aktuális hibahelyszín lezárva! Javítás befejezett !");
                        break;
                    }
                }

                // Question for new Issue
                Console.Write("\nVálaszt másik hibahelyszínt?\nÚj helyszín=(i), kilép=(n) (i/n) >");
                string reportedIssue_menuSelected = GetUserInput_YES_NO();
                if (reportedIssue_menuSelected.Equals("i"))
                    continue;
                if (reportedIssue_menuSelected.Equals("n"))
                {
                    reportedIssue_MENU_ExitFlag = true;
                    break;
                }
            }
            Console.WriteLine("Program vége.");
        }

        private static string GetUserInput_YES_NO()
        {
            string input;
            do
            {
                input = Console.ReadLine()!.ToLower();
                if ((input == "i") || (input == "n"))
                    break;

            } while (true);
            return input;
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
            while (true)
            {
                Console.WriteLine("\nHol történt a munkavégzés?");
                Console.WriteLine("══════════════════════════");
                int resultListHasEntries = ConsoleApp1.Program.ReportedIssuesList_InputReader_and_ListPrinter(dBServices);

                if (resultListHasEntries == -1)
                {
                    Console.Error.WriteLine("Nincs kiválasztható listaelem.\nÜss ENTER-t és szűrj más értékre!"); Console.ReadLine();
                    continue;
                }
                else
                {
                    Console.Write("Hol történt? (Helyszín kódja) > ");
                    List<int> acceptableIssueIDs = dBServices.ReportedIssues.Select(ri => ri.IssueId).ToList();
                    int selectedIssueID = ReportedIssueIDReader(acceptableIssueIDs);
                    return selectedIssueID;
                }
            }
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