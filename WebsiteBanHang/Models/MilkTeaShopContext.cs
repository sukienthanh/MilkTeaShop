using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MilkTeaShop.Models
{
    public partial class MilkTeaShopContext : DbContext
    {
        public MilkTeaShopContext()
        {
        }

        public MilkTeaShopContext(DbContextOptions<MilkTeaShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<About> Abouts { get; set; } = null!;
        public virtual DbSet<Catalog> Catalogs { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<About>(entity =>
            {
                entity.ToTable("About");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasMaxLength(200);
            });

            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.HasKey(e => e.CataId)
                    .HasName("PK__Catalog__CC584C7652B9100B");

                entity.ToTable("Catalog");

                entity.Property(e => e.CataId).HasColumnName("Cata_Id");

                entity.Property(e => e.CataName)
                    .HasMaxLength(50)
                    .HasColumnName("Cata_Name");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerAddress)
                    .HasMaxLength(300)
                    .HasColumnName("Customer_Address");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(50)
                    .HasColumnName("Customer_Name");

                entity.Property(e => e.CustomerPhone)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("Customer_Phone");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnName("Discount_Amount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.OrderNote)
                    .HasMaxLength(300)
                    .HasColumnName("Order_Note");

                entity.Property(e => e.ShippingCost)
                    .HasColumnName("Shipping_Cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.Property(e => e.TotalAmount).HasColumnName("Total_Amount");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("Order_Item");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.Note).HasMaxLength(300);

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Order_Ite__Order__078C1F06");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Order_Ite__Produ__0880433F");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Content).HasMaxLength(300);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Payment__Order_I__0C50D423");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Payment__User_Id__0E391C95");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.CatalogId, "IX_Product_Catalog_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.CatalogId).HasColumnName("Catalog_Id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("Product_Name");

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatalogId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Product__Catalog__30F848ED");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descriptions).HasMaxLength(300);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("Role_Name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("Full_Name");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("User_Name");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Users__RoleId__2BFE89A6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
