using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Interface;
using rBR.Scraper.Domain.Interface.Context;

namespace rBR.Scraper.Domain.Interface.Repository
{
    /// <summary>
    /// The database access implementation class for objects of type <see cref="ScraperRun"/>.
    /// </summary>
    public interface IScraperRunRepository : IBaseRepository<ScraperRun, IrBRScraperContext>
    {
    }
}
