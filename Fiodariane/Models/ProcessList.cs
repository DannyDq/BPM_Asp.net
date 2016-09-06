using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public class ProcessList
    {
        public int ProcessID { get; set; }
        [DisplayFormat(NullDisplayText = "No Name")]
        public string Name { get; set; }
        [DisplayFormat(NullDisplayText = "No Manager")]
        public string Manager { get; set; }
        [Display(Name = "Date")]
        public DateTime CreationTime { get; set; }
    }
}