using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Interface;
using rBR.Scraper.Domain.Interface.Context;

namespace rBR.Scraper.Domain.Interface.Repository
{
    /// <summary>
    /// The database access implementation class for objects of type <see cref="InstagramScraperDataset"/>.
    /// </summary>
    public interface IInstagramDatasetRepository : IBaseRepository<InstagramScraperDataset, IrBRScraperContext>
    {
    }
}
