using Microsoft.EntityFrameworkCore;
using rBR.BaseLibraries.Domain.Entity;

namespace rBR.Scraper.Data.Internal.ModelConfiguration
{
    /// <summary>
    /// The class that receives the settings from the <see cref="CommonScraper"/> entity to the database through the Fluent API.
    /// </summary>
    public static class CommonScraperConfiguration
    {
        /// <summary>
        /// The static method for the configuration of the <see cref="CommonScraper"/> model.
        /// </summary>
        /// <param name="modelBuilder">The API surface of <see cref="ModelBuilder"/> for associating application entities with database table objects.</param>
        /// <param name="databaseType">The indicator if the database is of the MySQL (0) or SQLServer (1) type.</param>
        internal static ModelBuilder ConfigureCommonScraper(this ModelBuilder modelBuilder, int databaseType)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Entity<CommonScraper>().ToTable("CommonScrapers");
                modelBuilder.Entity<CommonScraper>().HasKey("Id");

                if (databaseType == 0)
                {
                    modelBuilder.Entity<CommonScraper>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("varchar(36)").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<CommonScraper>().Property(x => x.Created).HasColumnType("varchar(33)").HasDefaultValueSql("sysdate()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.Modified).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.Removed).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.CreatedAt).HasColumnType("varchar(33)").IsRequired().HasColumnOrder(9);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.ModifiedAt).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(10);
                }
                else if (databaseType == 1)
                {
                    modelBuilder.Entity<CommonScraper>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<CommonScraper>().Property(x => x.Created).HasColumnType("datetimeoffset").HasDefaultValueSql("sysdatetimeoffset()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.Modified).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.Removed).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.CreatedAt).HasColumnType("datetimeoffset").IsRequired().HasColumnOrder(9);
                    modelBuilder.Entity<CommonScraper>().Property(x => x.ModifiedAt).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(10);
                }

                modelBuilder.Entity<CommonScraper>().Property(x => x.Status).HasColumnType("int").IsRequired().HasColumnOrder(4);
                modelBuilder.Entity<CommonScraper>().Property(x => x.DataId).HasColumnType("varchar(50)").IsRequired().HasColumnOrder(5);
                modelBuilder.Entity<CommonScraper>().Property(x => x.Title).HasColumnType("varchar(100)").IsRequired().HasColumnOrder(6);
                modelBuilder.Entity<CommonScraper>().Property(x => x.Description).HasColumnType("text").IsRequired().HasColumnOrder(7);
                modelBuilder.Entity<CommonScraper>().Property(x => x.Name).HasColumnType("varchar(100)").IsRequired().HasColumnOrder(8);

                modelBuilder.Entity<CommonScraper>().HasMany(x => x.Runs).WithOne(y => y.Scraper);

                modelBuilder.Entity<CommonScraper>().HasIndex(x => x.Id).IsUnique();
                modelBuilder.Entity<CommonScraper>().HasIndex(x => new { x.DataId }).IsUnique();
            }

            return modelBuilder;
        }
    }
}
