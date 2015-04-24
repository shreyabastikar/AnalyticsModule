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
            this.TimedOut = false;

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

        /*0 if close button is used intentionally, 1 if the user walks away from the screen
        without ending the session*/
        [CsvColumn(Name = "Bounce", FieldIndex = 6)]
        public bool TimedOut { get; set; }

        /*Journey has two string values, START to marks the beginning of the journey and END
         to mark the end of the journey - complete or incomplete */
        [CsvColumn(Name = "Journey", FieldIndex = 7)]
        public string Journey { get; set; }

        public void Complete()
        {
            this.EndTime = DateTime.Now;
            TimeSpan ts = this.EndTime - this.BeginTime;
            this.Duration = (int)ts.TotalSeconds;
    
        }
                   
    }
}
