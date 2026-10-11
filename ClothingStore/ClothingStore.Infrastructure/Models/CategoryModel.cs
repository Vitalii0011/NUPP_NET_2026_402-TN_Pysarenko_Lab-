using System.Collections.Generic;

namespace ClothingStore.Infrastructure.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductModel> Products { get; set; } = new();
    }
}