using System;

namespace ClothingStore.Common
{
    public class Shoes : Product
    {
        public double ShoeSize { get; set; }
        public string Season { get; set; }
        public bool IsWaterproof { get; set; }

        public Shoes() : base() { }

        public override void ShowInfo()
        {
            Console.WriteLine($"Взуття {Brand} | Розмір: {ShoeSize} | Сезон: {Season} | Водонепроникне: {(IsWaterproof ? "Так" : "Ні")} | Ціна: {Price} грн");
        }
    }
}