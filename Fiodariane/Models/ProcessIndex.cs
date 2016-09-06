using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public class ProcessIndex
    {
        public List<ProcessList> Macro { get; set; }
        public List<ProcessList> Process { get; set; }
        public List<ProcessList> Subprocess { get; set; }
    }
}