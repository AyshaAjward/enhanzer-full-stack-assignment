protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Location>().ToTable("Location_Details");
    modelBuilder.Entity<Location>().HasKey(x => x.Id);

    modelBuilder.Entity<PurchaseBill>().ToTable("Purchase_Bills");
    modelBuilder.Entity<PurchaseBill>().HasKey(x => x.Id);

    modelBuilder.Entity<PurchaseBillItem>().ToTable("Purchase_Bill_Items");
    modelBuilder.Entity<PurchaseBillItem>().HasKey(x => x.Id);

    modelBuilder.Entity<PurchaseBillItem>()
        .Property(x => x.Standard_Cost)
        .HasPrecision(18, 2);

    modelBuilder.Entity<PurchaseBillItem>()
        .Property(x => x.Standard_Price)
        .HasPrecision(18, 2);

    modelBuilder.Entity<PurchaseBillItem>()
        .Property(x => x.Margin)
        .HasPrecision(18, 2);

    modelBuilder.Entity<PurchaseBillItem>()
        .Property(x => x.Discount)
        .HasPrecision(5, 2);

    modelBuilder.Entity<PurchaseBillItem>()
        .Property(x => x.Total_Cost)
        .HasPrecision(18, 2);

    modelBuilder.Entity<PurchaseBillItem>()
        .Property(x => x.Total_Selling)
        .HasPrecision(18, 2);
}