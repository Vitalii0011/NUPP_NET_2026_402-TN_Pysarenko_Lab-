using System;

namespace ClothingStore.Common
{
    public delegate void PriceNotify(string message);

    public abstract class Product : IEntity
    {
        public static string StoreName = "Brand clothing";
        public static int TotalItemsCreated;

        public event PriceNotify PriceChanged;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Brand { get; set; }
        public string Color { get; set; }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    PriceChanged?.Invoke($"[Подія]: Ціну на {Brand} змінено на {value} грн");
                }
            }
        }

        static Product()
        {
            TotalItemsCreated = 0;
        }

        public Product()
        {
            TotalItemsCreated++;
        }

        public static decimal CalcTax(decimal amount) => amount * 0.20m;

        public virtual void ShowInfo()
        {
            Console.WriteLine($"{Brand} | Колір: {Color} | Ціна: {Price} грн (ПДВ: {CalcTax(Price)} грн)");
        }
    }
}