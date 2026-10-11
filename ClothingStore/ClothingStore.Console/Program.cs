using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClothingStore.Common;

class Program
{
    private static readonly object _lockObject = new();
    private static readonly SemaphoreSlim _semaphore = new(2, 2); 
    private static readonly AutoResetEvent _autoResetEvent = new(false);

    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("==========================================================");
        Console.WriteLine("  ЛАБОРАТОРНА РОБОТА №2: Багатопоточність, LINQ, Async/Await");
        Console.WriteLine("==========================================================\n");

        var service = new CrudServiceAsync<TShirt>("clothing_store_parallel.json");
        const int itemsCount = 1500;
        Console.WriteLine($"1. Генерація та паралельне додавання {itemsCount} об'єктів (Parallel.For)...");

        var stopwatch = Stopwatch.StartNew();

        Parallel.For(0, itemsCount, _ =>
        {
            var newItem = TShirt.CreateNew();
            service.CreateAsync(newItem).Wait();
        });

        stopwatch.Stop();
        Console.WriteLine($"-> Успішно згенеровано. Час виконання: {stopwatch.ElapsedMilliseconds} мс.\n");

        Console.WriteLine("2. Обчислення характеристик за допомогою LINQ:");

        decimal minPrice = service.Min(x => x.Price);
        decimal maxPrice = service.Max(x => x.Price);
        decimal avgPrice = Math.Round(service.Average(x => x.Price), 2);
        decimal totalPrice = service.Sum(x => x.Price);

        Console.WriteLine($"   Мінімальна ціна  : {minPrice:F2} грн");
        Console.WriteLine($"   Максимальна ціна  : {maxPrice:F2} грн");
        Console.WriteLine($"   Середня ціна      : {avgPrice:F2} грн");
        Console.WriteLine($"   Сумарна вартість  : {totalPrice:F2} грн\n");

        Console.WriteLine("3. Демонстрація пагінації (Сторінка 1, 3 елементи):");
        var firstPage = await service.ReadAllAsync(page: 1, amount: 3);
        foreach (var item in firstPage)
        {
            item.ShowInfo();
        }
        Console.WriteLine();

        Console.WriteLine("4. Демонстрація роботи примітивів синхронізації:\n");

        Console.WriteLine("   [A] Демонстрація 'lock':");
        int sharedCounter = 0;
        Parallel.For(0, 5, i =>
        {
            lock (_lockObject)
            {
                sharedCounter++;
                Console.WriteLine($"       Потік {Environment.CurrentManagedThreadId} збільшив лічильник до {sharedCounter}");
            }
        });

        Console.WriteLine("\n   [Б] Демонстрація 'SemaphoreSlim' (ліміт: максимум 2 потоки):");
        var semaphoreTasks = Enumerable.Range(1, 4).Select(async id =>
        {
            await _semaphore.WaitAsync();
            try
            {
                Console.WriteLine($"       -> Потік #{id} увійшов у критичну секцію");
                await Task.Delay(200);
            }
            finally
            {
                Console.WriteLine($"       <- Потік #{id} звільнив семафор");
                _semaphore.Release();
            }
        });
        await Task.WhenAll(semaphoreTasks);

        Console.WriteLine("\n   [В] Демонстрація 'AutoResetEvent' (сигнал між потоками):");
        var workerThread = new Thread(() =>
        {
            Console.WriteLine("       [Фоновий потік]: Чекаю на сигнал від головного потоку...");
            _autoResetEvent.WaitOne();
            Console.WriteLine("       [Фоновий потік]: Сигнал отримано! Продовжую роботу.");
        });
        workerThread.Start();

        Thread.Sleep(300);
        Console.WriteLine("       [Головний потік]: Відправляю сигнал (Set)...");
        _autoResetEvent.Set();
        workerThread.Join();

        Console.WriteLine("\n5. Асинхронне збереження всієї колекції у файл...");
        bool isSaved = await service.SaveAsync();
        Console.WriteLine($"   Результат збереження у '{service.FilePath}': {(isSaved ? "Успішно" : "Помилка")}");

    }
}