using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClothingStore.Infrastructure;
using ClothingStore.Infrastructure.Models;
using ClothingStore.Infrastructure.Repositories;

namespace ClothingStore.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №3: Entity Framework Core та SQLite ===");

            using var context = new ClothingStoreContext();
            await context.Database.MigrateAsync();

            var repository = new Repository<TShirtModel>(context);
            var crudService = new ProductDbCrudService(context, repository);
            if (!context.Categories.Any())
            {
                Console.WriteLine("\n[1] Ініціалізація бази даних початковими записами...");

                var casualCategory = new CategoryModel { Name = "Повсякденний одяг" };
                var sportCategory = new CategoryModel { Name = "Спортивний одяг" };
                await context.Categories.AddRangeAsync(casualCategory, sportCategory);
                await context.SaveChangesAsync();

                var customerOrder = new OrderModel
                {
                    CustomerName = "Олександр Коваленко",
                    OrderDate = DateTime.UtcNow
                };
                await context.Orders.AddAsync(customerOrder);
                await context.SaveChangesAsync();

                var tshirt1 = new TShirtModel
                {
                    Brand = "Nike",
                    Color = "Чорний",
                    Price = 1299.99m,
                    Size = "L",
                    Material = "100% Бавовна",
                    HasPrint = true,
                    CategoryId = sportCategory.Id,
                    Details = new ProductDetailsModel
                    {
                        Sku = "NK-TSH-001",
                        CountryOfOrigin = "В'єтнам",
                        CareInstructions = "Прання при 30°C, не відбілювати"
                    }
                };
                tshirt1.Orders.Add(customerOrder);

                var tshirt2 = new TShirtModel
                {
                    Brand = "Zara",
                    Color = "Білий",
                    Price = 799.50m,
                    Size = "M",
                    Material = "Органічна бавовна",
                    HasPrint = false,
                    CategoryId = casualCategory.Id,
                    Details = new ProductDetailsModel
                    {
                        Sku = "ZR-TSH-002",
                        CountryOfOrigin = "Португалія",
                        CareInstructions = "Делікатне прання"
                    }
                };
                tshirt2.Orders.Add(customerOrder);

                var tshirt3 = new TShirtModel
                {
                    Brand = "Adidas",
                    Color = "Синій",
                    Price = 1150.00m,
                    Size = "XL",
                    Material = "Поліестер ClimaCool",
                    HasPrint = true,
                    CategoryId = sportCategory.Id,
                    Details = new ProductDetailsModel
                    {
                        Sku = "AD-TSH-003",
                        CountryOfOrigin = "Камбоджа",
                        CareInstructions = "Швидке сушіння"
                    }
                };

                await crudService.CreateAsync(tshirt1);
                await crudService.CreateAsync(tshirt2);
                await crudService.CreateAsync(tshirt3);

                Console.WriteLine("Базу даних успішно заповнено тестовими записами!");
            }

            Console.WriteLine("\n[2] Отримання всіх товарів із БД через CRUD-сервіс (включаючи зв'язки):");
            var allProducts = await crudService.ReadAllAsync();
            foreach (var item in allProducts)
            {
                Console.WriteLine($"--------------------------------------------------");
                Console.WriteLine($"ID: {item.Id} | Бренд: {item.Brand} | Колір: {item.Color} | Ціна: {item.Price:C}");
                Console.WriteLine($"Розмір: {item.Size} | Матеріал: {item.Material} | Принт: {(item.HasPrint ? "Так" : "Ні")}");
                Console.WriteLine($"Категорія (1-до-багатьох): {item.Category?.Name ?? "Не вказано"}");
                Console.WriteLine($"Деталі (1-до-1): Артикул {item.Details?.Sku}, Країна: {item.Details?.CountryOfOrigin}");
                Console.WriteLine($"Кількість замовлень (багато-до-багатьох): {item.Orders.Count}");
            }

            Console.WriteLine("\n[3] Демонстрація пагінації (Сторінка 1, по 2 елементи):");
            var pagedProducts = await crudService.ReadAllAsync(page: 1, amount: 2);
            foreach (var item in pagedProducts)
            {
                Console.WriteLine($"-> {item.Brand} ({item.Color}, розмір {item.Size}) - {item.Price:C}");
            }

            Console.WriteLine("\nРоботу з базою даних SQLite успішно завершено!");
            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}