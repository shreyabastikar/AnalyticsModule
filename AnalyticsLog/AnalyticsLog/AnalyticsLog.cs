using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using LINQtoCSV;

namespace Analytics
{
    
    public class AnalyticsLog
    {
        public static string OutputFileName;
        public static List<LogRecord> currentObjects = new List<LogRecord>();
        public static LogRecord PostSessionStart(string Name, string Table)
        {
            //registers end time of the previous clicked object. 
            if(currentObjects.Count>0)
                currentObjects[currentObjects.Count - 1].Complete();

            LogRecord obj = new LogRecord(Name, Table);
            
            //if it is the first item on the screen
            if(currentObjects.Count==0)
                obj.Journey = "START";
            
            //adds the created object to the list
            currentObjects.Add(obj);
            return obj;
        }

        public static void PostSessionEnd(bool TimedOut)
        {
            if (OutputFileName == null) 
            {
                AnalyticsLog.OutputFileName = "logrecord.csv";
            }
            CsvContext cc = new CsvContext();

            /*This description is for the first entry into the CSV file. First entry
            requires column names, thus FirstLineHasColumnNames = true*/
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                
                SeparatorChar = ',', // tab delimited
                FirstLineHasColumnNames = true,

            };
            /*This description is for the appending into the existing CSV file. First entry
            does not require column names, thus FirstLineHasColumnNames = false*/
            CsvFileDescription outputFileDescription1 = new CsvFileDescription
            {

                SeparatorChar = ',', // tab delimited
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,

            };
            List<LogRecord> LogRecord = new List<LogRecord>();
         
            /*When the final object is clicked and the session is ended by the user (or by
             inactivity, this registers end time of the object last clicked*/
            currentObjects[currentObjects.Count - 1].TimedOut = TimedOut;
            currentObjects[currentObjects.Count - 1].Journey = "END";
            currentObjects[currentObjects.Count-1].Complete();
                try
                {

                    System.IO.StreamWriter w = null;
                    System.IO.TextWriter tw = null;
                    if (!System.IO.File.Exists(OutputFileName))
                    {
                        w = new System.IO.StreamWriter(OutputFileName, true);
                        tw = w;
                        cc.Write(
                        currentObjects,
                        tw, outputFileDescription);
                    }
                    else
                    {
                        w = new System.IO.StreamWriter(OutputFileName, true);
                        tw = w;
                        cc.Write(
                        currentObjects,
                        tw, outputFileDescription1);
                    }
                    
                 
                    w.Close();
                    tw.Close();
                    currentObjects.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            
        }
        
        public static string MaxUsedItem()
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<LogRecord> logrecords =
                cc.Read<LogRecord>(OutputFileName, inputFileDescription);
            // Data is now available via variable logrecords.
            //This query calculates the MAX frequent item
            var maxUsedItemName =
                (from row in logrecords
                 where row.Name != "butLeft"
                 group row by row.Name into g
                 orderby g.Count() descending
                 select g.Key).FirstOrDefault();

            Console.WriteLine("The maximum used item on the screen is ....{0}", maxUsedItemName);
            return maxUsedItemName;
        }
        public static string MinUsedItem()
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<LogRecord> logrecords =
                cc.Read<LogRecord>(OutputFileName, inputFileDescription);
            // Data is now available via variable logrecords.
            //This query calculates the MIN frequent item
            var minUsedItemName =
                (from row in logrecords
                 where row.Name != "butLeft"
                 group row by row.Name into g
                 orderby g.Count() ascending
                 select g.Key).First();

            Console.WriteLine("The minimum used item on the screen is ....{0}", minUsedItemName);
            return minUsedItemName;
        }
                
        public static double BounceRate()
        {

            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<LogRecord> logrecords =
                cc.Read<LogRecord>(OutputFileName, inputFileDescription);
            // Data is now available via variable logrecords.
            var total_journeys =
                (from row in logrecords
                 where row.Journey == "START"
                 select row.Name
                 ).Count();
            var incomplete_journeys =
                (from row in logrecords
                 where row.TimedOut == true
                 select row.Name
                 ).Count();
            var last_used =
                (from row in logrecords
                 where row.TimedOut == true
                 group row by row.Name into g
                 orderby g.Count() descending
                 select g.Key).FirstOrDefault();
            if (last_used == null)
                last_used = "not enough records";
            double bounceRate = ((double)incomplete_journeys / (double)total_journeys) * 100.0;
            Console.WriteLine("The Number of journeys ....{0}", total_journeys);
            Console.WriteLine("The Number of INCOMPLETE journeys ....{0}", incomplete_journeys);
            Console.WriteLine("Bounce rate is ....{0}%", bounceRate);
            Console.WriteLine("Last used item ..... {0}",last_used);
            return bounceRate;

        }
    }
}
