﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LINQtoCSV;

namespace Analytics
{
    

    public class AnalyticsLog
    {
        public static string OutputFileName;

        public static LogRecord PostSessionStart(int ID, string Name, string Table)
        {

            return new LogRecord(ID, Name, Table);
        }

        public static int PostSessionEnd(string OutputFileName, LogRecord sessionobj)
        {
            if (OutputFileName == null) 
            {
                AnalyticsLog.OutputFileName = "logrecord.csv";
            }
            CsvContext cc = new CsvContext();


            int leftsessionid = 1;

            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                QuoteAllFields = false,
                SeparatorChar = ',', // tab delimited
                FirstLineHasColumnNames = true,

            };


            List<LogRecord> LogRecord = new List<LogRecord>();
            LogRecord obj = new LogRecord { ID = leftsessionid, Name = sessionobj.Name, BeginTime = sessionobj.BeginTime, EndTime = sessionobj.EndTime};
            obj.Complete();
            LogRecord.Add(obj);

            cc.Write(
                LogRecord,
                OutputFileName,
                outputFileDescription);

            return 1;
        }

        
    }
}