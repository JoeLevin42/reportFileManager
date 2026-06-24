using System;
using System.IO;
using System.linq;

namespace Repots{
    enum ReportType {Collect,Analyze,Recon,Intell}
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
            Console.WriteLine($"File loaded: {lines.Length} lines found");
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
            type[index] = Enum.Parse<ReportType>(line[1]);
            priority[index] = int.Parse(line[2]);
            score[index] = double.Parse(line[3]);
            status[index] = Enum.Parse< ReportStatus>(line[4]);
            
            
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
             return index;
                
           
        }
            
        static double CalculateAverage(int[] score, int LinesNumber)
        {
            int total_sum = 0;
            foreach (int num in score)
            {
                total_sum += num;
            }
            double avg = (double)total_sum / LinesNumber;
            return avg;
        }

        static double FindMinScore(double[] score)
        {
            double min_find = score[0];
            foreach (int num in score)
            {
                if num < min_find {min_find = num}

            }
            return min_find;
        }

        static double FindMaxScore(double[] score)
        {
            double max_find = score[0];
            foreach (int num in score)
            {
                if num > max_find {max_find = num}
            }
            return max_find;
        }   

        static int CountByStatus(ReportStatus[] statuses, int LinesNumber, ReportStatus targetStatus)
        {   
            int statusCounter = 0
            foreach (ReportStatus stat in statuses)
            {
                if (stat == targetStatus)
                    statusCounter++;
            }
            return statusCounter;
        }

        static int CountByType(ReportType[] )

        static void Main()
        {   
            const string path = "reports.txt"
            string[] unit = new string[100];
            ReportType[] type = new ReportType[100];
            int[] priority = new int[100];
            double[] score = new double[100];
            ReportStatus[] statuses = new ReportStatus[100];

            string[] loadedFile = LoadFile(path);
            string[][] splitedArr = SplitArr(loadedFile);
            int validLinesNumber = ProcessReports(splitedArr, unit, type, priority, score,statuses);



            
        }
    }
}
