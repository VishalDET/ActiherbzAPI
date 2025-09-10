using System.Collections.Generic;

namespace SynfoShopAPI.Models
{
    public class CategorySunburst
    {
        public List<string> Ids { get; set; }
        public List<string> Labels { get; set; }
        public List<string> Parents { get; set; }

    }

    public class SunburstData
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string name { get; set; }
        public double value { get; set; }
    }
}
