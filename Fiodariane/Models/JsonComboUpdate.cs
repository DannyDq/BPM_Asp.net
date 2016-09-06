using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiodariane.Models
{
    public enum OperationType
    {
        Name,
        Parent,
        Procedure,
        Manager,
        SupportingIS,
        HRFunction,
        OrganisationUnit,
        Skills
    }
    public class JsonComboUpdate
    {
        public int Identifier { get; set; }
        public OperationType Operation { get; set; }
        public string Value { get; set; }
        public int TaskID { get; set; }
    }
}