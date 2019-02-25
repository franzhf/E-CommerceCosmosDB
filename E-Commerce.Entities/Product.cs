using System;

namespace E_Commerce.Entities
{
    public class Product: IEntity
    {
        public string id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string type { get; set; }
    }
}
