using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    
    public class RemoveCart
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int PackageId { get; set; }
    }
}
