using System;
using System.Linq;
using ClothingStore.Common;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine($"=== Вас вітає: {Product.StoreName} ===\n");

        var tshirtService = new CrudService<TShirt>();

        var tshirt1 = new TShirt
        {
            Brand = "Nike Sportswear",
            Color = "Чорний",
            Size = "L",
            Material = "Бавовна",
            HasPrint = true,
            Price = 1200m
        };

        tshirt1.PriceChanged += message => Console.WriteLine(message);

        Console.WriteLine("1. Створення елементів (Create):");
        tshirtService.Create(tshirt1);

        var tshirt2 = new TShirt
        {
            Brand = "Adidas Originals",
            Color = "Білий",
            Size = "M",
            Material = "Поліестер",
            HasPrint = false,
            Price = 950m
        };
        tshirtService.Create(tshirt2);
        Console.WriteLine("Товари успішно додано до сервісу.");

        Console.WriteLine("\n2. Виклик методу розширення (знижка 15%):");
        tshirt1.ApplyDiscount(15);

        Console.WriteLine("\n3. Перегляд списку товарів (ReadAll):");
        foreach (var item in tshirtService.ReadAll())
        {
            item.ShowInfo();
        }

        Console.WriteLine("\n4. Оновлення товару (Update):");
        tshirt2.Price = 850m;
        tshirt2.Color = "Молочний";
        tshirtService.Update(tshirt2);
        Console.WriteLine($"Оновлений Adidas: {tshirtService.Read(tshirt2.Id).Color}, ціна: {tshirtService.Read(tshirt2.Id).Price} грн");

        string path = "clothing_backup.json";
        Console.WriteLine($"\n5. Збереження списку товарів у JSON-файл '{path}'...");
        tshirtService.Save(path);

        Console.WriteLine("Завантаження даних у новий сервіс із файлу:");
        var restoredService = new CrudService<TShirt>();
        restoredService.Load(path);
        Console.WriteLine($"Успішно відновлено об'єктів: {restoredService.ReadAll().Count()} шт.");

        Console.WriteLine("\n6. Видалення товару (Remove):");
        restoredService.Remove(tshirt1);
        Console.WriteLine($"Кількість після видалення: {restoredService.ReadAll().Count()} шт.");

        Console.WriteLine($"\nВсього створено товарів: {Product.TotalItemsCreated}");
    }
}