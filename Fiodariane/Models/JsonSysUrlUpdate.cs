using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public class JsonSysUrlUpdate
    {
        public int process_id { get; set; }
        public int task_id { get; set; }
        public int sysurl_id { get; set; }
        public SysUrlType operation { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string embedded { get; set; }
    }
}