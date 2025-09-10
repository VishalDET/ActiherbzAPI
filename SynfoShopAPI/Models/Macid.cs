using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class Macid
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public int PackageId { get; set; }

        public int FranchiseId { get; set; }

        public string MacId { get; set; }

        public int OrderId { get; set; }

        public int OrderDetailsId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string SpType { get; set; }

        public int Result { get; set; }

    }
}
