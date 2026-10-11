using System;
using System.Collections.Generic;

namespace ClothingStore.Infrastructure.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string CustomerName { get; set; } = string.Empty;
        public List<ProductModel> Products { get; set; } = new();
    }
}