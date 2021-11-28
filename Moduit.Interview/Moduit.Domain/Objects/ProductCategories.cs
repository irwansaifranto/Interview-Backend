using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.Domain.Objects
{
    public class ProductCategories
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("category")]
        public int Category { get; set; }

        [JsonProperty("items")]
        public List<Product> Items { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}
