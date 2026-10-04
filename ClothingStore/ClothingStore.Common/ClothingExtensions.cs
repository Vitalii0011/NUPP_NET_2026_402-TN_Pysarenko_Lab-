namespace ClothingStore.Common
{
    public static class ClothingExtensions
    {
        public static void ApplyDiscount(this Product item, decimal discountPercent)
        {
            if (discountPercent > 0 && discountPercent <= 100)
            {
                item.Price -= item.Price * (discountPercent / 100m);
            }
        }
    }
}