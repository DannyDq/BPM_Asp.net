using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public enum TaskType
    {
        Add,
        Delete,
        Clear
    }
    public class JsonTask
    {
        public int ProcessID { get; set; }
        public int TaskID { get; set; }
        public TaskType TaskType { get; set; }
    }
}