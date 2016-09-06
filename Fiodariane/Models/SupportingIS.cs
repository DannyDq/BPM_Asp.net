using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public class SupportingIS
    {
        [Key]
        [ScaffoldColumn(false)]
        public int SupportingISID { get; set; }
        public string Name { get; set; }
    }
}