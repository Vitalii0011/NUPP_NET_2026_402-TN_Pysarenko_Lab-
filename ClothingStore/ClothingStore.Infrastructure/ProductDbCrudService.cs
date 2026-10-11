using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClothingStore.Common;
using ClothingStore.Infrastructure.Models;
using ClothingStore.Infrastructure.Repositories;

namespace ClothingStore.Infrastructure
{
    public class ProductDbCrudService : ICrudServiceAsync<TShirtModel>
    {
        private readonly ClothingStoreContext _context;
        private readonly IRepository<TShirtModel> _repository;

        public ProductDbCrudService(ClothingStoreContext context, IRepository<TShirtModel> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<bool> CreateAsync(TShirtModel element)
        {
            if (element == null) return false;
            await _repository.AddAsync(element);
            return true;
        }

        public async Task<TShirtModel?> ReadAsync(Guid id)
        {
            return await _context.TShirts
                .Include(t => t.Category)
                .Include(t => t.Details)
                .Include(t => t.Orders)
                .FirstOrDefaultAsync(t => t.ExternalId == id);
        }

        public async Task<IEnumerable<TShirtModel>> ReadAllAsync()
        {
            return await _context.TShirts
                .Include(t => t.Category)
                .Include(t => t.Details)
                .Include(t => t.Orders)
                .ToListAsync();
        }

        public async Task<IEnumerable<TShirtModel>> ReadAllAsync(int page, int amount)
        {
            if (page < 1) page = 1;
            if (amount < 1) amount = 10;

            return await _context.TShirts
                .Include(t => t.Category)
                .Include(t => t.Details)
                .Include(t => t.Orders)
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(TShirtModel element)
        {
            if (element == null) return false;
            await _repository.Update(element);
            return true;
        }

        public async Task<bool> RemoveAsync(TShirtModel element)
        {
            if (element == null) return false;
            await _repository.Delete(element);
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }

        public IEnumerator<TShirtModel> GetEnumerator()
        {
            return _context.TShirts.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}