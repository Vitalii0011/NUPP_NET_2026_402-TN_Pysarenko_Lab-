namespace ClothingStore.Infrastructure.Models
{
    public class TShirtModel : ProductModel
    {
        public string Size { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public bool HasPrint { get; set; }
    }
}