﻿// <auto-generated />
using System;
using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace WebApp.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20220630081233_mig3")]
    partial class mig3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Asset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("FileExtention")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasColumnName("ext");

                    b.Property<string>("FileName")
                        .HasMaxLength(260)
                        .HasColumnType("nvarchar(260)")
                        .HasColumnName("fileName");

                    b.Property<string>("MimeType")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("mime");

                    b.Property<string>("OriginalFileName")
                        .HasMaxLength(260)
                        .HasColumnType("nvarchar(260)")
                        .HasColumnName("original");

                    b.HasKey("Id");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("Entities.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<Guid>("CartId")
                        .HasMaxLength(50)
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CartId");

                    b.Property<DateTime>("DateCreated")
                        .HasMaxLength(50)
                        .HasColumnType("datetime2")
                        .HasColumnName("DateCreated");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasMaxLength(50)
                        .HasColumnType("int")
                        .HasColumnName("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Entities.CartItemOrder", b =>
                {
                    b.Property<Guid>("CartItemId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CartItem Id");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Order Id");

                    b.HasKey("CartItemId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("CartItemOrder");
                });

            modelBuilder.Entity("Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("City");

                    b.Property<string>("ClientName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ClientName");

                    b.Property<string>("ClientPhone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ClientPhone");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Email");

                    b.Property<int>("NovaPoshta")
                        .HasColumnType("int")
                        .HasColumnName("NovaPoshta");

                    b.Property<DateTime>("OrderDate")
                        .HasMaxLength(50)
                        .HasColumnType("datetime2")
                        .HasColumnName("OrderDate");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("About")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)")
                        .HasColumnName("About");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("Name");

                    b.Property<decimal>("RetailPrice")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Entities.ProductAsset", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Product Id");

                    b.Property<Guid>("AssetId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Asset Id");

                    b.HasKey("ProductId", "AssetId");

                    b.HasIndex("AssetId");

                    b.ToTable("ProductAsset");
                });

            modelBuilder.Entity("Entities.CartItem", b =>
                {
                    b.HasOne("Entities.Product", "Product")
                        .WithMany("cartItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Entities.CartItemOrder", b =>
                {
                    b.HasOne("Entities.CartItem", "CartItem")
                        .WithMany("CartItemOrders")
                        .HasForeignKey("CartItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Order", "Order")
                        .WithMany("CartItemOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartItem");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Entities.Product", b =>
                {
                    b.HasOne("Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Entities.ProductAsset", b =>
                {
                    b.HasOne("Entities.Asset", "Asset")
                        .WithMany("ProductAssets")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Product", "Product")
                        .WithMany("ProductAssets")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Entities.Asset", b =>
                {
                    b.Navigation("ProductAssets");
                });

            modelBuilder.Entity("Entities.CartItem", b =>
                {
                    b.Navigation("CartItemOrders");
                });

            modelBuilder.Entity("Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Entities.Order", b =>
                {
                    b.Navigation("CartItemOrders");
                });

            modelBuilder.Entity("Entities.Product", b =>
                {
                    b.Navigation("ProductAssets");

                    b.Navigation("cartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
