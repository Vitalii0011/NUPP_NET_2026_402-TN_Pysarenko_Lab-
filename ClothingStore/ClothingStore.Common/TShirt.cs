using System;

namespace ClothingStore.Common
{
    public class TShirt : Product
    {
        public string Size { get; set; }
        public string Material { get; set; }
        public bool HasPrint { get; set; }

        public TShirt() : base() { }

        public override void ShowInfo()
        {
            Console.WriteLine($"Футболка {Brand} | Розмір: {Size} | Матеріал: {Material} | З принтом: {(HasPrint ? "Так" : "Ні")} | Ціна: {Price} грн");
        }

        private static readonly Random _rnd = new();
        private static readonly string[] Brands = { "Nike", "Adidas", "Puma", "Zara", "H&M", "Under Armour" };
        private static readonly string[] Colors = { "Чорний", "Білий", "Синій", "Червоний", "Сірий" };
        private static readonly string[] Sizes = { "XS", "S", "M", "L", "XL", "XXL" };
        private static readonly string[] Materials = { "Бавовна", "Поліестер", "Льон", "Віскоза" };

        public static TShirt CreateNew()
        {
            return new TShirt
            {
                Brand = Brands[_rnd.Next(Brands.Length)],
                Color = Colors[_rnd.Next(Colors.Length)],
                Size = Sizes[_rnd.Next(Sizes.Length)],
                Material = Materials[_rnd.Next(Materials.Length)],
                HasPrint = _rnd.Next(2) == 0,
                Price = Math.Round((decimal)(_rnd.NextDouble() * (3000 - 300) + 300), 2) 
            };
        }
    }
}