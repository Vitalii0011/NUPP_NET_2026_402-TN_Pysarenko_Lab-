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
    }
}