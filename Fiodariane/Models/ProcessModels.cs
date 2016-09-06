using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiodariane.Models
{
    public class ProcessModels
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ProcessID { get; set; }
        [Display(Name = "Parent")]
        [ForeignKey("Parent")]
        [DisplayFormat(NullDisplayText = "Macro-Process")]
        public int? ParentID { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Procedure")]
        [ForeignKey("Procedure")]
        [DisplayFormat(NullDisplayText = "No Procedure")]
        public int? ProcedureID { get; set; }
        [Display(Name = "Manager")]
        [ForeignKey("Manager")]
        [DisplayFormat(NullDisplayText = "No Manager")]
        public int? ManagerID { get; set; }
        [Display(Name = "Flux Diagram")]
        public string FluxDiagram { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Date")]
        public DateTime CreationTime { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int Level { get; set; }

        public virtual ProcessModels Parent { get; set; }
        public virtual Procedure Procedure { get; set; }
        public virtual Manager Manager { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}