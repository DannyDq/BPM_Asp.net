using System.ComponentModel.DataAnnotations;

namespace Fiodariane.Models
{
    public class Manager
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ManagerID { get; set; }
        public string Name { get; set; }
    }
}