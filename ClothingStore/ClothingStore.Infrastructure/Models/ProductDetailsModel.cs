namespace ClothingStore.Infrastructure.Models
{
    public class ProductDetailsModel
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty; 
        public string CountryOfOrigin { get; set; } = string.Empty; 
        public string CareInstructions { get; set; } = string.Empty; 

        public int ProductId { get; set; }
        public ProductModel? Product { get; set; }
    }
}