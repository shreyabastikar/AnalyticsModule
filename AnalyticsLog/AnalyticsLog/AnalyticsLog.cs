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

        public static LogRecord PostSessionStart(string Name, string Table)
        {

            return new LogRecord(Name, Table);
        }

        public static int PostSessionEnd(string OutputFileName, List<LogRecord> currentObjects)
        {
            if (OutputFileName == null) 
            {
                AnalyticsLog.OutputFileName = "logrecord.csv";
            }
            CsvContext cc = new CsvContext();
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                
                SeparatorChar = ',', // tab delimited
                FirstLineHasColumnNames = true,

            };
            CsvFileDescription outputFileDescription1 = new CsvFileDescription
            {

                SeparatorChar = ',', // tab delimited
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,

            };
            List<LogRecord> LogRecord = new List<LogRecord>();
           // LogRecord obj = new LogRecord { Name = sessionobj.Name, BeginTime = sessionobj.BeginTime, EndTime = sessionobj.EndTime};
            for (int i = 0; i < currentObjects.Count-1; i++)
            {
                currentObjects[i].EndTime = currentObjects[i + 1].BeginTime;
                currentObjects[i].CompleteCDH();
                
            }

            currentObjects[currentObjects.Count-1].Complete();
            
          /*  cc.Write(
                currentObjects,
                OutputFileName,
                outputFileDescription);
            */
          //  using (TextWriter writer = new StreamWriter( ConfigurationManager.AppSettings["OutputFile"], true))
            /*{
                var context = new CsvContext();
                context.Write(cardholders, writer, outputDescription);
            } */
          //  cc.Write(currentObjects, new System.IO.StreamWriter( System.Configuration.ConfigurationManager.AppSettings["OutputFile"], true))
            
            //tw.FormatProvider()
            for (int i = 0; i < currentObjects.Count; i++) {

                Console.WriteLine(currentObjects[i].Name);
            
            }
                try
                {

                    System.IO.StreamWriter w = null;
                    System.IO.TextWriter tw = null;
                    if (!System.IO.File.Exists("logrecord.csv"))
                    {
                        w = new System.IO.StreamWriter("logrecord.csv", true);
                        tw = w;
                        cc.Write(
                        currentObjects,
                        tw, outputFileDescription);
                    }
                    else
                    {
                        w = new System.IO.StreamWriter("logrecord.csv", true);
                        tw = w;
                        cc.Write(
                        currentObjects,
                        tw, outputFileDescription1);
                    }
                    Console.WriteLine("*****************************************************");
                    Console.WriteLine(w);
                    Console.WriteLine("*****************************************************");

                    
                    

                    w.Close();
                    tw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }



            return 1;
        }
        
        
    }
}
