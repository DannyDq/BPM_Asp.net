using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public class JsonSysUrl
    {
        public int TaskID { get; set; }
        public int SysUrlID { get; set; }
        public SysUrlType SysUrlType { get; set; }
        public TaskType OperationType { get; set; }
    }
}