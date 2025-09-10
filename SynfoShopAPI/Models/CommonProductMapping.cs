using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class CommonProductMapping
    {
        public int Id { get; set; }
        public int FranchiseId { get; set; }
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
        public string SpType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
