using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string MainImage { get; set; }
        public int ParentId { get; set; }
        public int CategoryLevel { get; set; }
        public int DisplayOrder { get; set; }
        public string SpType { get; set; }
        public string CategoryHeading { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

    }

    public class GetCategoriesRequest
    {
        public List<int> ParentIds { get; set; }
    }

    public class CategoryView
    {
        public int main_category_id { get; set; }
        public string main_category_name { get; set; }
        public string main_category_image_url { get; set; }
        public List<SubCategoryView> sub_categories { get; set; }

    }

    public class SubCategoryView
    {
        public int sub_category_id { get; set; }
        public string sub_category_name { get; set; }
        public string sub_category_image_url { get; set; }
        public List<childCategoryView> child_category { get; set; }

    }
    public class childCategoryView
    {
        public int child_category_id { get; set; }
        public string child_category_name { get; set; }
        public string child_category_image_url { get; set; }

    }
}
