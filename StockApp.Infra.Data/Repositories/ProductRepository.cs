using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace StockApp.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        ApplicationDbContext _productContext;
        public ProductRepository(ApplicationDbContext context)
        {
            _productContext = context;
        }

        public async Task<Product> Create(Product product)
        {
            _productContext.Add(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetById(int? id)
        {
            return await _productContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productContext.Products.ToListAsync();
        }

        public async Task<Product> Remove(Product product)
        {
            _productContext.Remove(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> Update(Product product)
        {
            _productContext.Update(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        #region Filtro Com Ordenação
        public async Task<IEnumerable<Product>> SearchAsync(ProductFilters filters)
        {
            var query = _productContext.Products.AsQueryable();

            // Filtro por texto
            if (!string.IsNullOrWhiteSpace(filters.Query))
            {
                query = query.Where(p => p.Name.Contains(filters.Query) || p.Description.Contains(filters.Query));
            }

            // Filtro por faixa de preço
            if (filters.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filters.MinPrice.Value);
            }

            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filters.MaxPrice.Value);
            }

            // Ordenação
            if (!string.IsNullOrWhiteSpace(filters.SortBy))
            {
                query = filters.Descending
                    ? query.OrderByDescending(e => EF.Property<object>(e, filters.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filters.SortBy));
            }

            return await query.ToListAsync();
        }
        #endregion
    }
}
