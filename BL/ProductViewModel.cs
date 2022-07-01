using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace BL
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string About { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        [Required]
        public decimal RetailPrice { get; set; }

        public decimal WholesalePrice { get; set; }
        public List<Guid> ImageIds { get; set; } = new List<Guid>();
        public Guid? Asset { get; set; }

        public ProductViewModel()
        {
        }

        public bool IsEmpty
        {
            get => Name == null;
        }

        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            About = product.About;  
            RetailPrice = product.RetailPrice;
            CategoryName = product.Category?.Name;
            CategoryId = product.CategoryId;
            ImageIds = product.Assets.Select(a => a.Id).ToList();
        }

        public static implicit operator Product(ProductViewModel model)
        {
            return new Product
            {
                Id = model.Id,
                Description = model.Description,
                About = model.About,
                RetailPrice = model.RetailPrice,
                ProductAssets = model.ImageIds.Select(i => new ProductAsset { AssetId = i, ProductId = model.Id }).ToList(),
                CategoryId = model.CategoryId,
                Name = model.Name
            };
        }

        public static ProductViewModel GetProductById(Guid id, IProductRepository repository)
        {
            return (repository.AllItems as DbSet<Product>)
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .Include(p => p.Assets)
                .Select(p => new ProductViewModel(p))
                .FirstOrDefault();
        }

        public static async Task<bool> Delete(Guid id, IProductRepository repository)
        {
            return await repository.DeleteItemAsync(id);
        }

        public static async Task<bool> Edit(ProductViewModel model, IProductRepository repository)
        {
            return await repository.ChangeItemAsync(model);
        }

        public static IQueryable<ProductViewModel> GetProductList(IProductRepository repository, Guid? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                return repository.AllItems
                    .Where(p => p.CategoryId == categoryId)
                    .Include(p => p.Assets)
                    .Select(p => new ProductViewModel(p));
            }
            return (repository.AllItems as DbSet<Product>)
                .Include(p => p.Assets)
                .Include(p => p.Category)
                .Select(p => new ProductViewModel(p));
        }
    }
}