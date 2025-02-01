using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailScanner
{
    internal class MyEmail
    {
        public string? Subject { get; set; }
        public string? From { get; set; }
        public DateTime Date { get; set; }
        public string? Body { get; set; }
        public string? Note { get; set; } 
    }
}
