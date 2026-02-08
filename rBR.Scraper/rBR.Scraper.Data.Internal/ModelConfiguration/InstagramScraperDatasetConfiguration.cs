using Microsoft.EntityFrameworkCore;
using rBR.BaseLibraries.Domain.Entity;

namespace rBR.Scraper.Data.Internal.ModelConfiguration
{
    /// <summary>
    /// The class that receives the settings from the <see cref="InstagramScraperDataset"/> entity to the database through the Fluent API.
    /// </summary>
    public static class InstagramScraperDatasetConfiguration
    {
        /// <summary>
        /// The static method for the configuration of the <see cref="InstagramScraperDataset"/> model.
        /// </summary>
        /// <param name="modelBuilder">The API surface of <see cref="ModelBuilder"/> for associating application entities with database table objects.</param>
        /// <param name="databaseType">The indicator if the database is of the MySQL (0) or SQLServer (1) type.</param>
        internal static ModelBuilder ConfigureInstagramScraperDataset(this ModelBuilder modelBuilder, int databaseType)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Entity<InstagramScraperDataset>().ToTable("InstagramScraperDatasets");
                modelBuilder.Entity<InstagramScraperDataset>().HasKey("Id");

                if (databaseType == 0)
                {
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("varchar(36)").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Created).HasColumnType("varchar(33)").HasDefaultValueSql("sysdate()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Modified).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Removed).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Timestamp).HasColumnType("varchar(33)").IsRequired().HasColumnOrder(8);
                    modelBuilder.Entity<InstagramScraperDataset>().Ignore(x => x.FullObjectStructured);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.FullObjectData).HasColumnType("json").IsRequired().HasColumnOrder(11).HasColumnName("FullObject");
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.RunId).HasColumnType("varchar(36)").IsRequired().HasColumnOrder(9);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.InstagramProfileId).HasColumnType("varchar(36)").IsRequired().HasColumnOrder(10);
                }
                else if (databaseType == 1)
                {
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Created).HasColumnType("datetimeoffset").HasDefaultValueSql("sysdatetimeoffset()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Modified).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Removed).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Timestamp).HasColumnType("datetimeoffset").IsRequired().HasColumnOrder(8);
                    modelBuilder.Entity<InstagramScraperDataset>().Ignore(x => x.FullObjectStructured);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.FullObjectData).HasColumnType("nvarchar(max)").IsRequired().HasColumnOrder(11).HasColumnName("FullObject");
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.RunId).HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(9);
                    modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.InstagramProfileId).HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(10);
                }

                modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Status).HasColumnType("int").IsRequired().HasColumnOrder(4);
                modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.DataId).HasColumnType("varchar(50)").IsRequired().HasColumnOrder(5);
                modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.Url).HasColumnType("varchar(300)").IsRequired().HasColumnOrder(6);
                modelBuilder.Entity<InstagramScraperDataset>().Property(x => x.InputUrl).HasColumnType("varchar(300)").IsRequired().HasColumnOrder(7);

                modelBuilder.Entity<InstagramScraperDataset>().HasOne(x => x.Run).WithMany(y => y.InstagramPosts).HasForeignKey(x => x.RunId).OnDelete(DeleteBehavior.NoAction);
                modelBuilder.Entity<InstagramScraperDataset>().HasOne(x => x.InstagramProfile).WithMany(y => y.InstagramPosts).HasForeignKey(x => x.InstagramProfileId).OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<InstagramScraperDataset>().HasIndex(x => x.Id).IsUnique();
            }

            return modelBuilder;
        }
    }
}
