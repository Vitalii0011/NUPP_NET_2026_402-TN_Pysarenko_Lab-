using System;

namespace ClothingStore.Common
{
    public class Pants : Product
    {
        public int WaistSize { get; set; }
        public int Length { get; set; }
        public string FitType { get; set; }

        public Pants() : base() { }

        public override void ShowInfo()
        {
            Console.WriteLine($"Штани {Brand} | Пояс: {WaistSize} | Довжина: {Length} | Фасон: {FitType} | Ціна: {Price} грн");
        }
    }
}