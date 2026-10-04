using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ClothingStore.Common
{
    public interface ICrudService<T>
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }

    public class CrudService<T> : ICrudService<T> where T : class, IEntity
    {
        private readonly List<T> _items = new();

        public void Create(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            _items.Add(element);
        }

        public T Read(Guid id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> ReadAll()
        {
            return _items;
        }

        public void Update(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var index = _items.FindIndex(x => x.Id == element.Id);
            if (index != -1)
            {
                _items[index] = element;
            }
            else
            {
                throw new KeyNotFoundException($"Елемент із Id {element.Id} не знайдено.");
            }
        }

        public void Remove(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            _items.RemoveAll(x => x.Id == element.Id);
        }

        public void Save(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл {filePath} не знайдено.");
            }

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<List<T>>(json);
            _items.Clear();
            if (data != null)
            {
                _items.AddRange(data);
            }
        }
    }
}