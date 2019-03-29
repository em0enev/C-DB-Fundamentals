using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTOs.Export
{
    public class ProductDTO
    {
        [JsonProperty(PropertyName = "name")]
        public string  Name { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "seller")]
        public string Seller { get; set; }
    }
}
