using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model.CategoryModel
{
    public class CategoryCreateDTO
    {
        public required string CategoryName { get; set; }
        public required string CategoryDescription { get; set; }
        public int? ParentCategoryID { get; set; }
        public bool IsActive { get; set; }
    }
}
