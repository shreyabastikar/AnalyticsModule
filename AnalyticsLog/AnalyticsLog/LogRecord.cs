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
        public LogRecord(int ID, string Name, string Table)
        {

            this.ID = ID;
            this.Name = Name;
            this.Table = Table;
            this.BeginTime = DateTime.Now;

        }

        [CsvColumn(Name = "ID", FieldIndex = 1)]
        public int ID { get; set; }

        [CsvColumn(Name = "Name", FieldIndex = 2)]
        public string Name { get; set; }

        [CsvColumn(Name = "Table", FieldIndex = 3)]
        public string Table { get; set; }

        [CsvColumn(Name = "BeginTime", FieldIndex = 4, OutputFormat = "MM/dd/yyyy HH:mm:ss")]
        public DateTime BeginTime { get; set; }

        [CsvColumn(Name = "EndTime", FieldIndex = 5, OutputFormat = "MM/dd/yyyy HH:mm:ss")]
        public DateTime EndTime { get; set; }

        [CsvColumn(Name = "Duration", FieldIndex = 6)]
        public int Duration { get; set; }

        public void Complete()
        {
            this.EndTime = DateTime.Now;
            TimeSpan ts = this.EndTime - this.BeginTime;
            this.Duration = (int)ts.TotalSeconds;
            

        }
            
    }
}
