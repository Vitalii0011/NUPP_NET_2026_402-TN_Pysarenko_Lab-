using System;

namespace ClothingStore.Common
{
    public class Customer : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int DiscountCardPercent { get; set; }

        public Customer() { }

        public void PrintCustomerCard()
        {
            Console.WriteLine($"Клієнт: {FullName} | Номер: {PhoneNumber} | Знижка: {DiscountCardPercent}%");
        }
    }
}