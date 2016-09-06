using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public class ProcessTreeModel
    {
        public List<ProcessSummary> Macro { get; set; }
        public List<ProcessSummary> Process { get; set; }
        public List<ProcessSummary> SubProcess { get; set; }
    }
}