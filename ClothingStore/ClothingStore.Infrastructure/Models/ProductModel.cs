using System;
using System.Collections.Generic;

namespace ClothingStore.Infrastructure.Models
{
    public abstract class ProductModel
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();
        public string Brand { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public CategoryModel? Category { get; set; }

        public ProductDetailsModel? Details { get; set; }
        
        public List<OrderModel> Orders { get; set; } = new();
    }
}