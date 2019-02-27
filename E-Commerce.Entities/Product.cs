using System;
using Newtonsoft.Json;
namespace E_Commerce.Entities
{
    public class Product: IEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "priceWithTaxes")]
        public double PriceWithTaxes { get; set; }
    }
}
