using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Inventory.Manager
{
    public class ProductException: Exception
    {
        public string CustomMessage { get; }

        public ProductException(string m, Exception ex):base(ex.Message)
        {
            CustomMessage = m;            
        }    
    }
}
