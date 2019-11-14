using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VillaMouzinho.Web.Models.Board.CMS
{
    public class CreatePageModel
    {
        public CreatePageModel()
        {
            modules = new List<ModuleDto>();
        }

        public PagerHeaderDto pageHeader { get; set; } 
        public List<ModuleDto> modules { get; set; }
    }

    public class PagerHeaderDto
    {
        public string id { get; set; }
        public int status { get; set; }
        public int type { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string location { get; set; }        
    }

    public class ModuleDto
    {
        public ModuleDto()
        {
            attributes = new List<SelectedAttributoDto>();
            images = new List<ImageDto>();
        }
        public string type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string imageName { get; set; }
        public List<SelectedAttributoDto> attributes { get; set; }
        public List<ImageDto> images { get; set; }
        public string higherPrice { get; set; }
        public string lowerPrice { get; set; }
    }

    public class SelectedAttributoDto
    {
        public int id { get; set; }
        public string attributeName { get; set; }
        public int valueId { get; set; }
        public string attributeValueName { get; set; }
    }

    public class ImageDto
    {
        public string image { get; set; }
        public string name { get; set; }
    }
}