using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public class JsonCheckUpdate
    {
        public int process_id { get; set; }
        public int task_id { get; set; }
        public TaskEntityType operation { get; set; }
        public int[] values { get; set; }
    }
}