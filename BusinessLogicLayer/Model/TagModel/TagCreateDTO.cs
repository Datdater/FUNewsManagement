using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model.TagModel
{
    public class TagCreateDTO
    {
        public required string TagName { get; set; }
        public required string Note { get; set; }
    }
}
