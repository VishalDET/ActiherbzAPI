using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string LogoUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string SpType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
