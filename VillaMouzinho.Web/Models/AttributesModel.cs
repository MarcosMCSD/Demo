using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VillaMouzinho.Business.DB;

namespace VillaMouzinho.Web.Models
{
    public class AttributesModel
    {
        public attributes Attribute { get; set; }
        public List<attributes> Attributes { get; set; }
    }
}