using Microsoft.EntityFrameworkCore;
using rBR.BaseLibraries.Domain.Entity;

namespace rBR.Scraper.Data.Internal.ModelConfiguration
{
    /// <summary>
    /// The class that receives the settings from the <see cref="ScraperRun"/> entity to the database through the Fluent API.
    /// </summary>
    public static class ScraperRunConfiguration
    {
        /// <summary>
        /// The static method for the configuration of the <see cref="CommonScraper"/> model.
        /// </summary>
        /// <param name="modelBuilder">The API surface of <see cref="ModelBuilder"/> for associating application entities with database table objects.</param>
        /// <param name="databaseType">The indicator if the database is of the MySQL (0) or SQLServer (1) type.</param>
        internal static ModelBuilder ConfigureScraperRun(this ModelBuilder modelBuilder, int databaseType)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Entity<ScraperRun>().ToTable("ScrapersRuns");
                modelBuilder.Entity<ScraperRun>().HasKey("Id");

                if (databaseType == 0)
                {
                    modelBuilder.Entity<ScraperRun>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("varchar(36)").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<ScraperRun>().Property(x => x.Created).HasColumnType("varchar(33)").HasDefaultValueSql("sysdate()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.Modified).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.Removed).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.StartedAt).HasColumnType("varchar(33)").IsRequired().HasColumnOrder(8);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.FinishedAt).HasColumnType("varchar(33)").IsRequired().HasColumnOrder(9);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.ScraperId).HasColumnType("varchar(36)").IsRequired().HasColumnOrder(12);
                }
                else if (databaseType == 1)
                {
                    modelBuilder.Entity<ScraperRun>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<ScraperRun>().Property(x => x.Created).HasColumnType("datetimeoffset").HasDefaultValueSql("sysdatetimeoffset()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.Modified).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.Removed).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.StartedAt).HasColumnType("datetimeoffset").IsRequired().HasColumnOrder(8);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.FinishedAt).HasColumnType("datetimeoffset").IsRequired().HasColumnOrder(9);
                    modelBuilder.Entity<ScraperRun>().Property(x => x.ScraperId).HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(12);
                }

                modelBuilder.Entity<ScraperRun>().Property(x => x.Status).HasColumnType("int").IsRequired().HasColumnOrder(4);
                modelBuilder.Entity<ScraperRun>().Property(x => x.DataId).HasColumnType("varchar(50)").IsRequired().HasColumnOrder(5);
                modelBuilder.Entity<ScraperRun>().Property(x => x.DataStatus).HasColumnType("varchar(50)").IsRequired().HasColumnOrder(6);
                modelBuilder.Entity<ScraperRun>().Property(x => x.DatasetId).HasColumnType("varchar(50)").IsRequired().HasColumnOrder(7);
                modelBuilder.Entity<ScraperRun>().Property(x => x.Imported).HasColumnType("bit").IsRequired().HasColumnOrder(10);
                modelBuilder.Entity<ScraperRun>().Property(x => x.ImportingError).HasColumnType("text").IsRequired(false).HasColumnOrder(11);

                modelBuilder.Entity<ScraperRun>().HasOne(x => x.Scraper).WithMany(y => y.Runs).HasForeignKey(x => x.ScraperId).OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<ScraperRun>().HasMany(x => x.InstagramProfiles).WithOne(y => y.Run);
                modelBuilder.Entity<ScraperRun>().HasMany(x => x.InstagramPosts).WithOne(y => y.Run);

                modelBuilder.Entity<ScraperRun>().HasIndex(x => x.Id).IsUnique();
                modelBuilder.Entity<ScraperRun>().HasIndex(x => new { x.DataId, x.DatasetId }).IsUnique();
            }

            return modelBuilder;
        }
    }
}
