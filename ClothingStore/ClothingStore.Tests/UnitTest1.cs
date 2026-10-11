using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClothingStore.Common;
using Xunit;

namespace ClothingStore.Tests
{
    public class CrudServiceAsyncTests
    {
        private readonly string _testFilePath = "test_store.json";

        [Fact]
        public async Task CreateAsync_ShouldAddElement_WhenElementIsValid()
        {
            var service = new CrudServiceAsync<TShirt>(_testFilePath);
            var item = TShirt.CreateNew();

            bool result = await service.CreateAsync(item);
            var retrieved = await service.ReadAsync(item.Id);

            Assert.True(result);
            Assert.NotNull(retrieved);
            Assert.Equal(item.Id, retrieved.Id);
        }

        [Fact]
        public async Task ReadAllAsync_Pagination_ShouldReturnCorrectAmount()
        {
            var service = new CrudServiceAsync<TShirt>(_testFilePath);
            for (int i = 0; i < 15; i++)
            {
                await service.CreateAsync(TShirt.CreateNew());
            }

            var pageItems = (await service.ReadAllAsync(page: 1, amount: 5)).ToList();

            Assert.Equal(5, pageItems.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingElement()
        {
            var service = new CrudServiceAsync<TShirt>(_testFilePath);
            var item = TShirt.CreateNew();
            await service.CreateAsync(item);

            item.Price = 9999m;
            bool updated = await service.UpdateAsync(item);
            var retrieved = await service.ReadAsync(item.Id);

            Assert.True(updated);
            Assert.Equal(9999m, retrieved.Price);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteElement()
        {
            var service = new CrudServiceAsync<TShirt>(_testFilePath);
            var item = TShirt.CreateNew();
            await service.CreateAsync(item);

            bool removed = await service.RemoveAsync(item);
            var retrieved = await service.ReadAsync(item.Id);

            Assert.True(removed);
            Assert.Null(retrieved);
        }

        [Fact]
        public async Task SaveAsync_ShouldCreateFileWithData()
        {
            var service = new CrudServiceAsync<TShirt>(_testFilePath);
            await service.CreateAsync(TShirt.CreateNew());

            bool saved = await service.SaveAsync();

            Assert.True(saved);
            Assert.True(File.Exists(_testFilePath));

            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}