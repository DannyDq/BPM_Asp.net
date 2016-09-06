using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Fiodariane.Models
{
    public class Tasks
    {
        [Key]
        [ScaffoldColumn(false)]
        public int TaskID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int Order { get; set; }
        [DisplayName("Name")]
        [ForeignKey("Name")]
        public int NameID { get; set; }
        [DisplayName("Supporting IS")]
        [ForeignKey("SupportingIS")]
        [DisplayFormat(NullDisplayText = "No Supporting IS")]
        public int? SupportingISID { get; set; }
        [DisplayName("HR Function")]
        [ForeignKey("HRFunction")]
        [DisplayFormat(NullDisplayText = "No HR Function")]
        public int? HRFunctionID { get; set; }
        [DisplayName("Organisation Unit")]
        [ForeignKey("OrganisationUnit")]
        [DisplayFormat(NullDisplayText = "No Organisation Unit")]
        public int? OrganisationUnitID { get; set; }
        [DisplayName("Skills")]
        [ForeignKey("HRSkills")]
        [DisplayFormat(NullDisplayText = "No Skills")]
        public int? HRSkillsID { get; set; }
        [DisplayName("Archive")]
        [ForeignKey("Archive")]
        [DisplayFormat(NullDisplayText = "No Archive")]
        public int? ArchiveID { get; set; }
        [DisplayName("KPI")]
        [ForeignKey("KPI")]
        [DisplayFormat(NullDisplayText = "No KPI")]
        public int? KPIID { get; set; }
        [ForeignKey("Process")]
        [HiddenInput(DisplayValue = false)]
        public int ProcessID { get; set; }

        public virtual ICollection<SysUrl> Input { get; set; }
        public virtual ICollection<SysUrl> Output { get; set; }
        public virtual ICollection<Entity> Origin { get; set; }
        public virtual ICollection<Entity> Destination { get; set; }
        public virtual ICollection<SysUrl> DocRef { get; set; }

        public virtual SysUrl Name { get; set; }
        public virtual SupportingIS SupportingIS { get; set; }
        public virtual Entity HRFunction { get; set; }
        public virtual Entity OrganisationUnit { get; set; }
        public virtual HRSkills HRSkills { get; set; }
        public virtual ProcessModels Process { get; set; }
        public virtual SysUrl Archive { get; set; }
        public virtual SysUrl KPI { get; set; }
    }
}