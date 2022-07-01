using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Context
{
    public class AppDataContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(
                    prod =>
                    {
                        prod.Property(p => p.RetailPrice)
                            .HasColumnType("money");

                        prod.HasOne(p => p.Category)
                            .WithMany(c => c.Products)
                            .HasForeignKey(p => p.CategoryId);

                        prod.HasMany(p => p.Assets)
                            .WithMany(a => a.Products)
                            .UsingEntity<ProductAsset>(
                                pa => pa.HasOne(p => p.Asset)
                                    .WithMany(a => a.ProductAssets)
                                    .HasForeignKey(a => a.AssetId),

                                pa => pa.HasOne(p => p.Product)
                                    .WithMany(a => a.ProductAssets)
                                    .HasForeignKey(a => a.ProductId),

                                pa => pa.HasKey(
                                    qa => new { qa.ProductId, qa.AssetId })
                                );

                       

                    });

            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Order>();

            modelBuilder.Entity<CartItem>(
                  cart =>
                  {
                      cart.HasOne(c => c.Product)
                          .WithMany(p => p.cartItems)
                          .HasForeignKey(c => c.ProductId);

                      cart.HasMany(c => c.Orders)
                           .WithMany(o => o.CartItems)
                           .UsingEntity<CartItemOrder>(
                               co => co.HasOne(c => c.Order)
                                   .WithMany(o => o.CartItemOrders)
                                   .HasForeignKey(o => o.OrderId),

                               co => co.HasOne(c => c.CartItem)
                                   .WithMany(c => c.CartItemOrders)
                                   .HasForeignKey(o => o.CartItemId),

                               co => co.HasKey(
                                   qa => new { qa.CartItemId, qa.OrderId })
                               );

                  });



                      base.OnModelCreating(modelBuilder);
        }
    }
}