using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiodariane.Models
{
    public enum SysUrlType
    {
        Name,
        Input,
        Output,
        DocRef,
        Archive,
        KPI
    }

    public class SysUrl
    {
        [Key]
        [ScaffoldColumn(false)]
        public int SysUrlID { get; set; }
        [StringLength(300)]
        [DataType(DataType.Url)]
        public string Url { get; set; }
        [StringLength(160)]
        public string Description { get; set; }
        [StringLength(500)]
        public string Embedded { get; set; }
        public SysUrlType SysUrlType { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int TaskID { get; set; }
    }
}