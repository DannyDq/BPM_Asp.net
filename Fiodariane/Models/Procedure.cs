using System.ComponentModel.DataAnnotations;

namespace Fiodariane.Models
{
    public class Procedure
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ProcedureID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}