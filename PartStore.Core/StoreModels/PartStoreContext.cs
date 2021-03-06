﻿using Microsoft.EntityFrameworkCore;

namespace PartStore.Core.StoreModels
{
    public partial class PartStoreContext : DbContext
    {
        public PartStoreContext()
        {
        }

        public PartStoreContext(DbContextOptions<PartStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<AccountTypes> AccountTypes { get; set; }
        public virtual DbSet<Banks> Banks { get; set; }
        public virtual DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<InvoiceTypes> InvoiceTypes { get; set; }
        public virtual DbSet<ItemParts> ItemParts { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Makes> Makes { get; set; }
        public virtual DbSet<Models> Models { get; set; }
        public virtual DbSet<Operations> Operations { get; set; }
        public virtual DbSet<Parts> Parts { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<PaymentTypes> PaymentTypes { get; set; }
        public virtual DbSet<Photos> Photos { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Years> Years { get; set; }
        public virtual DbSet<ClientInvoicesPayments> ClientInvoicesPayments { get; set; } // Client Statement

        // Unable to generate entity type for table 'dbo.ItemPhotos'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=PartStore;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountTypeId).HasColumnName("AccountTypeID");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.BalanceIn)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BalanceOut)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.More).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountTypeId)
                    .HasConstraintName("FK_Accounts_AccountTypes");
            });

            modelBuilder.Entity<AccountTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Banks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TypeId)
                    .HasColumnName("TypeID")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<InvoiceDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Discount)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.PartName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.SubTotal).HasColumnType("money");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetails_Invoices");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_InvoiceDetails_Items");
            });

            modelBuilder.Entity<Invoices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Archived).HasDefaultValueSql("((0))");

                entity.Property(e => e.AddDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AddTime).HasColumnType("datetime").HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Discount).HasColumnType("money");

                entity.Property(e => e.InvoiceNo).HasMaxLength(50);

                entity.Property(e => e.InvoiceTypeId).HasColumnName("InvoiceTypeID");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(50);

                entity.Property(e => e.IsCache).HasDefaultValueSql("((1))");

                entity.Property(e => e.NetAmount).HasColumnType("money");

                entity.Property(e => e.Notes).HasMaxLength(250);

                entity.Property(e => e.Tax)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalAmount).HasColumnType("money");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Invoices_Accounts");

                entity.HasOne(d => d.InvoiceType)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.InvoiceTypeId)
                    .HasConstraintName("FK_Invoices_InvoiceTypes");
            });

            modelBuilder.Entity<InvoiceTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ItemParts>(entity =>
            {
                entity.HasKey(e => e.PartId);

                entity.Property(e => e.PartId).HasColumnName("PartID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.AddDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AvgCost)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Barcode).HasMaxLength(500);

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.LastCost)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastPurchasedDate).HasColumnType("date");

                entity.Property(e => e.More).HasMaxLength(1000);

                entity.Property(e => e.PartId1).HasColumnName("Part_ID");

                entity.Property(e => e.PartName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SalePrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Starred).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemParts)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemParts_Items");

                entity.HasOne(d => d.PartId1Navigation)
                    .WithMany(p => p.ItemParts)
                    .HasForeignKey(d => d.PartId1)
                    .HasConstraintName("FK_ItemParts_Parts");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.AvgCost)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Barcode).HasMaxLength(1000);

                entity.Property(e => e.Discount)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastCost)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastPurchasedDate).HasColumnType("date");

                entity.Property(e => e.LotNo).HasMaxLength(50);

                entity.Property(e => e.MakeId).HasColumnName("MakeID");

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.More).HasMaxLength(1000);

                entity.Property(e => e.NetPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Photo).HasMaxLength(100);

                entity.Property(e => e.Qty).HasDefaultValueSql("((0))");

                entity.Property(e => e.RefNo).HasMaxLength(50);

                entity.Property(e => e.SalePrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Starred).HasDefaultValueSql("((0))");

                entity.Property(e => e.Sold).HasDefaultValueSql("((0))");

                entity.Property(e => e.SupplierCarNo).HasMaxLength(50);

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Vin)
                    .HasColumnName("VIN")
                    .HasMaxLength(60);

                entity.Property(e => e.YearId).HasColumnName("YearID");

                entity.HasOne(d => d.Make)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.MakeId)
                    .HasConstraintName("FK_Items_Makes");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ModelId)
                    .HasConstraintName("FK_Items_Models");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Items_Accounts");

                entity.HasOne(d => d.Year)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.YearId)
                    .HasConstraintName("FK_Items_Years");
            });

            modelBuilder.Entity<Makes>(entity =>
            {
                entity.HasKey(e => e.MakeId);

                entity.Property(e => e.MakeId).HasColumnName("MakeID");

                entity.Property(e => e.MakeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Models>(entity =>
            {
                entity.HasKey(e => e.ModelId);

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.MakeId).HasColumnName("MakeID");

                entity.Property(e => e.ModelName).HasMaxLength(50);

                entity.HasOne(d => d.Make)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.MakeId)
                    .HasConstraintName("FK_Models_Makes");
            });

            modelBuilder.Entity<Operations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Parts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Archived).HasDefaultValueSql("((0))");

                entity.Property(e => e.AddDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AddTime).HasColumnType("datetime").HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Discount)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Extra)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FromBankId).HasColumnName("FromBankID");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.Property(e => e.OperationId).HasColumnName("OperationID");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.RefNo).HasMaxLength(50);

                entity.Property(e => e.Tax)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ToBankId).HasColumnName("ToBankID");

                entity.Property(e => e.Total)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Payments_Accounts");

                entity.HasOne(d => d.FromBank)
                    .WithMany(p => p.PaymentsFromBank)
                    .HasForeignKey(d => d.FromBankId)
                    .HasConstraintName("FK_Payments_BanksFrom");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_Payments_Invoices");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OperationId)
                    .HasConstraintName("FK_Payments_Operations");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .HasConstraintName("FK_Payments_PaymentTypes");

                entity.HasOne(d => d.ToBank)
                    .WithMany(p => p.PaymentsToBank)
                    .HasForeignKey(d => d.ToBankId)
                    .HasConstraintName("FK_Payments_BanksTo");
            });

            modelBuilder.Entity<PaymentTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Photos>(entity =>
            {
                entity.HasKey(e => e.PhotoId);

                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.Main).HasDefaultValueSql("((0))");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Photos_Items");
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'all')");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AddDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Transactions_Accounts");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_Transactions_Banks");
            });

            modelBuilder.Entity<Years>(entity =>
            {
                entity.HasKey(e => e.YearId);

                entity.Property(e => e.YearId)
                    .HasColumnName("YearID")
                    .ValueGeneratedNever();
            });
        }
    }
}