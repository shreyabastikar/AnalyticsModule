using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LINQtoCSV;

namespace mcMath
{
    public class LogRecord
    {
        public LogRecord() { }
        public LogRecord(int ID, string Name, string Table) {

            this.ID = ID;
            this.Name = Name;
            this.Table = Table;
        
        }

        [CsvColumn(Name = "ID", FieldIndex = 1)]
        public int ID { get; set; }

        [CsvColumn(Name = "Name", FieldIndex = 2)]
        public string Name { get; set; }

        [CsvColumn(Name = "Table", FieldIndex = 3)]
        public string Table { get; set; }

        [CsvColumn(Name = "BeginTime", FieldIndex = 4, OutputFormat = "dd MMM HH:mm:ss")]
        public DateTime BeginTime { get; set; }

        [CsvColumn(Name = "EndTime", FieldIndex = 5, OutputFormat = "dd MMM HH:mm:ss")]
        public DateTime EndTime { get; set; }


    }

    public class AnalyticsLog
    {
        private bool bTest = false;
        public AnalyticsLog()
        {
            // 
            // TODO: Add constructor logic here 
            // 
        }
        /// <summary> 
        /// //This is a test method 
        /// </summary> 
        public void mcTestMethod()
        {
        }
        /// <summary>
        /// //This is a test property
        /// </summary>
        public bool Extra
        {
            get
            {
                return bTest;
            }
            set
            {
                bTest = Extra;
            }
        }

        public long Add(long val1, long val2)
        {
            return val1 + val2;
        }
        public LogRecord PostSessionStart(int ID, string Name, string Table)
        {
            

            return new LogRecord(ID, Name, Table);
        }

        public int PostSessionEnd(LogRecord sessionobj)
        {

            CsvContext cc = new CsvContext();

           
            int leftsessionid = 1;
           
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                QuoteAllFields = false,
                SeparatorChar = ',', // tab delimited
                FirstLineHasColumnNames = true,

            };


            List<LogRecord> LogRecord = new List<LogRecord>();
            LogRecord.Add(new LogRecord { ID = leftsessionid, Name = sessionobj.Name });
            

            cc.Write(
                LogRecord,
                "logrecord.csv",
                outputFileDescription);

            return 1;
        }
    }
}
