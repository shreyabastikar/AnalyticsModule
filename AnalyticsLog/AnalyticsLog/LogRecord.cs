using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LINQtoCSV;

namespace Analytics
{
    public class LogRecord
    {
        public LogRecord() { }
        public LogRecord(string Name, string Table)
        {

            this.Name = Name;
            this.Table = Table;
            this.BeginTime = DateTime.Now;
            this.Bounce = 0;


        }

        [CsvColumn(Name = "Name", FieldIndex = 1)]
        public string Name { get; set; }

        [CsvColumn(Name = "Table", FieldIndex = 2)]
        public string Table { get; set; }

        [CsvColumn(Name = "BeginTime", FieldIndex = 3, OutputFormat = "MM/dd/yyyy HH:mm:ss")]
        public DateTime BeginTime { get; set; }

        [CsvColumn(Name = "EndTime", FieldIndex = 4, OutputFormat = "MM/dd/yyyy HH:mm:ss")]
        public DateTime EndTime { get; set; }

        [CsvColumn(Name = "Duration", FieldIndex = 5)]
        public int Duration { get; set; }

        [CsvColumn(Name = "Bounce", FieldIndex = 6)]
        public int Bounce { get; set; }

        [CsvColumn(Name = "Journey", FieldIndex = 7)]
        public string Journey { get; set; }

        public void Complete()
        {
            this.EndTime = DateTime.Now;
            TimeSpan ts = this.EndTime - this.BeginTime;
            this.Duration = (int)ts.TotalSeconds;
    
        }
        public void CompleteCDH()
        {
            
            TimeSpan ts = this.EndTime - this.BeginTime;
            this.Duration = (int)ts.TotalSeconds;

        }
            
    }
}
