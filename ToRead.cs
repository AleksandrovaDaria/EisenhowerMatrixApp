using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace EisenhowerMatrixApp
{
    public class ToRead
    {
        [Name(name: "Title")]
        public string Title { get; set; }

        [Name(name: "Deadline")]
        public DateTime Deadline { get; set; }

        [Name(name: "IsDone")]
        public bool IsDone { get; set; }

    }
}
   
