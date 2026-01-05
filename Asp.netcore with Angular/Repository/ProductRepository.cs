using Asp.netcore_with_Angular.Data;
using Asp.netcore_with_Angular.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Asp.netcore_with_Angular.Repository
{
    public class ProductRepository
    {
        private readonly AppDbContext db;
        public ProductRepository(AppDbContext dbContext)
        {
            this.db = dbContext;
        }
        public async Task<List<Product>> getAll()
        {
            return await db.Products.ToListAsync();
        }
        public async Task saveAllProduct(Product pv)
        {
            await db.Products.AddAsync(pv);
            await db.SaveChangesAsync();
        }
        public async Task updateProduct(int id, Product pv)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
                throw new Exception("Product not found");
            product.ProductName = pv.ProductName;
            product.Price = pv.Price;
            product.Description = pv.Description;
            product.Rating = pv.Rating;
            product.Status = pv.Status;

            await db.SaveChangesAsync();
        }
        public async Task deleteProduct(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            db.Products.Remove(product);
            await db.SaveChangesAsync();
        }
    }
}
