using System;
using System.IO;
using System.Linq;

namespace Repots{
    enum ReportType {Collect,Analyze,Recon,Intel}
    enum ReportStatus {Pending,Approved,Rejected}

    class RepotsManager
    {   

        static string[] LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($".Error: File {Path.GetFileName(path)} not found");
                return null;
            }
            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                Console.WriteLine("Error: File is empty");
                return null;
            }
            Console.WriteLine($"     File loaded: {lines.Length} lines found");
            return lines;
        }
        
        static string[][] SplitArr(string[] linesArr)
        {
            string[][] splitedArr = new string[linesArr.Length][];

           for (int i = 0; i < linesArr.Length; i++)
            {
                splitedArr[i] = linesArr[i].Split(',');

            }
            return splitedArr;
                
        }
        static bool ValidatePriority(string priority)
        {
            bool validValue = false;
            int numericPriority;
            bool intParseSuccess = int.TryParse(priority, out numericPriority);
            if (intParseSuccess)
            {
               if (numericPriority>=1 && numericPriority <= 5)
                {
                    validValue = true;
                }
            }
            return validValue;

        }

        static bool ValidateScore(string score)
        {
            bool validValue = false;
            double numericScore;
            bool doubleParseSuccess = double.TryParse(score, out numericScore);
            if (doubleParseSuccess)
            {
                if (numericScore >= 0.0 && numericScore <= 100.0)
                {
                    validValue = true;
                }
            }
            return validValue;
        }

        static bool ValidateReportType(string report)
        {
            ReportType enumReport;
            bool isSuccess = Enum.TryParse<ReportType>(report, true, out enumReport);

            return isSuccess;
        }

        static bool ValidateReportStatus(string status)
        {
            ReportStatus enumStatus;
            bool isSuccess = Enum.TryParse<ReportStatus>(status, true, out enumStatus);

            return isSuccess;
        }

        static bool ValidateUnitName(string unitName)
        {
            unitName = unitName.Trim();
            return unitName.Length > 0;
        }

        static bool ValidateAll(string[] line)
        {
            if (line.Length !=5)
           
            {
                return false;
            }
            if(
            ValidateUnitName(line[0]) &&
            ValidateReportType(line[1]) &&
            ValidatePriority(line[2]) && 
            ValidateScore(line[3]) &&
            ValidateReportStatus(line[4]) 
            )
            {
                return true;
            }
            return false;
        }

        static void AddToArray(int index, string[] line, string[] unit, ReportType[] type, int[] priority, double[] score, ReportStatus[] status)
        {
            unit[index] = line[0];
            type[index] = Enum.Parse<ReportType>(line[1],true);
            priority[index] = int.Parse(line[2]);
            score[index] = double.Parse(line[3]);
            status[index] = Enum.Parse< ReportStatus>(line[4],true);
            
            
        }

        static int ProcessReports(string[][] splitedArr, string[] unit, ReportType[] type, int[] priority, double[] score, ReportStatus[] status)
        {
            int index = 0;
            
                foreach (string[] line in splitedArr)
            {
                 if (ValidateAll(line))
                {
                    AddToArray(index, line, unit, type, priority, score, status);
                    index++;
                    
                }
            }
            Console.WriteLine("        Processing complete.");
            Console.WriteLine($"        Valid records: {index}");
            Console.WriteLine($"        Invalid records: {splitedArr.Length - index}");
             return index;
                
           
        }
            
        static double CalculateAverage(double[] score, int LinesNumber)
        {
            double total_sum = 0;
            foreach (double num in score)
            {
                total_sum += num;
            }
            double avg = total_sum / (double)LinesNumber;
            return avg;
        }

        static double FindMinScore(double[] score)
        {
            double min_find = score[0];
            foreach (double num in score)
            {
                if (num < min_find) { min_find = num; }

            }
            return min_find;
        }

        static double FindMaxScore(double[] score)
        {
            double max_find = score[0];
            foreach (int num in score)
            {
                if (num > max_find) { max_find = num; }
            }
            return max_find;
        }   

        static int CountByStatus(ReportStatus[] statuses, ReportStatus targetStatus,int linesNumber)
        {
            int statusCounter = 0;
            for (int i = 0; i < linesNumber; i++)
            {
                if (statuses[i] == targetStatus)
                {
                    statusCounter++;
                }
            }
            return statusCounter;

        }

        static int CountByType(ReportType[] typeArr , ReportType targetType, int linesNumber) // need be for example (ReportType.something)
        {
            int typeCounter = 0;
            for (int i=0;i< linesNumber; i++)
            {
                if (typeArr[i] == targetType)
                {
                    typeCounter++;
                }
            }
            return typeCounter;
        }

        

        static string DisplayBasicStatistics(double[] score , int linesNumber)
        {
            double avg = CalculateAverage(score, linesNumber);
            double max = FindMaxScore(score);
            double min = FindMinScore(score);


            string printPromt = ($@"
                ===Report Statistics ===
                Total Reports: {linesNumber}
                Average Score: {avg:0.##}
                Highest Score: {max}
                Lowest Score: {min}");
            Console.WriteLine(printPromt);
            return printPromt;
        }


        static string DisplayTypeCounts(ReportType[] type , int LinesNumber)
        { 
            int collectCounter = CountByType(type,ReportType.Collect, LinesNumber);
            int analyzeCounter = CountByType(type,ReportType.Analyze, LinesNumber);
            int reconCounter = CountByType(type,ReportType.Recon, LinesNumber);
            int intelCounter = CountByType(type,ReportType.Intel, LinesNumber);


            string printPromt = ($@"
            ===Reports by Type===
                Collect: {collectCounter}
                Analyze: {analyzeCounter}
                Recon: {reconCounter}
                Intel: {intelCounter}
            ");
            Console.WriteLine(printPromt);
            return printPromt;
        }

        static string DisplayStatusCounts(ReportStatus[] statuses, int linesNumber)
        {
            int pendingCounter = CountByStatus(statuses, ReportStatus.Pending, linesNumber);
            int approvedCounter = CountByStatus(statuses, ReportStatus.Approved, linesNumber);
            int rejectedCounter = CountByStatus(statuses, ReportStatus.Rejected, linesNumber);

            string printPromt = ($@"
            ===Reports by Status===
                Approved: {approvedCounter}
                Pending: {pendingCounter}
                Rejected: {rejectedCounter}
            ");
            Console.WriteLine(printPromt);
            return printPromt;
        }

        static string DisplayHighestPriorityApproved(string[] unit, ReportType[] type, int[] priority, double[] score, ReportStatus[] statuses, int numberLines)
        {   //return only the first!!!!
            int highestIndex= -1;
            int maxPriority=0;
            for (int i = 0; i < numberLines; i++)
            {
                if (statuses[i] == ReportStatus.Approved)
                {
                    if (priority[i] > maxPriority)
                    {
                        maxPriority = priority[i];
                        highestIndex = i;
                    }
                }
            }
            string printPromt = "Not Found";
            if (highestIndex >= 0) {


                printPromt = ($@"
           ===Highest Priority Approved Report===
                    Unit: {unit[highestIndex]}
                    Type: {type[highestIndex]}
                    Priority: {priority[highestIndex]}
                    Score: {score[highestIndex]}
                ");
            }
            

            Console.WriteLine(printPromt);
            return printPromt;
        }


        static string DisplayAverageByPriority(int[] priority, double[] score , int linesNumber)
        {
            
            int incrementPriority = 1;
            double[] avgArr = new double[5];
            
            for (int pLevel = 0; pLevel< 5; pLevel++)
            {
                double totalSum = 0;
                int count = 0;
                for (int i = 0; i < linesNumber; i++)
                {
                    if (priority[i] == pLevel+1)
                    {
                        totalSum += score[i];
                        count++;
                    }
                }
                if (count > 0)
                {
                    double avg = totalSum / (double)count;
                    avgArr[pLevel] = avg;
                }
               }
            string printPromt = $@"
           ===Average Score by Priority===
                Priority 1: {(avgArr[0] > 0 ? avgArr[0].ToString("F2") : "reports")}
                Priority 2: {(avgArr[1] > 0 ? avgArr[1].ToString("F2") : "No reports")}
                Priority 3: {(avgArr[2] > 0 ? avgArr[2].ToString("F2") : "No reports")}
                Priority 4: {(avgArr[3] > 0 ? avgArr[3].ToString("F2") : "No reports")}
                Priority 5: {(avgArr[4] > 0 ? avgArr[4].ToString("F2") : "No reports")}
            ";

            Console.WriteLine(printPromt);
            return printPromt;
        }
                            
        static void WriteToOutFile(string path,string basicStaticsPromt , string countStatusPromt, string countTypePromt, string highestPrApPromt, string avgByPrPromt)
        {
            string writeAllPromt = $@"
            {basicStaticsPromt}
            {countStatusPromt}
            {countTypePromt}
            {highestPrApPromt}
            {avgByPrPromt}
            ";

            File.WriteAllText(path, writeAllPromt);
        }
        static void Main()
        {
            const string path = "\\bin\\Debug\\net10.0\\reports.txt";
            const string outpath = "\\bin\\Debug\\net10.0\\output.txt";
            string[] unit = new string[100];
            ReportType[] type = new ReportType[100];
            int[] priority = new int[100];
            double[] score = new double[100];
            ReportStatus[] statuses = new ReportStatus[100];

            string[] loadedFile = LoadFile(path);
            string[][] splitedArr = SplitArr(loadedFile);
            int validLinesNumber = ProcessReports(splitedArr, unit, type, priority, score,statuses);

            string basicStatPromt = DisplayBasicStatistics(score, validLinesNumber);
            string countStatusPromt = DisplayStatusCounts(statuses, validLinesNumber);
            string countTypePromt = DisplayTypeCounts(type, validLinesNumber);
            string highestPrApPromt =DisplayHighestPriorityApproved(unit, type, priority, score, statuses, validLinesNumber);
            string avgByPrPromt = DisplayAverageByPriority(priority, score, validLinesNumber);
            WriteToOutFile(outpath, basicStatPromt, countStatusPromt, countTypePromt, highestPrApPromt, avgByPrPromt);
        }
    }
}

// need to refactore the calculate avg by  piroity
// need to diaplay numers of validline / invalid lines 