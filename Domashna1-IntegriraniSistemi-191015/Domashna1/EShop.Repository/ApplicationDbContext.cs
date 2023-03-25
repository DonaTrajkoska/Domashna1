using EShop.Domain.DomainModels;
using EShop.Web.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace EShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<EShopApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options)
        {

        }
        public virtual DbSet<Billet> Bilets { get; set; }
        public virtual DbSet<ShoppingCard> ShoppingCards { get; set; }
        public virtual DbSet<BiletInShoppingCard> BiletInShoppingCards { get; set; }
        public virtual DbSet<BiletInOrder> BiletInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ShoppingCard>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Billet>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            //builder.Entity<BiletInShoppingCard>()
              //  .HasKey(z => new { z.BiletId, z.ShoppingCartId });

            builder.Entity<BiletInShoppingCard>()
                .HasOne(z => z.Bilet)
                .WithMany(z => z.BiletInShoppingCards)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<BiletInShoppingCard>()
                .HasOne(z => z.ShoppingCard)
                .WithMany(z => z.BiletInShoppingCards)
                .HasForeignKey(z => z.BiletId);

            builder.Entity<ShoppingCard>()
                .HasOne<EShopApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCard)
                .HasForeignKey<ShoppingCard>(z => z.OwnerId);

           // builder.Entity<BiletInOrder>()
             //   .HasKey(z => new { z.BiletId, z.OrderId });

            builder.Entity<BiletInOrder>()
               .HasOne(z => z.SelectedBilet)
               .WithMany(z => z.Orders)
               .HasForeignKey(z => z.BiletId);

            builder.Entity<BiletInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.Bilets)
                .HasForeignKey(z => z.OrderId);
        }
    }
}
