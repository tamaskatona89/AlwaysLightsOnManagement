using AlwaysLightsOnManagement;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main()
        {
            DBServices dBServices = new();

            while (true)
            {
                PrintProgramTitle();
                ReportedIssuesList_InputReader_and_ListPrinter(dBServices);
                Console.Write("Szeretne új lekérdezést indítani? (i/n)>"); string wantNewQuery = GetUserInput_YES_NO();
                if (wantNewQuery.Equals("i"))
                {
                    Console.Clear();
                    continue;
                }
                if (wantNewQuery.Equals("n"))
                    break;
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
        public static int ReportedIssuesList_InputReader_and_ListPrinter(DBServices dBServices)
        {
            //PRINT MENU
            Console.WriteLine(" 4  számjegy = Országos           Lekérdezés Irányítószám alapján,");
            Console.WriteLine(" 3  számjegy = BUDAPESTI-Kerületi Lekérdezés 1[KerSzám] alapján, Üssön 1-et követve a Kerület számával,");
            Console.WriteLine("1-2 számjegy = Országos           Lekérdezés régiség szerint, Most-[Napokszáma]-nál régebben bejelentettek,");
            Console.WriteLine("                                  Ekkor nem lesz mutatva az elmúlt [Napokszáma], csak az ettől korábbiak  .");

            //READ INPUT
            Console.Write("Lekérdezés indítása >");
            var resultList = dBServices.ReportedIssuesQuery_byNumber_Switch(Int32.Parse(Console.ReadLine()!));
            ReportedIssuesListPrinter(resultList);
            if (0 == resultList?.Count)
                return -1;
            else
                return 1;

        }

        public static void ReportedIssuesListPrinter(List<ReportedIssue>? resultList)
        {
            if (resultList == null || resultList.Count == 0)
            {
                Console.WriteLine("Nincs bejelentett nyitott státuszú hibahelyszín .");
            }
            else
            {
                Console.WriteLine("Bejelentett lezáratlan hibahelyszínek listája:\n");
                Console.WriteLine(String.Format("{0,-13}{1,10}{2,60}{3,25}", "Helyszín kód", "Ir.szám", "Cím", "Bejelentve"));
                Console.WriteLine("════════════════════════════════════════════════════════════════════════════════════════════════════════════");
                foreach (var resultItem in resultList)
                {
                    Console.WriteLine(String.Format("{0,-13}{1,10}{2,60}{3,25}", resultItem.IssueId, resultItem.ZipCode, resultItem.Address, resultItem.ReportedDateTime!));
                }
                Console.WriteLine("════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            }
        }

        private static void PrintProgramTitle()
        {
            Console.WriteLine("Bejelentett Nyitott Hibahelyszínek listája");
            Console.WriteLine("══════════════════════════════════════════\n");
        }
    }
}