using Microsoft.EntityFrameworkCore;
using rBR.BaseLibraries.Domain.Entity;

namespace rBR.Scraper.Data.Internal.ModelConfiguration
{
    /// <summary>
    /// The class that receives the settings from the <see cref="InstagramProfileScraperDataset"/> entity to the database through the Fluent API.
    /// </summary>
    public static class InstagramProfileScraperDatasetConfiguration
    {
        /// <summary>
        /// The static method for the configuration of the <see cref="InstagramProfileScraperDataset"/> model.
        /// </summary>
        /// <param name="modelBuilder">The API surface of <see cref="ModelBuilder"/> for associating application entities with database table objects.</param>
        /// <param name="databaseType">The indicator if the database is of the MySQL (0) or SQLServer (1) type.</param>
        internal static ModelBuilder ConfigureInstagramProfileScraperDataset(this ModelBuilder modelBuilder, int databaseType)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Entity<InstagramProfileScraperDataset>().ToTable("InstagramProfileScraperDatasets");
                modelBuilder.Entity<InstagramProfileScraperDataset>().HasKey("Id");

                if (databaseType == 0)
                {
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("varchar(36)").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Created).HasColumnType("varchar(33)").HasDefaultValueSql("sysdate()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Modified).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Removed).HasColumnType("varchar(33)").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Ignore(x => x.FullObjectStructured);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.FullObjectData).HasColumnType("json").IsRequired().HasColumnOrder(14).HasColumnName("FullObject");
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.RunId).HasColumnType("varchar(36)").IsRequired().HasColumnOrder(13);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Timestamp).HasColumnType("varchar(33)").IsRequired().HasColumnOrder(12);
                }
                else if (databaseType == 1)
                {
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(0);

                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Created).HasColumnType("datetimeoffset").HasDefaultValueSql("sysdatetimeoffset()").IsRequired().HasColumnOrder(1);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Modified).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(2);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Removed).HasColumnType("datetimeoffset").IsRequired(false).HasColumnOrder(3);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Ignore(x => x.FullObjectStructured);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.FullObjectData).HasColumnType("nvarchar(max)").IsRequired().HasColumnOrder(14).HasColumnName("FullObject");
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.RunId).HasColumnType("uniqueidentifier").IsRequired().HasColumnOrder(13);
                    modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Timestamp).HasColumnType("datetimeoffset").IsRequired().HasColumnOrder(12);
                }

                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Status).HasColumnType("int").IsRequired().HasColumnOrder(4);
                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.DataId).HasColumnType("varchar(50)").IsRequired().HasColumnOrder(5);
                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Url).HasColumnType("varchar(300)").IsRequired().HasColumnOrder(6);
                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.InputUrl).HasColumnType("varchar(300)").IsRequired().HasColumnOrder(7);
                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.UserName).HasColumnType("varchar(150)").IsRequired().HasColumnOrder(8);
                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.FullName).HasColumnType("varchar(150)").IsRequired().HasColumnOrder(9);
                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.FollowersCount).HasColumnType("int").IsRequired(false).HasColumnOrder(10);
                modelBuilder.Entity<InstagramProfileScraperDataset>().Property(x => x.Verified).HasColumnType("bit").IsRequired(false).HasColumnOrder(11);

                modelBuilder.Entity<InstagramProfileScraperDataset>().HasOne(x => x.Run).WithMany(y => y.InstagramProfiles).HasForeignKey(x => x.RunId).OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<InstagramProfileScraperDataset>().HasMany(x => x.InstagramPosts).WithOne(y => y.InstagramProfile);

                modelBuilder.Entity<InstagramProfileScraperDataset>().HasIndex(x => x.Id).IsUnique();
                modelBuilder.Entity<InstagramProfileScraperDataset>().HasIndex(x => new { x.UserName, x.InputUrl, x.DataId, x.RunId }).IsUnique();
            }

            return modelBuilder;
        }
    }
}
