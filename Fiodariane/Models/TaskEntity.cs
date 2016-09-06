using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public enum TaskEntityType
    {
        Origin,
        Destination
    }

    public class TaskEntity
    {
        [Key]
        [ScaffoldColumn(false)]
        public int TaskEntityID { get; set; }
        public int EntityID { get; set; }
        public TaskEntityType EntityType { get; set; }
        public int TaskID { get; set; }
    }
}