using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class State
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public int StateCode { get; set; }
        public int CountryId { get; set; }

    }
}
