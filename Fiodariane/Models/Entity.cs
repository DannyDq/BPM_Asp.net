using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiodariane.Models
{
    public enum EntityType
    {
        HRFunction,
        OrganisationUnit
    }
    public class Entity
    {
        [Key]
        [ScaffoldColumn(false)]
        public int EntityID { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
    }
}