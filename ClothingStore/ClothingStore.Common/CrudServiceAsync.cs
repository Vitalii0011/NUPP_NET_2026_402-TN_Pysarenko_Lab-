using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClothingStore.Common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class, IEntity
    {
        private readonly ConcurrentDictionary<Guid, T> _storage = new();

        public string FilePath { get; set; }

        public CrudServiceAsync(string filePath = "store_async.json")
        {
            FilePath = filePath;
        }

        public Task<bool> CreateAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            bool added = _storage.TryAdd(element.Id, element);
            return Task.FromResult(added);
        }

        public Task<T> ReadAsync(Guid id)
        {
            _storage.TryGetValue(id, out T value);
            return Task.FromResult(value);
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            IEnumerable<T> allItems = _storage.Values.ToList();
            return Task.FromResult(allItems);
        }

        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            if (page < 1) page = 1;
            if (amount < 1) amount = 10;

            IEnumerable<T> pagedItems = _storage.Values
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();

            return Task.FromResult(pagedItems);
        }

        public Task<bool> UpdateAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (_storage.ContainsKey(element.Id))
            {
                _storage[element.Id] = element;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            bool removed = _storage.TryRemove(element.Id, out _);
            return Task.FromResult(removed);
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var itemsList = _storage.Values.ToList();

                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                {
                    await JsonSerializer.SerializeAsync(fileStream, itemsList, options);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _storage.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}